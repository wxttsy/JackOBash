using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
/// <summary>
/// Added the basic framework for you @Jemma. I'll go over it with you when you're ready to work on it. Just let me know :)
/// </summary>
public class PumpkinCrawler : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform targetPlayer;

    [Tooltip("The speed at which this enemy will move.")]
    public float moveSpeed = 5;
    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        navMeshAgent.speed = moveSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        targetPlayer = GameObject.FindWithTag("Player").transform;
        float distance = Vector3.Distance(targetPlayer.position, transform.position);
        if (distance > navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(targetPlayer.position);

        }
        else if (distance < navMeshAgent.stoppingDistance * 0.8)
        {

            Vector3 playerPos = targetPlayer.position;
            Vector3 enemyPos = transform.position;

            Vector3 directionTowardsPlayer = (playerPos- enemyPos).normalized;
            float distanceToBeAwayFrom = navMeshAgent.stoppingDistance * 1.2f;

            Vector3 destination = enemyPos - directionTowardsPlayer * distanceToBeAwayFrom;
            destination.y = 1;
            navMeshAgent.SetDestination(destination);

        }
    }
}
