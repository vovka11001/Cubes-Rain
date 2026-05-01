using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
  [SerializeField] private CollisionDetecter[] _collisionDetecters;
  [SerializeField] private Spawner _spawner;
  
  private HashSet<Cube> _activeCubes = new HashSet<Cube>();
  
  private void OnEnable()
  {
    foreach (CollisionDetecter collisionDetecter in _collisionDetecters)
      collisionDetecter.OnCollisionEntered += HandlerRelease;
  }

  private void OnDisable()
  {
    foreach (CollisionDetecter collisionDetecter in _collisionDetecters)
      collisionDetecter.OnCollisionEntered -= HandlerRelease;
  }

  private void HandlerRelease(Cube cube)
  {
    if (cube != null && !_activeCubes.Contains(cube))
    {
        _activeCubes.Add(cube);
        cube.CountChanged += CheckAndReleaseCube;
        cube.StartCoroutine();
    }
  }
    
  private void CheckAndReleaseCube(Cube cube)
  {
    if (cube.LifeTime == cube.Count)
    {
      cube.CountChanged -= CheckAndReleaseCube;
      _activeCubes.Remove(cube);
      cube.Stop();
      _spawner.PoolRelease(cube.gameObject);
    }
  }
}