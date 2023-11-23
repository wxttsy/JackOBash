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


    [Header("Throw Settings")]
    public GameObject orb;
    float currentTime;
    public int cooldownTime;
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
        UpdateState();
        switch (currentState)
        {
            case STATE.CHASE:
                break;
            case STATE.ATTACKING:
                break;
            case STATE.DEAD:

                break;
        }
    }

    private void UpdateState()
    {

        float distance = Vector3.Distance(targetPlayer.position, transform.position);

        if (distance > navMeshAgent.stoppingDistance + 2)
        {

            SwitchStateTo(STATE.CHASE);
        }
        else if (distance < navMeshAgent.stoppingDistance - 2)
        {
            Debug.Log("YOINK");
            SwitchStateTo(STATE.FLEE);

        }
        else
        {
            SwitchStateTo(STATE.ATTACKING);
        }


        


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
            case STATE.FLEE:
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
                navMeshAgent.SetDestination(targetPlayer.position);
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
            case STATE.FLEE:
                navMeshAgent.SetDestination(transform.position - targetPlayer.position * 5);
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
        if(currentTime >= cooldownTime)
        {
            Instantiate(orb, this.transform);
            currentTime = 0;
        }

        currentTime += Time.deltaTime;

    }
}
