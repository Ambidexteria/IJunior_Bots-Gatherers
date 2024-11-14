using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveToTarget : IUnitAction
{
    private MoverToTarget _mover;
    private Transform _target;

    public event Action Completed;

    public ActionMoveToTarget (MoverToTarget moverToTarget, Transform target)
    {
        _mover = moverToTarget;
        _target = target;
    }

    public IEnumerator Launch()
    {
        Debug.Log($"Move to {_target}");

        _mover.TargetReached += InvokeCompletedEvent;
        _mover.SetTarget(_target);
        _mover.Launch();

        yield return null;
    }

    private void InvokeCompletedEvent()
    {
        _mover.TargetReached += InvokeCompletedEvent;
        Completed?.Invoke();
    }
}
