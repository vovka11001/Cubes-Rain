using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    
    private readonly float _minLifeTime = 2;
    private readonly float _maxLifeTime = 5;
    private readonly float _elapsedTime = 1f;
    private int _count;
    private bool _isCollided;
    private float _lifeTime;

    private Coroutine _coroutine;
    public event Action<Cube> TimerStopped;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollided)
            return;
        
        if (collision.gameObject.TryGetComponent(out Platform platform))
        {
            _isCollided = true;

            StartCountDown();

            _renderer.material.color = Random.ColorHSV();
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        
        _renderer.material.color = Color.white;
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        _count = 0;
        _isCollided= false;
        _lifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1f);
    }
    
    private void OnDisable()
    {
        StopCountDown();
    }

    private void StartCountDown()
    {
        if (_coroutine != null) 
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CountDown());
    }

    private void StopCountDown()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator CountDown()
    {
        var wait = new WaitForSeconds(_elapsedTime);

        for (float i = 0; i < _lifeTime; i++)
        {
            _count++;

            if (_count >= _lifeTime)
            {
                StopCountDown();
            
                if (gameObject.activeInHierarchy)
                { 
                    TimerStopped?.Invoke(this);
                }
            
                yield break;
            }
            
            yield return wait;
        }
    }
}