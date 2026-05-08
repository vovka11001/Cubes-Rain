using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
   [SerializeField] private Cube _cube;
   [SerializeField] private Transform _platform;
   
   private readonly float _spawnRadius = 4f; 
   private readonly float _height = 10f;
   private readonly int _poolSize = 10;
   private readonly int _poolCapasity = 10;
   private readonly float _elapsedTime = 1f;
   private bool _isCounting;
   
   private Coroutine _coroutine;
   private ObjectPool<Cube> _pool;

    private void Awake()
   {
      _pool = new ObjectPool<Cube>(
         createFunc: () => Instantiate(_cube),
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
      float x = Random.Range(-_spawnRadius, _spawnRadius);
      float z = Random.Range(-_spawnRadius, _spawnRadius);

      Vector3 spawnPos = _platform.position + new Vector3(x, _height, z);

      cube.gameObject.transform.position = spawnPos;
      
      var rigidbody = cube.GetComponent<Rigidbody>();
      
      if (rigidbody != null)
         rigidbody.velocity = Vector3.zero;
      
      cube.gameObject.SetActive(true);
      cube.TimerStopped += PoolRelease;
   }

   private void GetCube()
   {
      _pool.Get();
   }
   
   private void StartCountDown()
   {
       if (_isCounting)
       {
           return;
       }

       if (_coroutine != null)
       {
           StopCoroutine(_coroutine);
       }

       _isCounting = true;
       _coroutine = StartCoroutine(CountDown());
   }

   private IEnumerator CountDown()
   {
       var wait = new WaitForSeconds(_elapsedTime);

       while (_isCounting)
       {
           GetCube();
           yield return wait;
       }
   }
}