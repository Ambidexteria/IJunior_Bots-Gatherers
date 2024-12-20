public class MainBuildingIdleState : IBuildingState
{
    private MainBuilding _building;

    public MainBuildingIdleState(MainBuilding building)
    {
        _building = building;
    }

    public void OnStart() { }

    public void OnStop() { }

    public void OnUpdate()
    {
        _building.CreateNewBot();

        _building.GatherResources();
    }
}
