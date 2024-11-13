using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericStateMachine : MonoBehaviour
{
    [SerializeField] private MoverToTarget _moverToTarget;
    [SerializeField] private MoverForward _moverForward;
    [SerializeField] private Transform _target1;
    [SerializeField] private Transform _target2;
    [SerializeField] private float _waitingTime;
    [SerializeField] private bool _loop;

    private ChainOfActions _chainOfActions;
    private List<IUnitAction> _actions;
    private int _actionNumber = 0;
    private IUnitAction _currentAction;
    private Coroutine _launchedActionCoroutine;


    public event Action ActionCompleted;

    private void Awake()
    {
        _actions = new List<IUnitAction>();
        _actions.Add(new ActionMoveToTarget(_moverToTarget, _target1));
        _actions.Add(new ActionWaitForAPeriodOfTime(_waitingTime));
        _actions.Add(new ActionMoveToTarget(_moverToTarget, _target2));
        _actions.Add(new ActionMoverForwardForAPeriodOfTime(_moverForward, _waitingTime));

        _chainOfActions = new ChainOfActions(_actions);

        _actions = _chainOfActions.GetActions() as List<IUnitAction>;

        Work();
    }

    public void SetChainOfActions(ChainOfActions chainOfActions)
    {
        _chainOfActions = chainOfActions;
    }

    public void Work()
    {
        Debug.Log("Launch chain of actions");

        SetNextAction();
    }

    private void SetNextAction()
    {
        Debug.Log("SetNextAction");
        StopCurrentAction();

        if (_actionNumber == _actions.Count)
        {
            EndCurrentChainOfActions();
            return;
        }

        _currentAction = _actions[_actionNumber];
        _launchedActionCoroutine = StartCoroutine(_currentAction.Launch());
        _currentAction.Completed += SetNextAction;

        _actionNumber++;
    }

    public void StopCurrentAction()
    {
        if (_currentAction != null)
        {
            _currentAction.Completed -= SetNextAction;
            StopCoroutine(_launchedActionCoroutine);
            _launchedActionCoroutine = null;
        }
    }

    private void EndCurrentChainOfActions()
    {
        Debug.Log("End Current Chain of Actions");
        _actionNumber = 0;
        _currentAction = null;
        _launchedActionCoroutine = null;

        if(_loop)
        {
            Work();
        }
    }
}
