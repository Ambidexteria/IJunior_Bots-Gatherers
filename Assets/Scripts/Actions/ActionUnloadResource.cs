using System;
using System.Collections;

public class ActionUnloadResource : IUnitAction
{
    private Resource _resource;

    public ActionUnloadResource(Resource resource)
    {
        _resource = resource;
    }

    public event Action Completed;

    public IEnumerator Launch()
    {
        _resource.transform.SetParent(null);
        _resource.Drop();
        yield return null;

        Completed?.Invoke();
    }
}
