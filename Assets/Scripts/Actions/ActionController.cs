using System;
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

    private void StartChainOfActions()
    {
        SetNextAction();
    }

    private void StopCurrentAction()
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
        _actionNumber = 0;
        _currentAction = null;
        _actions = null;
        _launchedActionCoroutine = null;

        ActionCompleted?.Invoke();
        Debug.Log("Actions completed");

        if (_loop)
        {
            StartChainOfActions();
        }
    }
}
