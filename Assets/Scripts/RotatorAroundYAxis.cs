using UnityEngine;

public class RotatorAroundYAxis : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 180f;

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
