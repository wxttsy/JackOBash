using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An enemy that attacks the player at range with projectiles. 
/// </summary>
public class EnemyRanged : MonoBehaviour
{
    // References:
    private Animator animator;
    public GameObject projectile;

    public enum STATE
    {
        CHASE,
        ATTACKING,
        HIT,
        DEAD
    }
    public STATE currentState;
    private CharacterController cc;
    [SerializeField] public UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField] public Transform targetPlayer;
    // Movement variables:
    [Tooltip("The speed at which this enemy will move.")]
    public float moveSpeed = 3;
    Vector3 oldPosition;
    public float distToPlayer;
    //=============================================Unity Built-in Methods===============================================
    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        navMeshAgent.speed = moveSpeed;
        currentState = STATE.CHASE;
        animator = GetComponentInChildren<Animator>();
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
                //Stop moving
                navMeshAgent.SetDestination(transform.position);
                break;
            case STATE.DEAD:
                //Stop moving
                navMeshAgent.SetDestination(transform.position);
                break;
        }    
    }
    //=============================================Calculate Movement===============================================
    /// <summary>
    /// Calculates the movement trajectory of the ranged enemy. 
    /// </summary>
    private void CalculateEnemyMovement()
    {
        distToPlayer = Vector3.Distance(targetPlayer.position, transform.position);
        if (distToPlayer >= 8 && distToPlayer <= 11)
        {
            SwitchStateTo(STATE.ATTACKING);
        }

        if (distToPlayer > navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
        else if (distToPlayer < navMeshAgent.stoppingDistance * 0.8)
        {
            Vector3 playerPos = targetPlayer.position;
            Vector3 enemyPos = transform.position;

            Vector3 directionTowardsPlayer = (playerPos - enemyPos).normalized;
            float distanceToBeAwayFrom = navMeshAgent.stoppingDistance * 1.2f;

            Vector3 destination = enemyPos - directionTowardsPlayer * distanceToBeAwayFrom;
            destination.y = 1;
            navMeshAgent.SetDestination(destination);
        }
        
        oldPosition = transform.position;
    }

    /// <summary>
    /// Changes enemy state to desired state upon function call. 
    /// </summary>
    /// <param name="_newState">The state to change to. Type: EnemyRanged.STATE</param>
    public void SwitchStateTo(STATE _newState)
    {
        // Enter new state
        switch (_newState)
        {
            case STATE.CHASE:
                break;
            case STATE.ATTACKING:
                // Update Animator: Play animation for attacking.
                animator.SetTrigger("Attacking");
                break;
            case STATE.HIT:

                break;
            case STATE.DEAD:
                animator.SetTrigger("Death");
                break;
        }
        currentState = _newState;
    }

    /// <summary>
    /// Called at the end of the attack animation (animation event). 
    /// </summary>
    public void AttackAnimationEnd()
    {
        //This method is called in an animation event at the end of the attack animation.
        //It uses the EnemyMelee script on the visual object.
        //This means we need to get the melee script attached to the parented object "Zombie" and update its state from there.
        GameObject parentObject = transform.parent.gameObject;
        EnemyMelee parentScript = parentObject.GetComponent<EnemyMelee>();
        parentScript.SwitchStateTo(EnemyMelee.STATE.CHASE);
    }

    /// <summary>
    /// Called at the end of the death animation (animation event). 
    /// </summary>
    public void DeathAnimationEnd()
    {
        //This method is called in an animation event at the end of the attack animation.
        //It uses the EnemyMelee script on the visual object and destroy's its parent object.
        Destroy(transform.parent.gameObject);
    } 
}