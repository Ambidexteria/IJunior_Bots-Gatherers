using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScaner : MonoBehaviour
{
    private const string _resourceLayerName = "Resources";

    [SerializeField] private float _range;
    [SerializeField] private float _cooldown;

    private WaitForSeconds _waitCooldown;
    private Coroutine _scanCoroutine;
    private int _resourceLayerMask;

    public event Action<List<Resource>, ResourceScaner> ResourcesFound;

    private void Awake()
    {
        _waitCooldown = new WaitForSeconds(_cooldown);
        _resourceLayerMask = LayerMask.GetMask(_resourceLayerName);
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

        Collider[] hits = Physics.OverlapSphere(transform.position, _range, _resourceLayerMask);

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Resource resource) && hit.gameObject.activeInHierarchy)
            {
                resources.Add(resource);
            }
        }

        if (resources.Count > 0)
            ResourcesFound?.Invoke(resources, this);
    }
}
