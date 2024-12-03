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
        

        while (IsTargetReached() == false)
        {
            transform.DOLookAt(_target.position, 0f, AxisConstraint.Y);
            transform.Translate(_speed * Time.deltaTime * (_forwardPoint.position - transform.position), Space.World);

            yield return null;
        }

        Debug.Log("Moving completed");
    }

    private bool IsTargetReached()
    {
        float distanceToTarget = (transform.position - _target.position).sqrMagnitude;
        //Debug.Log($"distance to target - {distanceToTarget}, minTargetPosition - {_minPositionToTarget}");

        if (distanceToTarget < _minPositionToTarget)
        {
            TargetReached?.Invoke();
            Debug.Log(nameof(IsTargetReached));
            _target = null;
            return true;
        }
        else
        {
            return false;
        }
    }
}
