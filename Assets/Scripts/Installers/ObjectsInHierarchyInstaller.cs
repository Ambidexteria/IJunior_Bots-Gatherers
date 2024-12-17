using Zenject;

public class ObjectsInHierarchyInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ResourceScaner>().FromComponentInHierarchy(true).AsSingle().NonLazy();
    }
}