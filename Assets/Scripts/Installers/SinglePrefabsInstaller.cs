using UnityEngine;
using Zenject;

public class SinglePrefabsInstaller : MonoInstaller
{
    [SerializeField] private PlayerInput _playerInputPrefab;
    [SerializeField] private FlagPlacer _flagPlacerPrefab;
    [SerializeField] private BotSpawner _botSpawnerPrefab;
    [SerializeField] private ResourceScanerDatabase _resourceScanerPrefab;
    [SerializeField] private MainBuildingSpawner _mainBuildingSpawnerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<PlayerInput>().FromComponentInNewPrefab(_playerInputPrefab).AsSingle().NonLazy();
        Container.Bind<FlagPlacer>().FromInstance(_flagPlacerPrefab).AsSingle().NonLazy();
        Container.Bind<BotSpawner>().FromComponentInNewPrefab(_botSpawnerPrefab).AsSingle().NonLazy();
        Container.Bind<ResourceScanerDatabase>().FromComponentInNewPrefab(_resourceScanerPrefab).AsSingle().NonLazy();
        Container.Bind<MainBuildingSpawner>().FromComponentInNewPrefab(_mainBuildingSpawnerPrefab).AsSingle().NonLazy();
    }
}