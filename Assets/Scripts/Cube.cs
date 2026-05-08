using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private List<Platform> _platforms = new List<Platform>();

    private readonly float _minLifeTime = 2;
    private readonly float _maxLifeTime = 5;
    private readonly float _elapsedTime = 1f;
    private int _count = 0;
    private bool _isCounting;
    private bool _isCollided;
    private float _lifeTime;

    private Coroutine _coroutine;

    public event Action<Cube> TimerStopped;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollided)
            return;

        Platform platform = collision.gameObject.GetComponent<Platform>();

        if (platform != null && _platforms.Contains(platform))
        {
            _isCollided = true;

            StartCountDown();

            Renderer renderer = GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = Random.ColorHSV();
            }

        }
    }

    private void OnEnable()
    {
        _platforms = FindObjectsByType<Platform>(FindObjectsSortMode.None).ToList();

        _count = 0;
        _isCounting = false;
        _isCollided= false;
        _lifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1f);

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }
    
    private void OnDisable()
    {
        StopCountDown();
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

    private void StopCountDown()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _isCounting = false;
    }

    private IEnumerator CountDown()
    {
        var wait = new WaitForSeconds(_elapsedTime);

        while (_isCounting)
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