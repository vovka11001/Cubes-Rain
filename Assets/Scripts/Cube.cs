using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    private readonly int _minLifeTime = 2;
    private readonly int _maxLifeTime = 5;
    private readonly float _elapsedTime = 1f;
    private int _count = 0;
    private bool _isCounting;
    
    public Coroutine Coroutine {get; private set;}
    public int LifeTime {get; private set;}
    public int Count => _count;

    public event Action<Cube> CountChanged;
    
    private void Start()
    {
        LifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1);
        
        if (GetComponent<Rigidbody>() == null)
            gameObject.AddComponent<Rigidbody>();
    }
    
    private IEnumerator Countdown()
    {
        var wait = new WaitForSeconds(_elapsedTime);

        while (_isCounting)
        {
            _count++;
            CountChanged?.Invoke(this);
            
            yield return wait;
        }
    }

    public void StartCoroutine()
    {
        if (_isCounting)
        {
            return;
        }
        
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Coroutine = null;
        }
        
        _isCounting = true;
        Coroutine = StartCoroutine(Countdown());
    }

    public void Stop()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Coroutine = null;
        }
        
        _isCounting = false;
    }
}