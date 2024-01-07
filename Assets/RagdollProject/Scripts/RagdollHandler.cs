using System.Linq;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;

    public void Initialize(Transform parent)
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();

        Disable();
    }

    public void Hit(Vector3 force, Vector3 hitPosition)
    {
        Rigidbody injuredRigidbody = _rigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPosition)).First();

        injuredRigidbody.AddForceAtPosition(force, hitPosition, ForceMode.Impulse);
    }

    public void Enable()
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = false;
    }

    public void Disable()
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = true;
    }
}
