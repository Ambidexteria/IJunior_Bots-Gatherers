using UnityEngine;

public interface IBuilding
{
    float ConstructionTime { get; }
    void Place(Vector3 position);
    void Enable();
}
