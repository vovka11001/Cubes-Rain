using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private readonly float _minLifeTime = 2;
    private readonly float _maxLifeTime = 5;
    private readonly float _elapsedTime = 1f;
    private int _count = 0;
    private bool _isCounting;
    
    public Coroutine Coroutine {get; private set;}
    public float LifeTime {get; private set;}

    public event Action<Cube> TimerStopped;

    private void OnEnable()
    {
        _count = 0;
        _isCounting = false;
        LifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1);

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }

    public void StartCountDown()
    {
        if (_isCounting)
        {
            return;
        }

        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        _isCounting = true;
        Coroutine = StartCoroutine(CountDown());
    }

    public void StopCountDown()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Coroutine = null;
        }

        _isCounting = false;
    }

    private IEnumerator CountDown()
    {
        var wait = new WaitForSeconds(_elapsedTime);

        while (_isCounting)
        {
            _count++;

            if (_count >= LifeTime)
            {
                StopCountDown();
                TimerStopped?.Invoke(this);

                yield break;
            }

            yield return wait;
        }
    }
}