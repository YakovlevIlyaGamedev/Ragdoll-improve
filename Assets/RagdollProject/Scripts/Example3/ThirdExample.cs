using UnityEngine;

public class ThirdExample : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void Awake()
    {
        _enemy.Initialize();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            _enemy.StartRun();

        if(Input.GetKeyDown(KeyCode.F))
            _enemy.StandUp();
    }
}
