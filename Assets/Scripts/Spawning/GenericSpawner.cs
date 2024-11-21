using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericSpawner<Type> : MonoBehaviour where Type : SpawnableObject
{
    [SerializeField] private Type _prefab;
    [SerializeField] private int _poolDefaultCapacity = 20;
    [SerializeField] private int _poolMaxSize = 100;

    private ObjectPool<Type> _pool;

    private void Awake()
    {
        if (_prefab == null)
            throw new NullReferenceException();

        InitializePool();

        PrepareOnAwake();
    }

    public virtual void PrepareOnAwake() { }

    public abstract Type Spawn();

    public abstract void Despawn(Type type);

    public void ReturnToPool(Type spawnedObject)
    {
        PrepareToDeactivate(spawnedObject);
        _pool.Release(spawnedObject);
    }

    public Type GetNextObject()
    {
        Type type = _pool.Get();
        type.gameObject.SetActive(true);

        return type;
    }

    public virtual void PrepareToDeactivate(Type spawnedObject) { }

    private Type PrepareForSpawn(Type spawnedObject)
    {
        spawnedObject.gameObject.SetActive(true);

        return spawnedObject;
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Type>(
            createFunc: () => Create(),
            actionOnGet: (spawnedObject) => PrepareForSpawn(spawnedObject),
            actionOnRelease: (spawnedObject) => spawnedObject.gameObject.SetActive(false),
            actionOnDestroy: (spawnedObject) => Destroy(spawnedObject.gameObject),
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize
            );
    }

    private Type Create()
    {
        return Instantiate(_prefab);
    }
}