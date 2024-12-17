using System;
using System.Collections;
using UnityEngine;

public class ActionWaitConstructionEnd : IUnitAction
{
    private MainBuildingFlag _flag;
    private WaitForSeconds _wait;
    private IBuilding _building;

    public ActionWaitConstructionEnd(IBuilding building, MainBuildingFlag flag)
    {
        _building = building;
        _flag = flag;
        _wait = new WaitForSeconds(building.ConstructionTime);
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
