using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWaitConstructionEnd : IUnitAction
{
    private MainBuildingFlag _flag;
    private WaitForSeconds _wait;
    private IBuilding _building;

    public ActionWaitConstructionEnd(IBuilding building, MainBuildingFlag flag, float waitTime)
    {
        _building = building;
        _flag = flag;
        _wait = new WaitForSeconds(waitTime);
    }

    public event Action Completed;

    public IEnumerator Launch()
    {
        yield return _wait;

        _flag.EndConstruction();
        _flag.Hide();
        _building.Enable();

        Completed?.Invoke();
    }
}
