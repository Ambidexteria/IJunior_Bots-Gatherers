using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScaner : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] private float _range;
    [SerializeField] private float _cooldown;

    private WaitForSeconds _waitCooldown;
    private Coroutine _scanCoroutine;

    public event Action<List<Resource>> ResourcesFound;

    private void Awake()
    {
        _waitCooldown = new WaitForSeconds(_cooldown);
    }

    private void OnEnable()
    {
        _scanCoroutine = StartCoroutine(LaunchScanCoroutine());
    }

    private IEnumerator LaunchScanCoroutine()
    {
        while (enabled)
        {
            yield return _waitCooldown;
            Scan();
        }
    }

    private void Scan()
    {
        List<Resource> resources = new();

        Collider[] hits = Physics.OverlapSphere(transform.position, _range, _layerMask);

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Resource resource) && hit.gameObject.activeInHierarchy)
            {
                resources.Add(resource);
            }
        }

        if (resources.Count > 0)
            ResourcesFound?.Invoke(resources);
    }
}
