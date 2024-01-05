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
        _mover.Enable();
        _view.StartRunning();
    }

    public void Kill()
    {
        _view.DisableAnimator();
        _ragdollHandler.Enable();
    }
}
