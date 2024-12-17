using System;
using System.Collections.Generic;
using UnityEngine;

public class MainBuildingBotsDatabase : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots;
    [SerializeField] private int _maxBots = 5;
    [SerializeField] private int _minBots = 1;

    public bool CanAddNewBot => _bots.Count < _maxBots;
    public bool BotsCountBiggerThanMin => _bots.Count > _minBots;

    public void AddNewBot(Bot bot)
    {
        if (_bots.Contains(bot))
            throw new ArgumentException();

        if (_bots.Count < _maxBots)
            _bots.Add(bot);
    }

    public void RemoveBot(Bot bot)
    {
        if(_bots.Contains(bot))
            _bots.Remove(bot);
    }

    public bool TryGetIdleBot(out Bot bot)
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
