using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour, IEnemySpawner
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private List<GameObject> enemy1;
    [SerializeField] private List<GameObject> enemy2;

    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float minSpawnRadius = 3f;


    private void OnEnable()
    {
        Enemy.OnEnemyKilledWithObject += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilledWithObject -= HandleEnemyKilled;
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemies), 1f, spawnInterval);
    }

    public void SpawnEnemies()
    {
        Vector3 spawnPosition;
        do
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            spawnPosition = new Vector3(
                player.position.x + randomPoint.x,
                player.position.y,
                player.position.z + randomPoint.y 
            );
        }
        while (Vector3.Distance(spawnPosition, player.position) < minSpawnRadius);

        int enemy = Random.Range(1,3);
        if (enemy == 1)
        {
            SpawnEnemy(enemy1, spawnPosition);
        }
        else if (enemy == 2)
        {
            SpawnEnemy(enemy2, spawnPosition);
        }
    }

    private void SpawnEnemy(List<GameObject> enemies, Vector3 spawnPosition)
    {
        GameObject enemy = enemies.Find(e => !e.activeInHierarchy);

        if (enemy != null)
        {
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;
            enemy.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No enemies available in the pool!");
        }
    }

    private void HandleEnemyKilled(Enemy enemy)
    {
        enemy1.Remove(enemy.gameObject);
        enemy2.Remove(enemy.gameObject);
    }

}
