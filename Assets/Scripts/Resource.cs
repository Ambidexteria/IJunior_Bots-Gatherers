using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Resource : SpawnableObject
{
    private Collider _collider;
    private ResourceState _state;

    public ResourceState State => _state;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        _collider.isTrigger = true;
        _collider.enabled = true;
        
        _state = ResourceState.Grounded;
    }

    public void Drop()
    {
        _collider.enabled = true;
        _state = ResourceState.Grounded;
    }

    public void Take()
    {
        _collider.enabled = false;
        _state = ResourceState.Taken;
    }

    public void MarkForGathering()
    {
        _state = ResourceState.MarkedForGathering;
    }
}
