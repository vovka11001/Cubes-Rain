using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(SphereCollider))]
public class Sphere : SpawnableObject
{
    [SerializeField] private Explosion _explosion;
    
    private readonly float _minLifeTime = 2f;
    private readonly float _maxLifeTime = 5f;
    
    private Renderer _renderer;
    private Coroutine _fadeCoroutine;

    protected override void Awake()
    {
        base.Awake();
        
        _renderer = GetComponent<Renderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _renderer.material.color = Color.black;
        _fadeCoroutine = StartCoroutine(FadeCoroutine());
    }

    private void OnDisable()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine); 
        }
        
        _fadeCoroutine = null;
    }

    private IEnumerator FadeCoroutine()
    {
        float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);
        float elapsedTime = 0f;

        while (elapsedTime < lifeTime)
        {
            elapsedTime += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / lifeTime));
            yield return null;
        }
        
        _explosion.Explode();
        Die();
    }
    
    private void SetAlpha(float alpha)
    {
        Color color = _renderer.material.color;
        color.a = alpha;
        _renderer.material.color = color;
    }
}