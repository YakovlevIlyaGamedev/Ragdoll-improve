using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private EnemyView _view;

    private void Awake()
    {
        _view.Initialize();
        _mover.Enable();
        _view.StartRunning();
    }
}
