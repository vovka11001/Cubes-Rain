using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
   [SerializeField] private Cube _cubePrefab;
   [SerializeField] private Transform _platform;
   
   private static readonly float _elapsedTime = 1f;
   private readonly float _spawnRadius = 4f; 
   private readonly float _height = 10f;
   private readonly int _poolSize = 10;
   private readonly int _poolCapasity = 10;
   private bool _isCounting;
   
   private WaitForSeconds _waitForSeconds = new WaitForSeconds(_elapsedTime);
   private Coroutine _coroutine;
   private ObjectPool<Cube> _pool;

    private void Awake()
   {
      _pool = new ObjectPool<Cube>(
         createFunc: () => Instantiate(_cubePrefab),
         actionOnGet: (cube) => OnActionOnGet(cube),
         actionOnRelease: (cube) => OnReleaseCube(cube),
         actionOnDestroy: (cube) => Destroy(cube.gameObject),
         collectionCheck: true,
         defaultCapacity: _poolCapasity,
         maxSize: _poolSize);
   }

   private void Start()
   {
       StartCountDown();
   }

    private void PoolRelease(Cube cube)
   {
       if (cube == null)
        return;
       
       if (cube.gameObject.activeInHierarchy == false)
           return;
       
       if (cube != null && cube.gameObject.activeInHierarchy)
           _pool.Release(cube);
   }
    
    private void OnReleaseCube(Cube cube)
    {
        cube.TimerStopped -= PoolRelease;
        cube.gameObject.SetActive(false);
    }
    
    private void OnActionOnGet(Cube cube)
   {
      float positionX = Random.Range(-_spawnRadius, _spawnRadius);
      float positionZ = Random.Range(-_spawnRadius, _spawnRadius);

      Vector3 spawnPos = _platform.position + new Vector3(positionX, _height, positionZ);

      cube.gameObject.transform.position = spawnPos;
      
      cube.gameObject.SetActive(true);
      cube.TimerStopped += PoolRelease;
   }
   
   private void StartCountDown()
   {
       if (_isCounting)
           return;
       
       if (_coroutine != null)
           StopCoroutine(_coroutine);
       
       _isCounting = true;
       _coroutine = StartCoroutine(CountDown());
   }

   private IEnumerator CountDown()
   {
       while (_isCounting)
       {
           _pool.Get();
           yield return _waitForSeconds;
       }
   }
}