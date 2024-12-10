using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionPoint : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private MainBuildingPicker _mainBuildingPicker;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerInput _playerInput;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public bool TryPlaceFlagForANewBuilding(out Vector3 placePosition)
    {
        placePosition = Vector3.zero;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, _mask))
        {
            if (hit.collider.transform.root.TryGetComponent(out Ground mainBuilding))
            {
                Debug.Log("ground is found");
                placePosition = hit.point;
                return true;
            }
        }

        return false;
    }
}
