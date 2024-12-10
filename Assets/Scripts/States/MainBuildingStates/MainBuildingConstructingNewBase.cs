using System;

public class MainBuildingConstructingNewBase : IBuildingState
{
    private MainBuilding _building;
    public MainBuildingConstructingNewBase(MainBuilding mainBuilding) 
    { 
        _building = mainBuilding;
    }

    public event Action ConstructionCompleted;

    public void OnStart()
    {
    }

    public void OnStop()
    {
    }

    public void OnUpdate()
    {
        if(_building.IsResourcesEnoughForConstructionNewMainBuilding)
        {
            _building.SendBotForConstruction();
            ConstructionCompleted?.Invoke();
        }
    }
}
