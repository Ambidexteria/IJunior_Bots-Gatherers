using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Bot[] _bots;
    [SerializeField] private List<Resource> _resourcesOnMap;
    [SerializeField] private int _resources;
    [SerializeField] private ResourceScaner _resourceScaner;
    [SerializeField] private ResourceCollector _resourceCollector;

    private void Update()
    {
        if (_resourcesOnMap.Count > 0)
        {
            if (TryGetResourceOnGround(out Resource resource))
            {
                SendBotForGatheringResource(resource);
            }
        }
    }

    private void OnEnable()
    {
        _resourceScaner.ResourcesFound += UpdateResourcesOnMap;
        _resourceCollector.Collected += IncreaseResourceCount;
    }

    private void OnDisable()
    {
        _resourceScaner.ResourcesFound -= UpdateResourcesOnMap;
        _resourceCollector.Collected -= IncreaseResourceCount;
    }

    private void UpdateResourcesOnMap(List<Resource> resources)
    {
        _resourcesOnMap = resources;
    }

    private void IncreaseResourceCount(Resource resource)
    {
        _resourcesOnMap.Remove(resource);
        _resources++;
    }

    private void SendBotForGatheringResource(Resource resource)
    {
        if (TryGetIdleBot(out Bot bot))
        {
            resource.MarkForGathering();
            bot.SendForGatheringResource(resource, transform);
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

    private bool TryGetResourceOnGround(out Resource resource)
    {
        resource = null;

        foreach(var tempResource in _resourcesOnMap)
        {
            if(tempResource.State == ResourceState.Grounded)
            {
                resource = tempResource;
                return true;
            }
        }

        return false;
    }

}
