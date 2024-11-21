using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnZone : MonoBehaviour
{
    [SerializeField] private float _verticalSpawnOffset;

    private Collider _collider;
    private Vector3 _scale;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;

        _scale = _collider.transform.localScale;
    }

    public Vector3 GetRandomSpawnPositionOnPlaneWithVerticalOffset()
    {
        Vector3 position = _collider.transform.position;
        float x = GetRandomCoordinate(_scale.x, position.x);
        float z = GetRandomCoordinate(_scale.z, position.z);
        float y = _collider.transform.position.y + _verticalSpawnOffset;

        return new Vector3(x, y, z);
    }

    private float GetRandomCoordinate(float scale, float position)
    {
        scale = Mathf.Abs(scale);

        float coordinate1 = position - (scale / 2);
        float coordinate2 = position + (scale / 2);

        return Random.Range(coordinate1, coordinate2);
    }
}
