using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
   [SerializeField] private  GameObject _prefab;
   [SerializeField] private Transform _platform;
   
   private readonly float _spawnRadius = 4f; 
   private readonly float _height = 10f;
   private readonly int _poolSize = 10;
   private readonly int _poolCapasity = 10;
   private readonly float _repeatRate = 1f;
   private readonly int _lifeTime;
   
   private ObjectPool<GameObject> _pool;

   private void Awake()
   {
      _pool = new ObjectPool<GameObject>(
         createFunc: () => Instantiate(_prefab) ,
         actionOnGet: (obj) => ActionOnGet(obj),
         actionOnRelease: (obj) => obj.SetActive(false),
         actionOnDestroy: (obj) => Destroy(obj),
         collectionCheck: true,
         defaultCapacity: _poolCapasity,
         maxSize: _poolSize);
   }

   private void ActionOnGet(GameObject obj)
   {
      float x = Random.Range(-_spawnRadius, _spawnRadius);
      float z = Random.Range(-_spawnRadius, _spawnRadius);

      Vector3 spawnPos = _platform.position + new Vector3(x, _height, z);

      obj.transform.position = spawnPos;
      
      var rigidbody = obj.GetComponent<Rigidbody>();
      
      if (rigidbody != null)
         rigidbody.velocity = Vector3.zero;
      
      obj.SetActive(true);
   }

   private void GetCube()
   {
      _pool.Get();
   }

   private void Start()
   {
      InvokeRepeating(nameof(GetCube) ,0f, _repeatRate);
   }
   
   public void PoolRelease(GameObject obj)
   {
      if (obj == null)
      {
         return;
      }

      if (obj.activeInHierarchy == false)
      {
         return;
      }
      
      _pool.Release(obj);
   }
}