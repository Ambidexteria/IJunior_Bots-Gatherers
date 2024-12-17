using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : SpawnableObject
{
    [SerializeField] private static int _creationPrice = 3;
    [SerializeField] private float _waitTakeResourceTime = 0.5f;
    [SerializeField] private float _timeWaitAfterCompletedChainOfActions = 0.1f;
    [SerializeField] private float _waitConstructionTime = 5f;
    [SerializeField] private MoverToTarget _moverToTarget;
    [SerializeField] private Transform _resourcePosition;
    [SerializeField] private ActionController _actionController;

    private BotState _state = BotState.Idle;
    private ChainOfActions _chain;
    private WaitForSeconds _waitAfterChainOfActions;

    public BotState State => _state;
    public static int CreationPrice => _creationPrice;

    private void Awake()
    {
        _waitAfterChainOfActions = new(_timeWaitAfterCompletedChainOfActions);
    }

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

    public void SendForConstructionMainBuilding(MainBuildingFlag flag, IBuilding building)
    {
        _chain = CreateConstructionChainOfActions(flag, building);
        _actionController.SetChainOfActions(_chain);
        _state = BotState.Working;
    }

    private void SetIdleState()
    {
        StartCoroutine(WaitBeforeSetIdleState());
    }

    private IEnumerator WaitBeforeSetIdleState()
    {
        yield return _waitAfterChainOfActions;

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
            new ActionWaitForAPeriodOfTime(_timeWaitAfterCompletedChainOfActions),
        };

        return new ChainOfActions(actions);
    }

    private ChainOfActions CreateConstructionChainOfActions(MainBuildingFlag flag, IBuilding building)
    {
        List<IUnitAction> actions = new List<IUnitAction>
        {
            new ActionMoveToTarget(_moverToTarget, flag.transform),
            new ActionPlaceBuilding(building, flag.transform.position),
            new ActionWaitConstructionEnd(building, flag, _waitConstructionTime),
            new ActionWaitForAPeriodOfTime(_timeWaitAfterCompletedChainOfActions),
        };

        return new ChainOfActions(actions);
    }
}
