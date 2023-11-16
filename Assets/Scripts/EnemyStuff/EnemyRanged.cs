using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    // References:
    private Animator animator;
    enum STATE
    {
        CHASE,
        ATTACKING,
        FLEE,
        HIT,
        DEAD
    }
    private STATE currentState;
    private CharacterController cc;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform targetPlayer;
    [Tooltip("The speed at which this enemy will move.")]
    public float moveSpeed = 3;
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
        CalculateEnemyMovement();
    }

    private void CalculateEnemyMovement()
    {
        if (currentState == STATE.ATTACKING) return;

        if (Vector3.Distance(targetPlayer.position, transform.position) > navMeshAgent.stoppingDistance-2)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
        else if (Vector3.Distance(targetPlayer.position, transform.position) < navMeshAgent.stoppingDistance+2)
        {
            navMeshAgent.SetDestination(transform.position - targetPlayer.position*5);
        }
        else
        {
            //currentState = STATE.ATTACKING;
            
        }
        // Update Rotation to face the direction immediately
        Quaternion newRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
        transform.rotation = newRotation;
    }
}
