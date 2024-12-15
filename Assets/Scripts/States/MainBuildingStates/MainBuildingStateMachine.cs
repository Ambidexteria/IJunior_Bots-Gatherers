using System.Collections;
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

        _buildingStates = new Dictionary<MainBuildingState, IBuildingState> ();
        _buildingStates.Add(MainBuildingState.Idle, new MainBuildingIdleState(_building));

        MainBuildingConstructingNewBase constructingNewBase = new MainBuildingConstructingNewBase(_building);
        constructingNewBase.ConstructionCompleted += SetIdleState;
        _buildingStates.Add(MainBuildingState.ConstructingNewBase, constructingNewBase);
        
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
        ChangeState(_buildingStates[MainBuildingState.ConstructingNewBase]);
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
