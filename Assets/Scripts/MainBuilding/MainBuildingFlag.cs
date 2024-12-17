using UnityEngine;

public class MainBuildingFlag : MonoBehaviour
{
    public bool IsConstructionStarted { get; private set; }

    public void Place(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void StartConstruction()
    {
        IsConstructionStarted = true;
    }

    public void EndConstruction()
    {
        IsConstructionStarted = false;
    }
}
