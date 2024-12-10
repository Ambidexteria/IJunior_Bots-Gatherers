using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuildingFlag : MonoBehaviour
{
    private void Awake()
    {
        Hide();
    }

    public void Place(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
