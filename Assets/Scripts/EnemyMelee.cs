using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    // References:
    private CharacterController cc;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform targetPlayer;
    // Movement variables:
    [Tooltip("The speed at which this enemy will move.")]
    public float moveSpeed = 3;
    //=============================================Unity Built-in Methods===============================================
    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        navMeshAgent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateEnemyMovement();
    }
    //=============================================Calculate Movement===============================================
    private void CalculateEnemyMovement(){
        if (Vector3.Distance(targetPlayer.position, transform.position) >= navMeshAgent.stoppingDistance){
            navMeshAgent.SetDestination(targetPlayer.position);
        } else {
            navMeshAgent.SetDestination(transform.position);
        }
    }
}
