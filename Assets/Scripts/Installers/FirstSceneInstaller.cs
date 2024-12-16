using UnityEngine;
using Zenject;

public class FirstSceneInstaller : MonoInstaller
{
    [SerializeField] private PlayerInput _playerInputPrefab;
    [SerializeField] private FlagPlacer _flagPlacerPrefab;
    [SerializeField] private MainBuilding _mainBuildingPrefab;
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private BotSpawner _botSpawnerPrefab;
    [SerializeField] private ResourceScanerDatabase _resourceScanerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<PlayerInput>().FromComponentInNewPrefab(_playerInputPrefab).AsSingle().NonLazy();
        Container.Bind<FlagPlacer>().FromInstance(_flagPlacerPrefab).AsSingle().NonLazy();
        Container.Bind<BotSpawner>().FromComponentInNewPrefab(_botSpawnerPrefab).AsSingle().NonLazy();
        Container.Bind<ResourceScanerDatabase>().FromComponentInNewPrefab(_resourceScanerPrefab).AsSingle().NonLazy();
        Container.Bind<ResourceScaner>().FromComponentInHierarchy(true).AsSingle().NonLazy();
        Container.Bind<MainBuildingSpawner>().FromComponentInHierarchy(true).AsSingle().NonLazy();

        //Container.BindFactory<MainBuilding, GenericSpawnableObjectFactory<MainBuilding>>().FromInstance(_mainBuildingPrefab).Lazy();
        //Container.BindFactory<Resource, GenericSpawnableObjectFactory<Resource>>().FromInstance(_resourcePrefab).Lazy();
        //Container.BindFactory<Bot, GenericSpawnableObjectFactory<Bot>>().FromInstance(_botPrefab).Lazy();

        Container.BindFactory<MainBuilding, GenericSpawnableObjectFactory<MainBuilding>>().FromComponentInNewPrefab(_mainBuildingPrefab).Lazy();
        Container.BindFactory<Resource, GenericSpawnableObjectFactory<Resource>>().FromComponentInNewPrefab(_resourcePrefab).Lazy();
        Container.BindFactory<Bot, GenericSpawnableObjectFactory<Bot>>().FromComponentInNewPrefab(_botPrefab).Lazy();

    }
}