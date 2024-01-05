using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private EnemyView _view;
    [SerializeField] private RagdollHandler _ragdollHandler;

    private void Awake()
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
        _view.DisableAnimator();
        _mover.Disable();
        _ragdollHandler.Enable();
    }
}
