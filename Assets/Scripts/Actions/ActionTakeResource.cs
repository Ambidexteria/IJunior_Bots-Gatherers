using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTakeResource : IUnitAction
{
    private Resource _resource;
    private Transform _parent;

    public ActionTakeResource(Resource resourceTransform, Transform parent)
    {
        _resource = resourceTransform;
        _parent = parent;
    }

    public event Action Completed;

    public IEnumerator Launch()
    {
        Debug.Log("Take resource");
        _resource.transform.SetParent(_parent);
        _resource.Take();
        _resource.transform.SetLocalPositionAndRotation(_parent.localPosition, _parent.localRotation);

        yield return null;

        Completed?.Invoke();
    }
}
