using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnRadius;
    public float minDistanceFromPlayer;
    public Transform player;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPosition;
            do
            {
                spawnPosition = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;
            }
            while (Vector2.Distance(spawnPosition, player.position) < minDistanceFromPlayer);

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
