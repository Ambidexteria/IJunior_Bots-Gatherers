using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : GenericSpawner<Resource>
{
    [SerializeField] private Transform _parentObject;
    [SerializeField] private string _objectName;
    [SerializeField] private SpawnZone3D _spawnZone;
    [SerializeField] private float _spawnCooldown = 5f;
    [SerializeField] private int _maxResourcesOnMap = 20;

    private WaitForSeconds _waitCooldown;
    private List<Resource> _resourcesOnMap;

    private int _resourceNumber = 0;

    public override void PrepareOnAwake()
    {
        _waitCooldown = new WaitForSeconds(_spawnCooldown);
        _resourcesOnMap = new List<Resource>();

        StartCoroutine(LaunchSpawnCoroutine());
    }

    public override void Despawn(Resource resource)
    {
        resource.ResetRotationAndScale();
        resource.Collected -= Despawn;

        _resourcesOnMap.Remove(resource);
        ReturnToPool(resource);
    }

    public override Resource Spawn()
    {
        Resource resource = GetNextObject();

        resource.transform.position = _spawnZone.GetRandomSpawnPositionOnPlaneWithVerticalOffset();
        resource.Collected += Despawn;
        resource.Drop();
        resource.gameObject.SetActive(true);
        resource.transform.SetParent(_parentObject, true);
        resource.gameObject.name = _objectName + "_" + _resourceNumber;

        _resourcesOnMap.Add(resource);
        _resourceNumber++;

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
