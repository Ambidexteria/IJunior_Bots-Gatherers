using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public bool TryGetPlaceForFlag(out Vector3 placePosition)
    {
        placePosition = Vector3.zero;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, _mask))
        {
            if (hit.collider.transform.root.TryGetComponent(out Ground mainBuilding))
            {
                placePosition = hit.point;
                return true;
            }
        }

        return false;
    }
}
