using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanerDatabase : MonoBehaviour
{
    [SerializeField] private ResourceScaner _scaner;

    private List<Resource> _resourcesMarkedForGathering = new();
    private List<Resource> _resourcesUnmarked = new();

    private void OnEnable()
    {
        _scaner.ResourcesFound += UpdateResources;
    }

    private void OnDisable()
    {
        _scaner.ResourcesFound -= UpdateResources;
    }

    public bool TryGetNearestResourceForGathering(out Resource resource, Vector3 currentPosition)
    {
        resource = null;

        if (_resourcesUnmarked.Count > 0)
        {
            resource = GetResourceNearestToPosition(currentPosition);
            resource.Collected += DeleteCollectedResource;

            _resourcesMarkedForGathering.Add(resource);
            _resourcesUnmarked.Remove(resource);

            return true;
        }

        return false;
    }

    private Resource GetResourceNearestToPosition(Vector3 currentPosition)
    {
        return _resourcesUnmarked.Where(x => x.gameObject.activeSelf).OrderBy(x => GetDistanceToResource(x, currentPosition)).ToList().First();
    }

    private float GetDistanceToResource(Resource resource, Vector3 currentPosition)
    {
        return (resource.transform.position - currentPosition).sqrMagnitude;
    }

    private void UpdateResources(List<Resource> resources)
    {
        resources = resources.Except(_resourcesUnmarked).Except(_resourcesMarkedForGathering).ToList();
        _resourcesUnmarked.AddRange(resources);
    }

    private void DeleteCollectedResource(Resource resource)
    {
        _resourcesMarkedForGathering.Remove(resource);
        resource.Collected -= DeleteCollectedResource;
    }
}
