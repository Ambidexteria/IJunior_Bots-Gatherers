using UnityEngine;
using Zenject;

public class SpawnableObjectsInstaller : MonoInstaller
{
    [SerializeField] private MainBuilding _mainBuildingPrefab;
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private Bot _botPrefab;

    public override void InstallBindings()
    {
        var abc = Container.BindFactory<MainBuilding, GenericSpawnableObjectFactory<MainBuilding>>().FromComponentInNewPrefab(_mainBuildingPrefab).Lazy();
        Container.BindFactory<Resource, GenericSpawnableObjectFactory<Resource>>().FromComponentInNewPrefab(_resourcePrefab).Lazy();
        Container.BindFactory<Bot, GenericSpawnableObjectFactory<Bot>>().FromComponentInNewPrefab(_botPrefab).Lazy();
    }
}