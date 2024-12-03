using System;
using UnityEngine;

[RequireComponent(typeof(Resource))]
public class ResourceCollisionHandler : MonoBehaviour
{
    private Resource _resource;

    public event Action<Resource> Collected;

    private void Awake()
    {
        _resource = GetComponent<Resource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ResourceCollector _))
            Collected?.Invoke(_resource);
    }
}
