using System;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private MoverToTarget _moverToTarget;
    [SerializeField] private Transform _resourcePosition;
    [SerializeField] private ActionController _actionController;

    private BotState _state = BotState.Idle;
    private ChainOfActions _chain;

    public BotState State => _state;

    private void OnEnable()
    {
        _actionController.ActionCompleted += SetIdleState;
    }

    private void OnDisable()
    {
        _actionController.ActionCompleted -= SetIdleState;
    }

    public void SendForGatheringResource(Resource resource, Transform basePositon)
    {
        _chain = CreateGatheringResourcesChainOfActions(resource, basePositon);
        _actionController.SetChainOfActions(_chain);
        _state = BotState.Gathering;
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
            new ActionTakeResource(resource, _resourcePosition),
            new ActionMoveToTarget(_moverToTarget, basePosition),
            new ActionUnloadResource(resource)
        };

        return new ChainOfActions(actions);
    }
}
