using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Bot[] _bots;
    [SerializeField] private int _resourcesCount;
    [SerializeField] private ResourceScaner _resourceScaner;
    [SerializeField] private ResourceCollector _resourceCollector;
    [SerializeField] private ResourceScanerDatabase _resourceScanerDatabase;

    public event Action<int> ResourcesCountChanged;

    public ResourceCollector ResourceCollector => _resourceCollector;
    public ResourceScaner ResourceScaner => _resourceScaner;

    private void OnEnable()
    {
        _resourceCollector.Collected += IncreaseResourceCount;
    }

    private void OnDisable()
    {
        _resourceCollector.Collected -= IncreaseResourceCount;
    }

    private void Update()
    {
        GatherResources();
    }

    private void GatherResources()
    {
        if (TryGetIdleBot(out Bot idleBot))
        {
            SendBotForGatheringResource(idleBot);
        }
    }

    private void IncreaseResourceCount(Resource resource)
    {
        _resourcesCount++;

        ResourcesCountChanged?.Invoke(_resourcesCount);
    }

    private void SendBotForGatheringResource(Bot bot)
    {
        if (_resourceScanerDatabase.TryGetResourceForGathering(out Resource resource, _resourceScaner))
        {
            bot.SendForGatheringResource(resource, _resourceCollector.transform);
        }
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
