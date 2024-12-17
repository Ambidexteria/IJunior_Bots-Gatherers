using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action LeftMouseButtonClicked;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonClicked?.Invoke();
        }
    }
}
