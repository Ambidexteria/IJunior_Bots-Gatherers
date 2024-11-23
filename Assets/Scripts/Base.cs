using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Bot[] _bots;
    [SerializeField] private List<Resource> _resourcesOnMap;
    [SerializeField] private int _resourcesCount;
    [SerializeField] private ResourceScaner _resourceScaner;
    [SerializeField] private ResourceCollector _resourceCollector;

    public event Action<int> ResourcesCountChanged;

    private void Update()
    {
        if (_resourcesOnMap.Count > 0)
        {
            GatherResources();
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

    private void GatherResources()
    {
        if (TryGetIdleBot(out Bot idleBot))
        {
            if (TryGetNearestResourceOnGround(out Resource resource))
            {
                SendBotForGatheringResource(idleBot, resource);
            }
        }
    }

    private void UpdateResourcesOnMap(List<Resource> resources)
    {
        _resourcesOnMap = resources;
    }

    private void IncreaseResourceCount(Resource resource)
    {
        _resourcesOnMap.Remove(resource);
        _resourcesCount++;
        
        ResourcesCountChanged?.Invoke(_resourcesCount);
    }

    private void SendBotForGatheringResource(Bot bot, Resource resource)
    {
        resource.MarkForGathering();
        bot.SendForGatheringResource(resource, transform);
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

    private bool TryGetNearestResourceOnGround(out Resource nearestResource)
    {
        nearestResource = null;

        if(TryGetResourcesOnGround(out List<Resource> resources))
        {
            float minDistance = float.MaxValue;
            float currentDistance;

            foreach (var tempResource in resources)
            {
                currentDistance = GetDistanceToResource(tempResource);

                if(currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    nearestResource = tempResource;
                }
            }

            return true;
        }

        return false;
    }

    private bool TryGetResourcesOnGround(out List<Resource> resources)
    {
        resources = new();

        foreach(var tempResource in _resourcesOnMap)
            if (tempResource.State == ResourceState.Grounded)
                resources.Add(tempResource);
        
        return resources.Count > 0;
    }

    private float GetDistanceToResource(Resource resource)
    {
        return Vector3.Distance(resource.transform.position, transform.position);
    }
}
