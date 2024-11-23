using UnityEngine;

public class GroundOrientator : MonoBehaviour
{
    [SerializeField] private float _rayLength = 2f;

    public void Orientate()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -Vector3.up, _rayLength);

        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.transform.TryGetComponent(out Ground _))
            {
                transform.up = hit.normal;
            }
        }
    }
}
