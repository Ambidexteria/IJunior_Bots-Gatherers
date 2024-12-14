using System;
using System.Collections.Generic;
using UnityEngine;

public class Bot : SpawnableObject
{
    [SerializeField] private float _waitTakeResourceTime = 0.5f;
    [SerializeField] private float _waitAfterCompletedChainOfActions = 0.1f;
    [SerializeField] private float _waitConstructionTime = 5f;
    [SerializeField] private MoverToTarget _moverToTarget;
    [SerializeField] private Transform _resourcePosition;
    [SerializeField] private ActionController _actionController;
    [SerializeField] private MainBuilding _mainBuildingPrefab;

    private BotState _state = BotState.Idle;
    private ChainOfActions _chain;

    public event Action<Bot> MainBuildingChanged;

    public BotState State => _state;

    private void OnEnable()
    {
        _actionController.ActionCompleted += SetIdleState;
    }

    private void OnDisable()
    {
        _actionController.ActionCompleted -= SetIdleState;
    }

    public void SendForGatheringResource(Resource resource, Transform resourceDropPosition)
    {
        _chain = CreateGatheringResourcesChainOfActions(resource, resourceDropPosition);
        _actionController.SetChainOfActions(_chain);
        _state = BotState.Gathering;
    }

    public void SendForConstructionMainBuilding(MainBuildingFlag flag)
    {
        MainBuilding building = Instantiate(_mainBuildingPrefab);
        building.AddBot(this);
        building.gameObject.SetActive(false);

        Debug.Log("MainBuildingChanged invoked");
        Debug.Log(MainBuildingChanged.Method);
        MainBuildingChanged?.Invoke(this);

        _chain = CreateConstructionChainOfActions(flag, building);
        _actionController.SetChainOfActions(_chain);
        _state = BotState.Working;
    }

    private void SetIdleState()
    {
        _state = BotState.Idle;
    }

    private ChainOfActions CreateGatheringResourcesChainOfActions(Resource resource, Transform basePosition)
    {
        List<IUnitAction> actions = new List<IUnitAction>
        {
            new ActionMoveToTarget(_moverToTarget, resource.transform),
            new ActionWaitForAPeriodOfTime(_waitTakeResourceTime),
            new ActionTakeResource(resource, _resourcePosition),
            new ActionMoveToTarget(_moverToTarget, basePosition),
            new ActionUnloadResource(resource),
            new ActionWaitForAPeriodOfTime(_waitAfterCompletedChainOfActions),
        };

        return new ChainOfActions(actions);
    }

    private ChainOfActions CreateConstructionChainOfActions(MainBuildingFlag flag, IBuilding building)
    {
        List<IUnitAction> actions = new List<IUnitAction>
        {
            new ActionMoveToTarget(_moverToTarget, flag.transform),
            new ActionPlaceBuilding(building, flag.transform.position),
            new ActionWaitConstructionEnd(flag, _waitConstructionTime),
            new ActionWaitForAPeriodOfTime(_waitAfterCompletedChainOfActions),
        };

        return new ChainOfActions(actions);
    }
}
