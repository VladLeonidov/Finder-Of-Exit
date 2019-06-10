using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int countEnemy;
    [SerializeField]
    private Vector3[] enemyPositions;

    private GameObject[] _enemies;

    private void Awake()
    {
        _enemies = new GameObject[countEnemy];
    }

    private void Update()
    {
        CreateEnemies();
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < countEnemy; i++)
        {
            if (_enemies[i] == null)
            {
                _enemies[i] = Instantiate(enemyPrefab) as GameObject;
                _enemies[i].transform.position = enemyPositions[i];
                float angle = Random.Range(0, 360);
                _enemies[i].transform.Rotate(0, angle, 0);
            }
        }
    }
}