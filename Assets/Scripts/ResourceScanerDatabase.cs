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

    public bool TryGetResourceForGathering(out Resource resource, ResourceScaner scaner)
    {
        resource = null;

        if (_resourcesUnmarked.ContainsKey(scaner))
        {
            List<Resource> resources = _resourcesUnmarked[scaner];

            if (resources.Count > 0)
            {
                Debug.Log(nameof(TryGetResourceForGathering));
                foreach (var temp in resources)
                    Debug.Log(temp.gameObject.name);

                resource = GetNearestResource(resources, scaner);

                _resourcesMarkedForGathering.Add(resource);
                resources.Remove(resource);
                _resourcesUnmarked[scaner] = resources;

                Debug.Log(nameof(GetNearestResource));
                Debug.Log(resource.gameObject.name);
                Debug.Log(resource.transform.position);
                Debug.Log($"resource.gameobject is enabled - {resource.gameObject.activeSelf}");

                DebugDictionaries();

                return true;
            }
        }

        return false;
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
    }

    private Resource GetNearestResource(List<Resource> resources, ResourceScaner scaner)
    {
        if (resources.Count == 0)
            throw new System.ArgumentOutOfRangeException();

        Resource nearestResource = null;

        resources = GetActiveResources(resources);

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

        return nearestResource;
    }

    private List<Resource> GetActiveResources(List<Resource> resources)
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
