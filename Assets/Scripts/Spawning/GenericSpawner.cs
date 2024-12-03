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

    public abstract void Despawn(Type spawnableObject);

    public void ReturnToPool(Type spawnableObject)
    {
        PrepareToDeactivate(spawnableObject);
        _pool.Release(spawnableObject);
    }

    public Type GetNextObject()
    {
        Type nextObject = _pool.Get();

        return nextObject;
    }

    public virtual void PrepareToDeactivate(Type spawnableObject) { }

    private Type PrepareForSpawn(Type spawnedObject)
    {
        return spawnedObject;
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Type>(
            createFunc: () => Create(),
            actionOnGet: (spawnableObject) => PrepareForSpawn(spawnableObject),
            actionOnRelease: (spawnableObject) => spawnableObject.gameObject.SetActive(false),
            actionOnDestroy: (spawnableObject) => Destroy(spawnableObject.gameObject),
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize
            );
    }

    private Type Create()
    {
        return Instantiate(_prefab);
    }
}