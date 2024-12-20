using UnityEngine;

public interface IBuilding
{
    float Size { get; }
    float ConstructionTime { get; }
    void Place(Vector3 position);
    void Enable();
}
