using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : GenericSpawner<Bot>
{
    public override void Despawn(Bot bot)
    {
        ReturnToPool(bot);
    }

    public override Bot Spawn()
    {
        Bot bot = GetNextObject();
        bot.gameObject.SetActive(true);
        return bot;
    }
}
