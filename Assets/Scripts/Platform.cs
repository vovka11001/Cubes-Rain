using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public event Action<Cube> CollisionEntered;
    
    private void OnCollisionEnter(Collision collision)
    {
        Cube cube = collision.gameObject.GetComponent<Cube>();
        
        if (cube != null)
        {
            CollisionEntered?.Invoke(cube);
        }
    }
}