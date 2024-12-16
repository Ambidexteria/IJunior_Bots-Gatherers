using UnityEngine;

public class MainBuildingSpawner : GenericSpawner<MainBuilding>
{
    [SerializeField] private ResourceScanerDatabase _scanerDatabase;
    [SerializeField] private BotSpawner _botSpawner;

    public override void Despawn(MainBuilding mainBuilding)
    {
        ReturnToPool(mainBuilding);
    }

    public override MainBuilding Spawn()
    {
        return GetNextObject();
    }
}
