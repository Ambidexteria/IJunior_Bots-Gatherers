using System;
using System.Collections;

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
        Completed?.Invoke();
    }
}
