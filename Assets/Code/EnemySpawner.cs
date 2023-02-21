using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 1.0f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0.0f, spawnRate);
    }

    void SpawnEnemy()
    {
        float spawnX = Random.Range(-10.0f, 10.0f);
        float spawnY = Random.Range(-10.0f, 10.0f);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0.0f);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}

