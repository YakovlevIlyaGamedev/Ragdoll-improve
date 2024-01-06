using UnityEngine;

public class ThirdExample : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void Awake()
    {
        _enemy.Initialize();
    }
}
