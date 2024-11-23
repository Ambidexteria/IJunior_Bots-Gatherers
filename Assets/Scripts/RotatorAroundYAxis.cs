using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorAroundYAxis : MonoBehaviour
{
    [SerializeField] private Transform _objectToRotate;
    [SerializeField] private float _angularSpeed = 180f;
    [SerializeField] private bool _activate;

    private void Update()
    {
        if (_activate)
            Rotate();
    }

    public void Enable()
    {
        _activate = true;
    }

    public void Disable() 
    { 
        _activate = false; 
    }

    private void Rotate()
    {
        _objectToRotate.Rotate(Vector3.up, _angularSpeed * Time.deltaTime, Space.World);
    }
}
