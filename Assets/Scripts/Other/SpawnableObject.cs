using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpawnableObject : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public event Action<SpawnableObject> Died;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    protected void Die() => Died?.Invoke(this);
}