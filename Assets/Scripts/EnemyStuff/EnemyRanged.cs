using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    // References:
    private Animator animator;
    public enum STATE
    {
        CHASE,
        ATTACKING,
        FLEE,
        HIT,
        DEAD
    }
    public STATE currentState;
    private CharacterController cc;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform targetPlayer;
    [Tooltip("The speed at which this enemy will move.")]
    public float moveSpeed = 3;
    public GameObject orb;
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
        switch (currentState)
        {
            case STATE.CHASE:
                CalculateEnemyMovement();
                break;
            case STATE.ATTACKING:
                break;
            case STATE.DEAD:

                break;
        }
    }

    private void CalculateEnemyMovement()
    {
        if (currentState == STATE.ATTACKING) return;

        if (Vector3.Distance(targetPlayer.position, transform.position) > navMeshAgent.stoppingDistance-2)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
        else if (Vector3.Distance(targetPlayer.position, transform.position) <= navMeshAgent.stoppingDistance + 2)
        {
            Debug.Log("YOINK");
            SwitchStateTo(STATE.ATTACKING);

        }
        
        if (Vector3.Distance(targetPlayer.position, transform.position) < navMeshAgent.stoppingDistance+2)
        {
            navMeshAgent.SetDestination(transform.position - targetPlayer.position*5);
        }

        // Update Rotation to face the direction immediately
        Quaternion newRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
        transform.rotation = newRotation;
    }


    public void SwitchStateTo(STATE _newState)
    {
        // Exit current state
        switch (currentState)
        {
            case STATE.CHASE:
                break;
            case STATE.ATTACKING:
                break;
            case STATE.HIT:
                break;
            case STATE.DEAD:
                break;
        }
        // Enter new state
        switch (_newState)
        {
            case STATE.CHASE:
                break;
            case STATE.ATTACKING:
                // Update Animator: Play animation for attacking.
                Debug.Log("YOINK");
                OrbSpawn();
                // Stop Movement
                // Update Rotation to face the direction immediately
                Quaternion newRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
                transform.rotation = newRotation;
                break;
            case STATE.HIT:

                break;
            case STATE.DEAD:
                animator.SetTrigger("Death");
                break;
        }
        currentState = _newState;
    }


    public void OrbSpawn()
    {
        Instantiate(orb, this.transform);
    }
}
