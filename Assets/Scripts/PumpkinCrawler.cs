using System.Collections;
using System.Collections.Generic;
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

        if (Vector3.Distance(targetPlayer.position, transform.position) > navMeshAgent.stoppingDistance - 2)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
        else if (Vector3.Distance(targetPlayer.position, transform.position) < navMeshAgent.stoppingDistance + 2)
        {
            navMeshAgent.SetDestination(transform.position + Vector3.back);
        }
        // Update Rotation to face the direction immediately
        Quaternion newRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
        transform.rotation = newRotation;
    }
}
