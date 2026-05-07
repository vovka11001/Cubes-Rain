using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
  [SerializeField] private Spawner _spawner;

  private List<Cube> _activeCubes = new List<Cube>();
  private List<Platform> _platforms = new List<Platform>();
  
  private void Awake()
   {
       _platforms = FindObjectsByType<Platform>(FindObjectsSortMode.None).ToList();
   }

  private void OnEnable()
   {
        foreach (var platform in _platforms)
        {
            platform.CollisionEntered += HandlerRelease;
        }
   }

  private void OnDisable()
  {
        foreach (var platform in _platforms)
        {
            platform.CollisionEntered -= HandlerRelease;
        }
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
          _spawner.PoolRelease(cube);
      }
  }
}