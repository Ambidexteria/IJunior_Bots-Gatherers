using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUnloadResource : IUnitAction
{
    private Resource _resource;

    public event Action Completed;

    public ActionUnloadResource(Resource resource)
    {
        _resource = resource;
    }

    public IEnumerator Launch()
    {
        _resource.transform.SetParent(null);
        _resource.Drop();
        yield return null;

        Completed?.Invoke();
    }
}
