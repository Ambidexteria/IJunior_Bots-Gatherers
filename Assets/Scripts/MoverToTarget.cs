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

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private IEnumerator Move()
    {
        while (IsTargetReached() == false)
        {
            transform.DOLookAt(_target.position, 0f, AxisConstraint.Y);
            transform.Translate((_speed * Time.deltaTime) * (_forwardPoint.position - transform.position), Space.World);

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
