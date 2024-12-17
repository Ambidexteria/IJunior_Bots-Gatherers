using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : GenericSpawner<Bot>
{
    public override void PrepareForSpawn(ref Bot bot)
    {
        bot.gameObject.SetActive(true);
    }
}
