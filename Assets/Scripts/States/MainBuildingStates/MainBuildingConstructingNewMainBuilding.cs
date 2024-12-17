using System;

public class MainBuildingConstructingNewMainBuilding : IBuildingState
{
    private MainBuilding _building;
    public MainBuildingConstructingNewMainBuilding(MainBuilding mainBuilding)
    {
        _building = mainBuilding;
    }

    public event Action ConstructionCompleted;

    public void OnStart() { }

    public void OnStop() { }

    public void OnUpdate()
    {
        if (_building.IsResourcesEnoughForConstructionNewMainBuilding)
        {
            if (_building.TrySendBotForConstruction())
            {
                ConstructionCompleted?.Invoke();
            }
        }
        else
        {
            _building.GatherResources();
        }
    }
}
