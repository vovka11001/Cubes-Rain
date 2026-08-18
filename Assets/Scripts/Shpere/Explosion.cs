using UnityEngine;
using UnityEngine.Serialization;

public class Explosion : MonoBehaviour
{
    [SerializeField] private LayerMask _affectedLayer;

    private readonly float _explosionForce = 700f;
    private readonly float _explosionRadius = 8f;
    private readonly int _maxAffectedColliders = 15;

    private Collider[] _overlapBuffer;

    private void Awake()
    {
        _overlapBuffer = new Collider[_maxAffectedColliders];
    }

    public void Explode()
    {
        int foundCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _overlapBuffer, _affectedLayer);

        for (int i = 0; i < foundCount; i++)
        {
            Rigidbody affectedBody = _overlapBuffer[i].attachedRigidbody;

            if (affectedBody != null)
                affectedBody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }
    }
}