using System.Collections;
using UnityEngine;


public class EnemySpawner : MonoBehaviour, IEnemySpawner
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 3f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPosition;
        do
        {
            // Generate a random point within a circle for x and z (or y for 2D)
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            spawnPosition = new Vector3(
                player.position.x + randomPoint.x,  // Horizontal position
                player.position.y,                // Match player's height
                player.position.z + randomPoint.y // Depth position
            );
        }
        while (Vector3.Distance(spawnPosition, player.position) < 3f);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
