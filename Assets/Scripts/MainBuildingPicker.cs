using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuildingPicker : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerInput _playerInput;

    public event Action<MainBuilding> Picked;

    private void OnEnable()
    {
        _playerInput.LeftMouseButtonClicked += PickMainBuilding;
    }

    private void OnDisable()
    {
        _playerInput.LeftMouseButtonClicked -= PickMainBuilding;
    }

    private void PickMainBuilding()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, _mask))
        {
            if (hit.collider.transform.root.TryGetComponent(out MainBuilding mainBuilding))
            {
                Picked?.Invoke(mainBuilding);
            }
        }
    }
}
