using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(ResourceCollisionHandler))]
public class Resource : SpawnableObject
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    private ResourceCollisionHandler _collisionHandler;

    private Quaternion _defaultRotation;
    private Vector3 _defaultScale;

    public ResourceCollisionHandler CollisionHandler => _collisionHandler;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _collisionHandler = GetComponent<ResourceCollisionHandler>();

        _defaultRotation = transform.rotation;
        _defaultScale = transform.localScale;

        _collider.enabled = true;
    }

    public void Drop()
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }

    public void Take()
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
    }

    public void ResetRotationAndScale()
    {
        transform.rotation = _defaultRotation;
        transform.localScale = _defaultScale;
    }
}
