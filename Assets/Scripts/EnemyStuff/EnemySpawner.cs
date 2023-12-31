using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class EnemySpawner : MonoBehaviour
{
    // References:
    public GameObject[] _enemyPrefab;
    private GameManager gameManager;
    private Player _pc;
    // Variables:
    public float spawnTimeInterval;
    private float startTime;
    public int enemiesToSpawn;
    private int enemiesSpawned;
    [SerializeField] private bool deactivated;


    //variables for player distance
    [SerializeField] float distanceFromPlayer;
    public float minDist;
    //=============================================Unity Built-in Methods===============================================
    void Awake()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        _pc = FindObjectOfType<Player>();
        // Initialise variables.
        startTime = Time.time;
        enemiesSpawned = 0;
        deactivated = true;
        enemiesToSpawn += gameManager.roomCount * gameManager.roomCount / 4;
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
            if (enemiesSpawned >= enemiesToSpawn)
            {
                // Deactivate the spawner.
                Destroy(GetComponentInChildren<ParticleSystem>());
                deactivated = true;
            }
            else if (Time.time > startTime + spawnTimeInterval)
            {
                distanceFromPlayer = Vector3.Distance(this.transform.position, _pc.transform.position);

                if (distanceFromPlayer > minDist)
                {
                    // Otherwise we still have enemies to spawn, continue spawning.
                    SpawnEnemy();
                    startTime = Time.time;
                }

            }
        }
    }
    //=============================================Spawning Enemies===============================================
    private void SpawnEnemy()
    {
        // Create an enemy from the prefab attached to this object: Increase spawned counter.
        GameObject enemyObject = Instantiate(_enemyPrefab[Random.Range(0, _enemyPrefab.Length)], transform.position, Quaternion.identity);
        enemyObject.transform.parent = this.transform.parent;
        enemiesSpawned += 1;
    }
}