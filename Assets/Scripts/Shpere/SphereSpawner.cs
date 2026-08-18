using UnityEngine;

public class SphereSpawner : Spawner<Sphere>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void OnEnable()
    {
        _cubeSpawner.Released += SpawnAtDeathPosition;
    }

    private void OnDisable()
    {
        _cubeSpawner.Released -= SpawnAtDeathPosition;
    }

    private void SpawnAtDeathPosition(Vector3 position)
    {
        Spawn(position);
    }
}
