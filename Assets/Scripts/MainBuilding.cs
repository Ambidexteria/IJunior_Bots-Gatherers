using System;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots;
    [SerializeField] private int _maxBots;
    [SerializeField] private int _resourcesCount;
    [SerializeField] private ResourceCollector _resourceCollector;
    [SerializeField] private ResourceScanerDatabase _resourceScanerDatabase;
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private Transform _botSpawnPosition;
    [SerializeField] private int _botPrice = 3;
    [SerializeField] private int _mainBuildingPrice = 3;
    [SerializeField] private MainBuildingFlag _mainBuildingFlag;

    public event Action<int> ResourcesCountChanged;
    public event Action<Vector3> ConstructionFlagSet;

    public bool IsResourcesEnoughForConstructionNewMainBuilding => _resourcesCount >= _mainBuildingPrice;
    public bool IsBotCanBeCreated => _resourcesCount >= _botPrice && _bots.Count < _maxBots;

    private void Update()
    {
        GatherResources();
    }

    public void SetFlagForConstructionNewBase(Vector3 placePosition)
    {
        ConstructionFlagSet?.Invoke(placePosition);
        _mainBuildingFlag.Place(placePosition);
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
        if (TryGetIdleBot(out Bot idleBot))
        {
            SendBotForConstruction(idleBot);
            return true;
        }
        else
        {
            return false;
        }
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
        bot.SendForConstruction(_mainBuildingFlag);
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
