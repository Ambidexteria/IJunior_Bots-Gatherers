using System;
using UnityEngine;
using Zenject;

public class MainBuilding : SpawnableObject, IBuilding, IPickable
{
    [SerializeField] private MainBuildingBotsDatabase _botsDatabase;
    [SerializeField] private int _resourcesCount;
    [SerializeField] private ResourceCollector _resourceCollector;
    [SerializeField] private Transform _botSpawnPosition;
    [SerializeField] private ColonizationController _colonizationController;
    [SerializeField] private int _mainBuildingConstructionPrice = 5;
    [SerializeField] private float _mainBuildingSize = 5f;
    [SerializeField] private int _botConstructionPrice = 3;
    [SerializeField] private float _constructionTime = 5f;
    [SerializeField] private MainBuildingFlag _mainBuildingFlag;

    private ResourceScanerDatabase _resourceScanerDatabase;
    private BotSpawner _botSpawner;
    private MainBuildingSpawner _mainBuildingSpawner;

    public event Action<int> ResourcesCountChanged;
    public event Action<Vector3> ConstructionFlagSet;

    public bool IsResourcesEnoughForConstructionNewMainBuilding => _resourcesCount >= _mainBuildingConstructionPrice;
    public float ConstructionTime => _constructionTime;
    public float Size => _mainBuildingSize;

    private void OnEnable()
    {
        _colonizationController.PositionPicked += SetFlagForConstructionNewMainBuilding;
    }

    private void OnDisable()
    {
        _colonizationController.PositionPicked -= SetFlagForConstructionNewMainBuilding;
    }

    public void GatherResources()
    {
        if (_botsDatabase.TryGetIdleBot(out Bot idleBot))
        {
            SendBotForGatheringResource(idleBot);
        }
    }

    public void CreateNewBot()
    {
        if (IsBotCanBeCreated())
        {
            Bot newBot = _botSpawner.Spawn();
            _botsDatabase.AddNewBot(newBot);
            newBot.transform.position = _botSpawnPosition.position;

            _resourcesCount -= _botConstructionPrice;
            ResourcesCountChanged?.Invoke(_resourcesCount);
        }
    }

    public bool TrySendBotForConstruction()
    {
        if (_botsDatabase.TryGetIdleBot(out Bot idleBot) && IsResourcesEnoughForConstructionNewMainBuilding)
        {
            _mainBuildingFlag.StartConstruction();

            MainBuilding mainBuilding = _mainBuildingSpawner.Spawn();
            mainBuilding.gameObject.SetActive(false);
            mainBuilding.AddBot(idleBot);

            _botsDatabase.RemoveBot(idleBot);

            SendBotForConstruction(idleBot, mainBuilding);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddBot(Bot bot)
    {
        _botsDatabase.AddNewBot(bot);
    }

    public void Place(Vector3 position)
    {
        transform.position = position;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Pick()
    {
        _colonizationController.PickPlaceForNewMainBuilding();
    }

    [Inject]
    private void Construct(BotSpawner botSpawner, ResourceScanerDatabase scanerDatabase, 
        MainBuildingSpawner mainBuildingSpawner)
    {
        _botSpawner = botSpawner;
        _resourceScanerDatabase = scanerDatabase;
        _mainBuildingSpawner = mainBuildingSpawner;
    }

    private bool IsBotCanBeCreated() => _resourcesCount >= _botConstructionPrice && _botsDatabase.CanAddNewBot;

    private void SetFlagForConstructionNewMainBuilding(Vector3 placePosition)
    {
        if (_botsDatabase.BotsCountBiggerThanMin && _mainBuildingFlag.IsConstructionStarted == false)
        {
            ConstructionFlagSet?.Invoke(placePosition);
            _mainBuildingFlag.Place(placePosition);
        }
    }

    private void Collect(Resource resource)
    {
        _resourcesCount++;
        resource.Collected -= Collect;
        ResourcesCountChanged?.Invoke(_resourcesCount);
    }

    private void SendBotForGatheringResource(Bot bot)
    {
        if (_resourceScanerDatabase.TryGetNearestResourceForGathering(out Resource resource, bot.transform.position))
        {
            bot.SendForGatheringResource(resource, _resourceCollector.transform);
            resource.Collected += Collect;
        }
    }

    private void SendBotForConstruction(Bot bot, IBuilding building)
    {
        bot.SendForConstructionMainBuilding(_mainBuildingFlag, building);
        _resourcesCount -= _mainBuildingConstructionPrice;
        ResourcesCountChanged?.Invoke(_resourcesCount);
    }
}
