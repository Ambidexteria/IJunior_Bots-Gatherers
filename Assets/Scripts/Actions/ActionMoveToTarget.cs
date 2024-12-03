using System;
using System.Collections;
using System.Threading;
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

        _mover.TargetReached += InvokeCompletedEvent;
        _mover.SetTarget(_target);
        _mover.Launch();

        if (Vector3.Distance(_mover.transform.position, _target.transform.position) < 2f)
            throw new Exception();

        yield return null;
    }

    private void InvokeCompletedEvent()
    {
        _mover.TargetReached -= InvokeCompletedEvent;
        _mover.Stop();
        Completed?.Invoke();
    }
}
