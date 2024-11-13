using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    [SerializeField] private Bot[] _bots;
    [SerializeField] private Resource[] _resourcesOnMap;
    [SerializeField] private int _resources;
    [SerializeField] private ResourceScaner _resourceScaner;

    private void ScanForResources()
    {

    }

    private void SendBot(Vector3 position)
    {

    }
}
