using System;
using System.Collections;
using UnityEngine;

public class MoverToTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _minPositionToTarget = 0.5f;
    [SerializeField] private Transform _target;
    [SerializeField] private GroundOrientator _groundOrientator;

    private Coroutine _coroutine;

    public event Action TargetReached;

    public void Launch()
    {
        _coroutine = StartCoroutine(Move());
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private IEnumerator Move()
    {
        Vector3 temp = transform.position;

        while (IsTargetReached() == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            _groundOrientator.Orientate();
            transform.LookAt(_target, Vector3.up);
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);

            yield return null;
        }

        StopCoroutine(_coroutine);
    }

    private bool IsTargetReached()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _target.position);

        if (distanceToTarget < _minPositionToTarget)
        {
            TargetReached?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }
}
