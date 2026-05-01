using System;
using System.Collections;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private float _elapsedTime = 1f;
    private int _count = 0;
    private bool _isCounting;
    public Coroutine Coroutine {get; private set;}

    public event Action CountChanged;

    public int Count => _count;

    public void StartCoroutine()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
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

    private IEnumerator Countdown()
    {
        var wait =  new WaitForSeconds(_elapsedTime);

        while (_isCounting)
        {
            _count++;
            CountChanged?.Invoke();
            yield return wait;
        }
    }
}
