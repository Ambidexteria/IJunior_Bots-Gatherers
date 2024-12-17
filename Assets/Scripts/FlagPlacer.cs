using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _raycastLength = 200f;

    public bool TryGetPlaceForFlag(out Vector3 placePosition)
    {
        placePosition = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastLength, _mask))
        {
            if (hit.collider.transform.root.TryGetComponent(out Ground _))
            {
                placePosition = hit.point;
                return true;
            }
        }

        return false;
    }
}
