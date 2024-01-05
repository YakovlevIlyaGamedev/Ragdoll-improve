using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Mover _mover;
    [SerializeField] private EnemyView _view;
    [SerializeField] private RagdollHandler _ragdollHandler;

    public void Initialize()
    {
        _view.Initialize();
        _ragdollHandler.Initialize();
    }

    public void StartRun()
    {
        _view.StartRunning();
        _mover.Enable();
    }

    public void Kill()
    {
        Debug.Log("Ќ≈≈≈“, € умееер!(");

        EnableRagdollBehaviour();
    }

    public void TakeDamage(Vector3 force, Vector3 hitPoint)
    {
        Debug.Log("јй, больно!");

        EnableRagdollBehaviour();

        _ragdollHandler.Hit(force, hitPoint);
    }

    private void EnableRagdollBehaviour()
    {
        _view.DisableAnimator();
        _mover.Disable();
        _ragdollHandler.Enable();
    }
}
