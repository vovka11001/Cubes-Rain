using System.Collections;
using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private Transform _platform;

    private readonly float _spawnDelay = 1f;
    private readonly float _spawnRadius = 4f;
    private readonly float _spawnHeight = 10f;

    private void Start()
    {
        StartCoroutine(SpawnRepeatedly());
    }

    private IEnumerator SpawnRepeatedly()
    {
        WaitForSeconds spawnInterval = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            float positionX = Random.Range(-_spawnRadius, _spawnRadius);
            float positionZ = Random.Range(-_spawnRadius, _spawnRadius);

            Spawn(_platform.position + new Vector3(positionX, _spawnHeight, positionZ));

            yield return spawnInterval;
        }
    }
}