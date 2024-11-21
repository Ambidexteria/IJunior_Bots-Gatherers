using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public event Action<Resource> Collected;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Resource resource))
        {
            Collected?.Invoke(resource);
        }
    }
}
