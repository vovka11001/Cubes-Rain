using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(BoxCollider))]
public class Cube : SpawnableObject
{
   private readonly float _minLifeTime = 2f;
   private readonly float _maxLifeTime = 5f;
   private bool _hasLanded;
   
   private Renderer _renderer;
   private Coroutine _cubeLifetime;

   protected override void Awake()
   {
      base.Awake();
      _renderer = GetComponent<Renderer>();
   }

   protected override void OnEnable()
   {
      base.OnEnable();
      
      _hasLanded = false;
      _renderer.material.color = Color.white;
   }

   private void OnDisable()
   {
      if(_cubeLifetime == null)
         return;
      
      if(_cubeLifetime != null)
        StopCoroutine(_cubeLifetime);
      
      _cubeLifetime = null;
   }

   private void OnCollisionEnter(Collision collision)
   {
      if (_hasLanded)
         return;

      if (collision.gameObject.TryGetComponent(out Platform platform) == false)
         return;
      
      _hasLanded = true;
      _renderer.material.color = Random.ColorHSV();
      _cubeLifetime = StartCoroutine(DeathCountDown());
   }
   
   private IEnumerator DeathCountDown()
   {
      yield return new WaitForSeconds(Random.Range(_minLifeTime, _maxLifeTime));
      
      Die();
   }
}