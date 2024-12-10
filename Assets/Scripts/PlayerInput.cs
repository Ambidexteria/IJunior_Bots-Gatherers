using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _canceledActionButton;

    public event Action LeftMouseButtonClicked;
    public event Action ActionCanceled;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonClicked?.Invoke();
        }
        else if (Input.GetKeyDown(_canceledActionButton))
        {
            ActionCanceled?.Invoke();
        }
    }
}
