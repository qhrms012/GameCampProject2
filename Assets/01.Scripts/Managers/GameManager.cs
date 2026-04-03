using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    EnemySpawner enemySpawner;

    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }
    private void Start()
    {
        enemySpawner.SpawnEnemy();
    }
}
