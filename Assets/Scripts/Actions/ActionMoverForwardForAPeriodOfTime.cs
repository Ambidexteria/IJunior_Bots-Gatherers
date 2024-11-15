using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoverForwardForAPeriodOfTime : IUnitAction
{
    private float _time;
    private MoverForward _mover;

    public event Action Completed;

    public ActionMoverForwardForAPeriodOfTime(MoverForward mover, float time)
    {
        _time = time;
        _mover = mover;
    }

    public IEnumerator Launch()
    {
        _mover.Launch(_time);
        _mover.Completed += InvokeCompletedEvent;

        yield return null;
    }

    private void InvokeCompletedEvent()
    {
        Debug.Log("Invoke Completed Event");
        Completed?.Invoke();
    }
}
