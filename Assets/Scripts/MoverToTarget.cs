using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class MoverToTarget : MonoBehaviour
{
    private const float MinDistanceToTarget = 4f;

    [SerializeField] private float _speed;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _forwardPoint;
    [SerializeField] private NavMeshAgent _agent;

    private Coroutine _coroutine;

    public event Action TargetReached;

    public void Launch(float minDistanceToTarget = MinDistanceToTarget)
    {
        _coroutine = StartCoroutine(Move(minDistanceToTarget));
    }

    public void Stop()
    {
        if (_coroutine != null)
        {
            _agent.isStopped = true;
            StopCoroutine(_coroutine);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private IEnumerator Move(float minDistanceToTarget)
    {
        _agent.SetDestination(_target.position);
        _agent.isStopped = false;

        yield return new WaitUntil(() => IsTargetReached(minDistanceToTarget));
    }

    private bool IsTargetReached(float minDistanceToTarget)
    {
        float distanceToTarget = (transform.position - _target.position).sqrMagnitude;

        if (distanceToTarget < minDistanceToTarget * minDistanceToTarget)
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
