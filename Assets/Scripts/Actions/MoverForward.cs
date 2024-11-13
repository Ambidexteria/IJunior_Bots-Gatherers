using System.Collections;
using System;
using UnityEngine;

public class MoverForward : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _endTime;

    public event Action Completed;

    public void Launch(float time)
    {
        StartCoroutine(MoveCoroutine(time));
    }

    private IEnumerator MoveCoroutine(float time)
    {
        _endTime = time;
        float currentTime = 0;

        while (currentTime < _endTime)
        {
            transform.Translate(_speed * Time.deltaTime * transform.forward);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Completed?.Invoke();
    }
}
