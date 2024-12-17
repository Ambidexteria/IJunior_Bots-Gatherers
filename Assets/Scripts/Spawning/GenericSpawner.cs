using System;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public abstract class GenericSpawner<Type> : MonoBehaviour where Type : SpawnableObject
{
    [SerializeField] private Type _prefab;
    [SerializeField] private int _poolDefaultCapacity = 20;
    [SerializeField] private int _poolMaxSize = 100;

    private ObjectPool<Type> _pool;
    private GenericSpawnableObjectFactory<Type> _factory;

    private void Awake()
    {
        if (_prefab == null)
            throw new NullReferenceException();

        InitializePool();

        PrepareOnAwake();
    }

    public Type Spawn()
    {
        return _pool.Get();
    }

    public void Despawn(Type spawnableObject)
    {
        PrepareForDespawn(ref spawnableObject);
        _pool.Release(spawnableObject);
    }

    public virtual void PrepareOnAwake() { }

    public virtual void PrepareForSpawn(ref Type spawnableObject) { }

    public virtual void PrepareForDespawn(ref Type spawnableObject) { }

    [Inject]
    private void Construct(GenericSpawnableObjectFactory<Type> factory)
    {
        _factory = factory;
    }

    private void InitializePool()
    {
        _pool = new ObjectPool<Type>(
            createFunc: () => Create(),
            actionOnGet: (spawnableObject) => PrepareForSpawn(ref spawnableObject),
            actionOnRelease: (spawnableObject) => spawnableObject.gameObject.SetActive(false),
            actionOnDestroy: (spawnableObject) => Destroy(spawnableObject.gameObject),
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize
            );
    }

    private Type Create()
    {
        return _factory.Create();
    }
}