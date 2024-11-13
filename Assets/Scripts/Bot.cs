using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    private bool _isActive = true;

    public event Action CommandCompleted;

    public bool IsActive => _isActive;

    public void SetStrategy()
    {

    }

    private void Move()
    {
        
    }

    private void TakeResource()
    {

    }

    private void UnloadResource()
    {

    }
}
