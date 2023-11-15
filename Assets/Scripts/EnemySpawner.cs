using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // References:
    public GameObject _enemyPrefab;
    RoomSpawner rs;
    // Variables:
    public float spawnTimeInterval;
    private float startTime;
    public int enemiesToSpawn;
    private int enemiesSpawned;
    private bool deactivated;
    //=============================================Unity Built-in Methods===============================================
    void Awake()
    {
        // Initialise variables.
        rs = FindObjectOfType<RoomSpawner>();
        startTime = Time.time;
        enemiesSpawned = 0;
        deactivated = false;
        enemiesToSpawn += rs.roomCount * rs.roomCount/4;
        Debug.Log(enemiesToSpawn);
    }
    private void Update()
    {
        if (!deactivated)
        {
            // If this spawner has not yet summoned all of its enemies:
            if (enemiesSpawned >= enemiesToSpawn)
            {
                // Deactivate the spawner.
                Destroy(GetComponentInChildren<ParticleSystem>());
                deactivated = true;
            }
            else if (Time.time > startTime + spawnTimeInterval)
            {
                // Otherwise we still have enemies to spawn, continue spawning.
                SpawnEnemy();
                startTime = Time.time;
            }
        }
    }
    //=============================================Spawning Enemies===============================================
    private void SpawnEnemy()
    {
        // Create an enemy from the prefab attached to this object: Increase spawned counter.
        Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        enemiesSpawned += 1;
    }
}