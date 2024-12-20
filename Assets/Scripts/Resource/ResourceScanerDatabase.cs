using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ResourceScanerDatabase : MonoBehaviour
{
    private ResourceScaner _scaner;
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
        if (_scaner == null)
            throw new System.NullReferenceException();

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

    [Inject]
    private void Construct(ResourceScaner scaner)
    {
        _scaner = scaner;
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
