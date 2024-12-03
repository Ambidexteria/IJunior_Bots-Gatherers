using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanerDatabase : MonoBehaviour
{
    [SerializeField] private List<Base> _bases;

    private List<Resource> _resourcesMarkedForGathering = new();
    private Dictionary<ResourceScaner, List<Resource>> _resourcesUnmarked;

    private void Awake()
    {
        InitializeDictionaries();
    }

    private void OnEnable()
    {
        foreach (var baseBuilding in _bases)
        {
            baseBuilding.ResourceScaner.ResourcesFound += UpdateResources;
        }
    }

    private void OnDisable()
    {
        foreach (var baseBuilding in _bases)
        {
            baseBuilding.ResourceScaner.ResourcesFound -= UpdateResources;
        }
    }

    public bool TryGetResourceForGathering(out Resource resource, ResourceScaner scaner)
    {
        resource = null;

        if (_resourcesUnmarked.ContainsKey(scaner))
        {
            List<Resource> resources = _resourcesUnmarked[scaner];

            if (resources.Count > 0)
            {
                resource = GetNearestResource(resources, scaner);
                resource.CollisionHandler.Collected += DeleteCollectedResource;

                _resourcesMarkedForGathering.Add(resource);
                resources.Remove(resource);
                _resourcesUnmarked[scaner] = resources;

                return true;
            }
        }

        return false;
    }

    private Resource GetNearestResource(List<Resource> resources, ResourceScaner scaner)
    {
        if (resources.Count == 0)
            throw new System.ArgumentOutOfRangeException();

        return resources.Where(x => x.gameObject.activeSelf).OrderBy(x => GetDistanceToResource(x, scaner)).ToList().First();
    }

    private float GetDistanceToResource(Resource resource, ResourceScaner scaner)
    {
        return (resource.transform.position - scaner.transform.position).sqrMagnitude;
    }

    private void InitializeDictionaries()
    {
        _resourcesMarkedForGathering = new();
        _resourcesUnmarked = new();

        foreach (var baseBulding in _bases)
        {
            _resourcesUnmarked.Add(baseBulding.ResourceScaner, new List<Resource>());
        }
    }

    private void UpdateResources(List<Resource> resources, ResourceScaner scaner)
    {
        if (_resourcesUnmarked.ContainsKey(scaner))
        {
            resources = resources.Except(_resourcesUnmarked[scaner]).Except(_resourcesMarkedForGathering).ToList();
            _resourcesUnmarked[scaner].AddRange(resources);
        }
    }

    private void DeleteCollectedResource(Resource resource)
    {
        _resourcesMarkedForGathering.Remove(resource);
        resource.CollisionHandler.Collected -= DeleteCollectedResource;
    }
}
