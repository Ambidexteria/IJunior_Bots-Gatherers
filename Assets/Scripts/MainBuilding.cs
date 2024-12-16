using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainBuilding : SpawnableObject, IBuilding, IPickable
{
    [SerializeField] private List<Bot> _bots;
    [SerializeField] private int _maxBots = 5;
    [SerializeField] private int _minBots = 1;
    [SerializeField] private int _resourcesCount;
    [SerializeField] private ResourceCollector _resourceCollector;
    [SerializeField] private Transform _botSpawnPosition;
    [SerializeField] private ColonizationController _colonizationController;
    [SerializeField] private int _botPrice = 3;
    [SerializeField] private int _mainBuildingPrice = 5;
    [SerializeField] private MainBuildingFlag _mainBuildingFlag;

    private ResourceScanerDatabase _resourceScanerDatabase;
    private BotSpawner _botSpawner;
    private MainBuildingSpawner _mainBuildingSpawner;

    public event Action<int> ResourcesCountChanged;
    public event Action<Vector3> ConstructionFlagSet;

    public bool IsResourcesEnoughForConstructionNewMainBuilding => _resourcesCount >= _mainBuildingPrice;
    public bool IsBotCanBeCreated => _resourcesCount >= _botPrice && _bots.Count < _maxBots;

    private void OnEnable()
    {
        _colonizationController.PositionPicked += SetFlagForConstructionNewMainBuilding;
    }

    private void OnDisable()
    {
        _colonizationController.PositionPicked -= SetFlagForConstructionNewMainBuilding;
    }

    private void Update()
    {
        GatherResources();
    }

    public void SetFlagForConstructionNewMainBuilding(Vector3 placePosition)
    {
        if(_bots.Count > _minBots && _mainBuildingFlag.IsConstructionStarted == false)
        {
            ConstructionFlagSet?.Invoke(placePosition);
            _mainBuildingFlag.Place(placePosition);
        }
    }

    public void CreateNewBot()
    {
        Bot newBot = _botSpawner.Spawn();
        _bots.Add(newBot);
        newBot.transform.position = _botSpawnPosition.position;

        _resourcesCount -= _botPrice;
        ResourcesCountChanged?.Invoke(_resourcesCount);
    }

    public bool TrySendBotForConstruction()
    {
        if (TryGetIdleBot(out Bot idleBot) && IsResourcesEnoughForConstructionNewMainBuilding)
        {
            _mainBuildingFlag.StartConstruction();

            MainBuilding mainBuilding = _mainBuildingSpawner.Spawn();
            mainBuilding.gameObject.SetActive(false);
            mainBuilding.AddBot(idleBot);

            _bots.Remove(idleBot);

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
        if(_bots.Count < _maxBots)
        {
            _bots.Add(bot);
        }
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
        Debug.Log("mainBUilding.Pick()");
        _colonizationController.PickPlaceForNewMainBuilding();
    }

    [Inject]
    private void Construct(BotSpawner botSpawner, ResourceScanerDatabase scanerDatabase, MainBuildingSpawner mainBuildingSpawner)
    {
        _botSpawner = botSpawner;
        _resourceScanerDatabase = scanerDatabase;
        _mainBuildingSpawner = mainBuildingSpawner;
    }

    private void GatherResources()
    {
        if (TryGetIdleBot(out Bot idleBot))
        {
            SendBotForGatheringResource(idleBot);
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
        _resourcesCount -= _mainBuildingPrice;
        ResourcesCountChanged?.Invoke(_resourcesCount);
    }

    private bool TryGetIdleBot(out Bot bot)
    {
        bot = null;

        foreach (var tempBot in _bots)
        {
            if (tempBot.State == BotState.Idle)
            {
                bot = tempBot;
                return true;
            }
        }

        return false;
    }
}
