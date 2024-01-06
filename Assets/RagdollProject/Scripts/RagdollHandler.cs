using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    private List<Rigidbody> _rigidbodies;

    private Transform _parent;
    private Transform _hipsBone;

    public void Initialize(Transform parent, Transform hipsBone)
    {
        _rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        _parent = parent;
        _hipsBone = hipsBone;
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

    public void AdjustParentTransformToRagdoll()
    {
        AdjustParentRotationToRagdoll();
        AdjustParentPositionToRagdoll();
    }

    private void AdjustParentPositionToRagdoll()
    {
        Vector3 initHipsPosition = _hipsBone.position;
        _parent.position = initHipsPosition;

        AdjustParentPositionRelativeGround();

        _hipsBone.position = initHipsPosition;
    }

    //Убрать слой в константу
    private void AdjustParentPositionRelativeGround()
    {
        if (Physics.Raycast(_parent.position, Vector3.down, out RaycastHit hit, 5, 1 << LayerMask.NameToLayer("Ground")))
            _parent.position = new Vector3(_parent.position.x, hit.point.y, _parent.position.z);
    }

    //скорректировать
    private void AdjustParentRotationToRagdoll()
    {
        Vector3 initHipsPosition = _hipsBone.position;
        Quaternion initHipsRotation = _hipsBone.rotation;

        Vector3 directionForRotate = -_hipsBone.up;
        directionForRotate.y = 0;

        Quaternion correctionRotation = Quaternion.FromToRotation(_parent.forward, directionForRotate.normalized); 
        _parent.rotation *= correctionRotation;

        _hipsBone.position = initHipsPosition;
        _hipsBone.rotation = initHipsRotation;
    }
}
