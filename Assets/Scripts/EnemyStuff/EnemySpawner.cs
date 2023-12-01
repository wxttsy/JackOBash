using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // References:
    public GameObject[] _enemyPrefab;
    private GameManager gameManager;
    public Transform targetPlayer;
    public SpawnController spawnController;
    // Variables:
    public float spawnTimeInterval;
    private float startTime;
    public int enemiesToSpawn;
    public float distToPlayer;
    private int enemiesSpawned;
    public float tooClose;
    [SerializeField] private bool deactivated;
    //=============================================Unity Built-in Methods===============================================
    void Awake()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        spawnController = this.gameObject.GetComponentInParent<SpawnController>();
        // Initialise variables.
        startTime = Time.time;
        enemiesSpawned = 0;
        deactivated = true;
        //enemiesToSpawn += gameManager.roomCount * gameManager.roomCount / 4;
    }
    private void Update()
    {
        if (gameManager.currentRoom == this.gameObject.transform.parent.root.gameObject)
        {
            deactivated = false;
        }
        else
        {
            deactivated = true;
        }


        if (!deactivated)
        {
            // If this spawner has not yet summoned all of its enemies:
            if (spawnController.enemiesToSpawn <= 0)
            {
                // Deactivate the spawner.
                Destroy(GetComponentInChildren<ParticleSystem>());
                deactivated = true;
            }
            else if (Time.time > startTime + spawnTimeInterval)
            {
                distToPlayer = Vector3.Distance(targetPlayer.position, transform.position);
                //if player isnt too close to spawner
                if (distToPlayer >= tooClose)
                {
                    // Otherwise we still have enemies to spawn, continue spawning.
                    SpawnEnemy();
                }
                startTime = Time.time;
            }
        }
    }
    //=============================================Spawning Enemies===============================================
    private void SpawnEnemy()
    {
        // Create an enemy from the prefab attached to this object: Increase spawned counter.
        GameObject enemyObject = Instantiate(_enemyPrefab[Random.Range(0, _enemyPrefab.Length-1)], transform.position, Quaternion.identity);
        enemyObject.transform.parent = this.transform.parent;
        spawnController.enemiesToSpawn -= 1;
    }
}