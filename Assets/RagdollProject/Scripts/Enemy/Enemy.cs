using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Mover _mover;
    [SerializeField] private EnemyView _view;
    [SerializeField] private RagdollHandler _ragdollHandler;

    public void Initialize()
    {
        _view.Initialize();
        _ragdollHandler.Initialize(transform, _view.GetBone(HumanBodyBones.Spine));
    }

    public void StartRun()
    {
        _view.StartRunning();
        _mover.Enable();
    }

    public void Kill()
    {
        Debug.Log("�����, � ������!(");

        EnableRagdollBehaviour();
    }

    public void TakeDamage(Vector3 force, Vector3 hitPoint)
    {
        Debug.Log("��, ������!");

        EnableRagdollBehaviour();

        _ragdollHandler.Hit(force, hitPoint);
    }

    public void StandUp()
    {
        _ragdollHandler.AdjustParentTransformToRagdoll();
        DisableRagdollBehaviour();
    }

    private void EnableRagdollBehaviour()
    {
        _view.DisableAnimator();
        _mover.Disable();
        _ragdollHandler.Enable();
    }

    private void DisableRagdollBehaviour()
    {
        _view.EnableAnimator();
        _ragdollHandler.Disable();
    }
}
