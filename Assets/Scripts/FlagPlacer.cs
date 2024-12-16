using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;

    public bool TryGetPlaceForFlag(out Vector3 placePosition)
    {
        placePosition = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
