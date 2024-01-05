using System.Collections.Generic;
using UnityEngine;

public class SecondExample : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemies;

    private void Awake()
    {
        foreach (Enemy enemy in _enemies)
        {
            enemy.Initialize();
            enemy.StartRun();
        }
    }
}
