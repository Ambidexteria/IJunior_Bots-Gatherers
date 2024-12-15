using System;
using UnityEngine;

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
        if (_building.IsResourcesEnoughForConstructionNewMainBuilding)
        {
            if (_building.TrySendBotForConstruction())
            {
                ConstructionCompleted?.Invoke();
            }
        }
    }
}
