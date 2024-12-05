using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Resource : SpawnableObject
{
    private Rigidbody _rigidbody;
    private Collider _collider;

    private Quaternion _defaultRotation;
    private Vector3 _defaultScale;

    public event Action<Resource> Collected;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        _defaultRotation = transform.rotation;
        _defaultScale = transform.localScale;

        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourceCollector _))
            Collected?.Invoke(this);
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
