using UnityEngine;

public class FirstExample : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void Awake()
    {
        _enemy.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
            _enemy.Kill();
    }
}
