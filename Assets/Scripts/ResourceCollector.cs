using System;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public event Action Collected;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Resource _))
        {
            Collected?.Invoke();
        }
    }
}
