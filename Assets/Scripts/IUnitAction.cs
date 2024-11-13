using System.Collections;
using System;

public interface IUnitAction
{
    public event Action Completed;

    public IEnumerator Launch();
}
