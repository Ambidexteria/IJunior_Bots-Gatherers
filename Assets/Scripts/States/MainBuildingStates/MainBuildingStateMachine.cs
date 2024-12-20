using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MainBuilding))]
public class MainBuildingStateMachine : MonoBehaviour
{
    private MainBuilding _building;
    private IBuildingState _currentState;
    private Dictionary<MainBuildingState, IBuildingState> _buildingStates;

    private void Awake()
    {
        _building = GetComponent<MainBuilding>();

        MainBuildingConstructingNewMainBuilding constructingNewMainBuilding = new(_building);
        constructingNewMainBuilding.ConstructionCompleted += SetIdleState;

        _buildingStates = new Dictionary<MainBuildingState, IBuildingState>
        {
            { MainBuildingState.Idle, new MainBuildingIdleState(_building) },
            { MainBuildingState.ConstructingNewMainBuilding, constructingNewMainBuilding }
        };
        
        SetIdleState();
    }

    private void OnEnable()
    {
        _building.ConstructionFlagSet += SetConstructingNewBaseState;
    }

    private void OnDisable()
    {
        _building.ConstructionFlagSet -= SetConstructingNewBaseState;
    }

    private void Update()
    {
        _currentState.OnUpdate();
    }

    private void SetIdleState()
    {
        ChangeState(_buildingStates[MainBuildingState.Idle]);
    }

    private void SetConstructingNewBaseState(Vector3 position)
    {
        ChangeState(_buildingStates[MainBuildingState.ConstructingNewMainBuilding]);
    }

    private void ChangeState(IBuildingState state)
    {
        if(_currentState != null)
        {
            _currentState.OnStop();
        }

        _currentState = state;
        _currentState.OnStart();
    }
}
