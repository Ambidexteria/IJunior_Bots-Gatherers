using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainBuildingPicker : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerInput.LeftMouseButtonClicked += PickMainBuilding;
    }

    private void OnDisable()
    {
        _playerInput.LeftMouseButtonClicked -= PickMainBuilding;
    }

    [Inject]
    public void Construct(PlayerInput input)
    {
        _playerInput = input;
    }

    private void PickMainBuilding()
    {
        Debug.Log(nameof(PickMainBuilding));
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, _mask))
        {
            if (hit.collider.transform.root.TryGetComponent(out IPickable pickableObject))
            {
                Debug.Log("picked");
                pickableObject.Pick();
            }
        }
    }
}
