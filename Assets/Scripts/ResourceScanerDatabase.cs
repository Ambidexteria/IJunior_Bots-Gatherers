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
            baseBuilding.ResourceCollector.Collected += DeleteCollectedResource;
        }
    }

    private void OnDisable()
    {
        foreach (var baseBuilding in _bases)
        {
            baseBuilding.ResourceScaner.ResourcesFound -= UpdateResources;
            baseBuilding.ResourceCollector.Collected -= DeleteCollectedResource;
        }
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
            resources = resources.Except(_resourcesUnmarked[scaner]).ToList();
            _resourcesUnmarked[scaner].AddRange(resources);

            //DebugDictionaries();
        }
    }

    private void DeleteCollectedResource(Resource resource)
    {
        _resourcesMarkedForGathering.Remove(resource);
    }

    public bool TryGetResourceForGathering(out Resource resource, ResourceScaner scaner)
    {
        resource = null;

        if (_resourcesUnmarked.ContainsKey(scaner))
        {
            if (TryGetNearestUnmarkedResource(out resource, _resourcesUnmarked[scaner], scaner))
            {
                _resourcesUnmarked[scaner].Remove(resource);
                _resourcesMarkedForGathering.Add(resource);

                Debug.Log(nameof(TryGetNearestUnmarkedResource));
                Debug.Log(resource.gameObject.name);
                Debug.Log(resource.transform.position);
                Debug.Log($"resource.gameobject is enabled - {resource.gameObject.activeSelf}");

                return true;
            }
        }

        return false;
    }

    private bool TryGetNearestUnmarkedResource(out Resource nearestResource, List<Resource> resources, ResourceScaner scaner)
    {
        DebugDictionaries();

        nearestResource = null;

        resources = GetActivedResources(resources);

        if (resources.Count == 0)
            return false;

        float minDistance = float.MaxValue;
        float currentDistance;

        foreach (var tempResource in resources)
        {
            currentDistance = GetDistanceToResource(tempResource, scaner);

            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                nearestResource = tempResource;
            }
        }

        return true;
    }

    private List<Resource> GetActivedResources(List<Resource> resources)
    {
        return resources.Where(x => x.gameObject.activeSelf).ToList();
    }

    private float GetDistanceToResource(Resource resource, ResourceScaner scaner)
    {
        return (resource.transform.position - scaner.transform.position).sqrMagnitude;
    }

    private void DebugDictionaries()
    {
        Debug.Log(nameof(_resourcesMarkedForGathering));

        foreach (var resource in _resourcesMarkedForGathering)
        {
            Debug.Log(resource.ToString());
        }

        Debug.Log(nameof(_resourcesUnmarked));

        foreach (var resourceList in _resourcesUnmarked.Values)
        {
            foreach (var resource in resourceList)
            {
                Debug.Log(resource.ToString());
            }
        }
    }
}
