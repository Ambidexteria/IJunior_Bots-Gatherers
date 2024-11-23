using System;
using System.Collections;
using UnityEngine;

public class ActionWaitForAPeriodOfTime : IUnitAction
{
    private float _time;

    public event Action Completed;

    public ActionWaitForAPeriodOfTime(float time)
    {
        _time = time;
    }

    public IEnumerator Launch()
    {
        yield return new WaitForSeconds(_time);

        Completed?.Invoke();
    }
}
