using System;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private float _waitTakeResourceTime = 0.5f;
    [SerializeField] private float _waitAfterCompletedChainOfActions = 0.1f;
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

    public void SendForGatheringResource(Resource resource, Transform resourceDropPosition)
    {
        _chain = CreateGatheringResourcesChainOfActions(resource, resourceDropPosition);
        _actionController.SetChainOfActions(_chain);
        _state = BotState.Gathering;
    }

    private void SetIdleState()
    {
        _state = BotState.Idle;
    }

    private ChainOfActions CreateGatheringResourcesChainOfActions(Resource resource, Transform basePosition)
    {
        Debug.Log(nameof(CreateGatheringResourcesChainOfActions));  
        Debug.Log(resource.gameObject.name);  
        Debug.Log(resource.transform.position);  

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
}
