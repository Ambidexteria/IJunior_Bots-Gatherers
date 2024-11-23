using System;
using System.Collections;
using UnityEngine;

public class ActionTakeResource : IUnitAction
{
    private Resource _resource;
    private Transform _transportingPoint;

    public ActionTakeResource(Resource resourceTransform, Transform transportingPoint)
    {
        _resource = resourceTransform;
        _transportingPoint = transportingPoint;
    }

    public event Action Completed;

    public IEnumerator Launch()
    {
        _resource.transform.SetParent(_transportingPoint, true);
        _resource.Take();
        _resource.transform.position = _transportingPoint.position;
        _resource.ResetRotationAndScale();

        yield return null;

        Completed?.Invoke();
    }
}
