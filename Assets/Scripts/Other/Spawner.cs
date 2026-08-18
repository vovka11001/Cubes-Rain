using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<TSpawnable> : MonoBehaviour, ISpawnerStats where TSpawnable : SpawnableObject
{
    [SerializeField] private TSpawnable _prefab;

    private readonly int _poolCapacity = 10;
    private readonly int _poolMaxSize = 20;

    private ObjectPool<SpawnableObject> _pool;

    public event Action<Vector3> Released;
    
    public int CreatedCount => _pool.CountAll;
    public int ActiveCount => _pool.CountActive;
    public int TotalSpawnedCount { get; private set; }

    protected virtual void Awake()
    {
        _pool = new ObjectPool<SpawnableObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: spawnable => spawnable.Died += ReleaseToPool,
            actionOnRelease: OnReleaseSpawnable,
            actionOnDestroy: spawnable => Destroy(spawnable.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    protected void Spawn(Vector3 position)
    {
        SpawnableObject spawnable = _pool.Get();

        spawnable.transform.position = position;
        spawnable.gameObject.SetActive(true);
        
        TotalSpawnedCount++;
    }

    private void OnReleaseSpawnable(SpawnableObject spawnable)
    {
        spawnable.Died -= ReleaseToPool;
        spawnable.gameObject.SetActive(false);
    }

    private void ReleaseToPool(SpawnableObject spawnable)
    {
        if (spawnable.gameObject.activeInHierarchy == false)
            return;

        Vector3 deathPosition = spawnable.transform.position;

        _pool.Release(spawnable);
        Released?.Invoke(deathPosition);
    }
}