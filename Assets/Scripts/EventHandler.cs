using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
  [SerializeField] private CollisionDetecter[] _collisionDetecters;
  [SerializeField] private Spawner _spawner;
  
  private List<Cube> _activeCubes = new List<Cube>();
  
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

        Renderer renderer = cube.GetComponent<Renderer>();

        if(renderer != null)
                renderer.material.color = Random.ColorHSV();
          
        cube.TimerStopped += OnCubeCountChanged;
        cube.StartCountDown();

    }
  }

    private void OnCubeCountChanged(Cube cube)
    {
        if (cube != null && _activeCubes.Contains(cube))
        {
            cube.TimerStopped -= OnCubeCountChanged;

            _activeCubes.Remove(cube);
            _spawner.PoolRelease(cube.gameObject);
        }
    }
}