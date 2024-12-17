using UnityEngine;
using Zenject;

public class BuildingPicker : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _raycastLength = 200f;

    private Camera _mainCamera;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _playerInput.LeftMouseButtonClicked += PickMainBuilding;
    }

    private void OnDisable()
    {
        _playerInput.LeftMouseButtonClicked -= PickMainBuilding;
    }

    [Inject]
    private void Construct(PlayerInput input)
    {
        _playerInput = input;
    }

    private void PickMainBuilding()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastLength, _mask))
        {
            if (hit.collider.transform.root.TryGetComponent(out IPickable pickableObject))
            {
                pickableObject.Pick();
            }
        }
    }
}
