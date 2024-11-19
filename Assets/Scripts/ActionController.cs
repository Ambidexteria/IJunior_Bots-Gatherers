using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] private bool _loop;

    private ChainOfActions _chainOfActions;
    private List<IUnitAction> _actions;
    private int _actionNumber = 0;
    private IUnitAction _currentAction;
    private Coroutine _launchedActionCoroutine;

    public event Action ActionCompleted;

    public void SetChainOfActions(ChainOfActions chainOfActions)
    {
        _actions = chainOfActions.GetActions();

        StartChainOfActions();
    }

    public void StartChainOfActions()
    {
        SetNextAction();
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

    private void EndCurrentChainOfActions()
    {
        Debug.Log("End Current Chain of Actions");
        _actionNumber = 0;
        _currentAction = null;
        _launchedActionCoroutine = null;
        _actions = null;

        if(_loop)
        {
            StartChainOfActions();
        }
    }
}
