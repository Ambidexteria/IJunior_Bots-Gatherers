using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : GenericSpawner<Resource>
{
    [SerializeField] private SpawnZone3D _spawnZone;
    [SerializeField] private ResourceCollector _collector;
    [SerializeField] private float _spawnCooldown = 5f;
    [SerializeField] private int _maxResourcesOnMap = 20;

    private WaitForSeconds _waitCooldown;
    private List<Resource> _resourcesOnMap;

    private void OnEnable()
    {
        _collector.Collected += Despawn;
    }

    private void OnDisable()
    {
        _collector.Collected -= Despawn;
    }

    public override void PrepareOnAwake()
    {
        _waitCooldown = new WaitForSeconds(_spawnCooldown);
        _resourcesOnMap = new List<Resource>();

        StartCoroutine(LaunchSpawnCoroutine());
    }

    public override void Despawn(Resource type)
    {
        _resourcesOnMap.Remove(type);
        type.ResetRotationAndScale();
        ReturnToPool(type);
    }

    public override Resource Spawn()
    {
        Resource resource = GetNextObject();
        resource.transform.position = _spawnZone.GetRandomSpawnPositionOnPlaneWithVerticalOffset();
        resource.Drop();
        _resourcesOnMap.Add(resource);

        return resource;
    }

    private IEnumerator LaunchSpawnCoroutine()
    {
        while (enabled)
        {
            yield return _waitCooldown;

            if (_resourcesOnMap.Count < _maxResourcesOnMap)
                Spawn();
        };
    }
}
