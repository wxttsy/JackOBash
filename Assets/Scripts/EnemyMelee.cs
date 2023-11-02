using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class EnemyMelee : MonoBehaviour
{
    // References:
    private Animator animator;
    private DamageCollision attackHitbox;
    enum STATE
    {
        CHASE,
        ATTACKING,
        HIT,
        DEAD
    }
    private STATE currentState;
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
        currentState = STATE.CHASE;
        animator = GetComponent<Animator>();
        attackHitbox = GetComponentInChildren<DamageCollision>();
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
        }
    }
    //=============================================Calculate Movement===============================================
    private void CalculateEnemyMovement(){
        if (Vector3.Distance(targetPlayer.position, transform.position) >= navMeshAgent.stoppingDistance){
            navMeshAgent.SetDestination(targetPlayer.position);
        } else {
            navMeshAgent.SetDestination(transform.position);
            SwitchStateTo(STATE.ATTACKING);
        }
    }
    private void SwitchStateTo(STATE _newState)
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
                animator.SetTrigger("Attacking");
                // Stop Movement
                // Update Rotation to face the direction immediately
                Quaternion newRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
                transform.rotation = newRotation;
                break;
            case STATE.HIT:

                break;
            case STATE.DEAD:

                break;
        }
        currentState = _newState;
    }
    public void AttackAnimationEnd()
    {
        SwitchStateTo(STATE.CHASE);
    }
    public void EnableDamageCollider()
    {
        attackHitbox.EnableDamageCollider();

    }
    public void DisableDamageCollider()
    {
        attackHitbox.DisableDamageCollider();

    }
}
