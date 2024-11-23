using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardOrientator : MonoBehaviour
{
    [SerializeField] private Transform _orientationObject;

    private void Update()
    {
        
    }

    public void OrientateForward(Transform orintationObject)
    {
        Vector3 direction = Vector3.one;
    }
}
