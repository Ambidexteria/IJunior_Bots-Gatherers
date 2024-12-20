using System;
using System.Collections;
using UnityEngine;

public class ActionMoveToTarget : IUnitAction
{
    private MoverToTarget _mover;
    private Transform _target;
    private float _minDistanceToTarget = float.MinValue;

    public ActionMoveToTarget(MoverToTarget moverToTarget, Transform target)
    {
        _mover = moverToTarget;
        _target = target;
    }

    public ActionMoveToTarget(MoverToTarget moverToTarget, Transform target, float minDistanceToTarget)
    {
        _mover = moverToTarget;
        _target = target;
        _minDistanceToTarget = minDistanceToTarget;
    }

    public event Action Completed;

    public IEnumerator Launch()
    {
        _mover.TargetReached += InvokeCompletedEvent;
        _mover.SetTarget(_target);

        if (_minDistanceToTarget > 0f)
            _mover.Launch(_minDistanceToTarget);
        else
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
