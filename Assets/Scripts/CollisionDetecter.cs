using System;
using UnityEngine;

public class CollisionDetecter : MonoBehaviour
{
    public event Action<Cube> OnCollisionEntered;
    
    private void OnCollisionEnter(Collision collision)
    {
        Cube cube = collision.gameObject.GetComponent<Cube>();
        
        if (cube != null)
        {
            OnCollisionEntered?.Invoke(cube);
        }
    }
}