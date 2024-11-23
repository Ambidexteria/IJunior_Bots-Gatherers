using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Resource : SpawnableObject
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    private ResourceState _state;

    private Quaternion _defaultRotation;
    private Vector3 _defaultScale;

    public ResourceState State => _state;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        _defaultRotation = transform.rotation;
        _defaultScale = transform.localScale;

        _collider.enabled = true;
        
        _state = ResourceState.Grounded;
    }

    public void Drop()
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _state = ResourceState.Grounded;
    }

    public void Take()
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        _state = ResourceState.Taken;
    }

    public void MarkForGathering()
    {
        _state = ResourceState.MarkedForGathering;
    }

    public void ResetRotationAndScale()
    {
        transform.rotation = _defaultRotation;
        transform.localScale = _defaultScale;
    }
}
