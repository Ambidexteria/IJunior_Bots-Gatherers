using System;
using System.Collections;
using UnityEngine;

public class ActionMoveToTarget : IUnitAction
{
    private MoverToTarget _mover;
    private Transform _target;

    public ActionMoveToTarget (MoverToTarget moverToTarget, Transform target)
    {
        _mover = moverToTarget;
        _target = target;
    }

    public event Action Completed;

    public IEnumerator Launch()
    {

        _mover.TargetReached += InvokeCompletedEvent;
        _mover.SetTarget(_target);
        _mover.Launch();

        yield return null;
    }

    private void InvokeCompletedEvent()
    {
        _mover.TargetReached -= InvokeCompletedEvent;
        _mover.Stop();
        Completed?.Invoke();
    }
}
