using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// An enemy that upon death drops an abundance of candy. 
/// </summary>
public class PumpkinCrawler : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform targetPlayer;
    Animator _anim;
    public bool isDead;

    [Tooltip("The speed at which this enemy will move.")]
    public float moveSpeed = 5;
    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        navMeshAgent.speed = moveSpeed;
        _anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {

        if (!isDead)
        {
            if (navMeshAgent.velocity != Vector3.zero)
            {
                _anim.SetTrigger("Walking");
            }
            else if (navMeshAgent.velocity == Vector3.zero)
            {
                _anim.SetTrigger("Wait");
            }

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

                Vector3 directionTowardsPlayer = (playerPos - enemyPos).normalized;
                float distanceToBeAwayFrom = navMeshAgent.stoppingDistance * 1.2f;

                Vector3 destination = enemyPos - directionTowardsPlayer * distanceToBeAwayFrom;
                destination.y = 1;
                navMeshAgent.SetDestination(destination);

            }
        }
        

        if(isDead)
        {
            navMeshAgent.velocity = Vector3.zero;
            _anim.SetTrigger("Dead");
        }






    }

    /// <summary>
    /// Destroys pumpkin crawler upon the end of the death animation (animation event). 
    /// </summary>
    void DeathAnimEnd()
    {
        Destroy(this.gameObject);
    }
}
