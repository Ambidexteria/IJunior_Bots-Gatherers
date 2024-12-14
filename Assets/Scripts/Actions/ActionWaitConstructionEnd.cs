using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWaitConstructionEnd : IUnitAction
{
    private MainBuildingFlag _flag;
    private WaitForSeconds _wait;

    public ActionWaitConstructionEnd(MainBuildingFlag flag, float waitTime)
    {
        _flag = flag;
        _wait = new WaitForSeconds(waitTime);
    }

    public event Action Completed;

    public IEnumerator Launch()
    {
        yield return _wait;
        _flag.Hide();
        Completed?.Invoke();
    }
}
