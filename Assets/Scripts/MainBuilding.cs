using System;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : MonoBehaviour, IBuilding
{
    [SerializeField] private List<Bot> _bots;
    [SerializeField] private int _maxBots = 5;
    [SerializeField] private int _minBots = 1;
    [SerializeField] private int _resourcesCount;
    [SerializeField] private ResourceCollector _resourceCollector;
    [SerializeField] private ResourceScanerDatabase _resourceScanerDatabase;
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private Transform _botSpawnPosition;
    [SerializeField] private int _botPrice = 3;
    [SerializeField] private int _mainBuildingPrice = 5;
    [SerializeField] private MainBuildingFlag _mainBuildingFlag;

    public event Action<int> ResourcesCountChanged;
    public event Action<Vector3> ConstructionFlagSet;

    public bool IsResourcesEnoughForConstructionNewMainBuilding => _resourcesCount >= _mainBuildingPrice;
    public bool IsBotCanBeCreated => _resourcesCount >= _botPrice && _bots.Count < _maxBots;

    private void Awake()
    {
        _resourceScanerDatabase = FindFirstObjectByType<ResourceScanerDatabase>();
        _botSpawner = FindFirstObjectByType<BotSpawner>();

        if (_resourceScanerDatabase == null)
            throw new NullReferenceException();

        if (_botSpawner == null)
            throw new NullReferenceException();
    }

    private void OnEnable()
    {
        if (_bots.Count > 0)
            foreach (var bot in _bots)
                bot.MainBuildingChanged += RemoveBot;
    }

    private void OnDisable()
    {
        if (_bots.Count > 0)
            foreach (var bot in _bots)
                bot.MainBuildingChanged -= RemoveBot;
    }

    private void Update()
    {
        GatherResources();
    }

    public void SetFlagForConstructionNewBase(Vector3 placePosition)
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
        newBot.MainBuildingChanged += RemoveBot;

        _resourcesCount -= _botPrice;
        ResourcesCountChanged?.Invoke(_resourcesCount);
    }

    public bool TrySendBotForConstruction()
    {
        if (TryGetIdleBot(out Bot idleBot))
        {
            _mainBuildingFlag.StartConstruction();
            SendBotForConstruction(idleBot);
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

    private void RemoveBot(Bot bot)
    {
        _bots.Remove(bot);
        bot.MainBuildingChanged -= RemoveBot;
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

    private void SendBotForConstruction(Bot bot)
    {
        bot.SendForConstructionMainBuilding(_mainBuildingFlag);
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
