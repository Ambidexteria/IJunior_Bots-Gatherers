using System;
using System.Collections;
using UnityEngine;

public class ActionPlaceBuilding : IUnitAction
{
    private IBuilding _building;
    private Vector3 _position;

    public ActionPlaceBuilding(IBuilding building, Vector3 position)
    {
        _building = building;
        _position = position;
    }

    public event Action Completed;

    public IEnumerator Launch()
    {
        _building.Place(_position);
        yield return null;
        Completed?.Invoke();
    }
}
