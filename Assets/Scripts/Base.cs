using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    [SerializeField] private Bot[] _bots;
    [SerializeField] private List<Resource> _resourcesOnMap;
    [SerializeField] private int _resources;
    [SerializeField] private ResourceScaner _resourceScaner;

    private void Awake()
    {
        SendBotForGatheringResource(_resourcesOnMap[0]);
    }

    private void OnEnable()
    {
        _resourceScaner.ResourcesFound += UpdateResourcesOnMap;
    }

    private void OnDisable()
    {
        _resourceScaner.ResourcesFound -= UpdateResourcesOnMap;
    }

    private void UpdateResourcesOnMap(List<Resource> resources)
    {
        _resourcesOnMap = resources;
    }

    private void SendBotForGatheringResource(Resource resource)
    {
        if (TryGetIdleBot(out Bot bot))
        {
            bot.SendForGatheringResource(resource, transform);
        }
    }

    private bool TryGetIdleBot(out Bot bot)
    {
        bot = null;

        foreach(var tempBot  in _bots) 
        {
            if(tempBot.State == BotState.Idle)
            {
                bot = tempBot;
                return true;
            }
        }

        return false;
    }
}
