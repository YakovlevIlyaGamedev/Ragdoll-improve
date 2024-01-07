using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Mover _mover;
    [SerializeField] private EnemyView _view;
    [SerializeField] private RagdollHandler _ragdollHandler;

    public void Initialize()
    {
        _view.Initialize(transform);
        _ragdollHandler.Initialize(transform);
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

    public void StandUp()
    {
        _ragdollHandler.Disable();
        _view.PlayStandingUp(_view.EnableAnimator, _view.StartIdling);
    }

    private void EnableRagdollBehaviour()
    {
        _view.DisableAnimator();
        _mover.Disable();
        _ragdollHandler.Enable();
    }
}
