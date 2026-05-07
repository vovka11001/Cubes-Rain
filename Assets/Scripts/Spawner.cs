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
   private readonly float _repeatRate = 1f;
   
   private ObjectPool<Cube> _pool;

   private void Awake()
   {
      _pool = new ObjectPool<Cube>(
         createFunc: () => Instantiate(_cube) ,
         actionOnGet: (cube) => OnActionOnGet(cube),
         actionOnRelease: (cube) => cube.gameObject.SetActive(false),
         actionOnDestroy: (cube) => Destroy(cube.gameObject),
         collectionCheck: true,
         defaultCapacity: _poolCapasity,
         maxSize: _poolSize);
   }

   private void Start()
   {
       InvokeRepeating(nameof(GetCube), 0f, _repeatRate);
   }

   public void PoolRelease(Cube cube)
   {
       if (cube.gameObject == null)
       {
           return;
       }

       if (cube.gameObject.activeInHierarchy == false)
       {
           return;
       }

       _pool.Release(cube);
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
   }

   private void GetCube()
   {
      _pool.Get();
   }
}