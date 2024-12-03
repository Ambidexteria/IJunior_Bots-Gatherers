using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class MoverToTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _minPositionToTarget = 0.5f;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _forwardPoint;

    private Coroutine _coroutine;

    public event Action TargetReached;

    public void Launch()
    {
        _coroutine = StartCoroutine(Move());
    }

    public void Stop()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private IEnumerator Move()
    {
        Vector3 direction;

        while (IsTargetReached() == false)
        {
            direction = _forwardPoint.position - transform.position;

            transform.DOLookAt(_target.position, 0f, AxisConstraint.Y);
            transform.Translate(direction * _speed * Time.deltaTime, Space.World);

            yield return null;
        }
    }

    private bool IsTargetReached()
    {
        float distanceToTarget = (transform.position - _target.position).sqrMagnitude;

        if (distanceToTarget < _minPositionToTarget)
        {
            TargetReached?.Invoke();
            _target = null;
            return true;
        }
        else
        {
            return false;
        }
    }
}
