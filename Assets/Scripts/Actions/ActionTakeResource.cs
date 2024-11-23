using System;
using System.Collections;
using UnityEngine;

public class ActionTakeResource : IUnitAction
{
    private Resource _resource;
    private Transform _transportingPosition;

    public ActionTakeResource(Resource resourceTransform, Transform transportingPosition)
    {
        _resource = resourceTransform;
        _transportingPosition = transportingPosition;
    }

    public event Action Completed;

    public IEnumerator Launch()
    {
        _resource.transform.SetParent(_transportingPosition);
        _resource.Take();
        _resource.transform.SetLocalPositionAndRotation(_transportingPosition.localPosition, _transportingPosition.localRotation);

        yield return null;

        Completed?.Invoke();
    }
}
