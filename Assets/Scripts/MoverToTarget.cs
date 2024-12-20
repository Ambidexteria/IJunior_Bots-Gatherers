using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class MoverToTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _minPositionToTarget = 0.5f;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _forwardPoint;
    [SerializeField] private NavMeshAgent _agent;

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
        _agent.SetDestination(_target.position);

        yield return new WaitUntil(() => IsTargetReached());
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
