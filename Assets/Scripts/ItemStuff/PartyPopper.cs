using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// Party popper is a passive ability that quickly appears and disapears, dealing damage to enemies. 
/// 
/// </summary>
public class PartyPopper : MonoBehaviour
{
    BoxCollider _damageCollider;
    public float timeToBeActive;
    public float timeActive;

    void Start()
    {
        transform.parent = FindObjectOfType<PlayerInput>().gameObject.transform;
        _damageCollider = GetComponentInChildren<BoxCollider>();

        timeActive = 0f;

        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {

        timeActive += Time.deltaTime;

        if(timeActive> timeToBeActive)
        {
            DestroyThis();
        }

    }

    /// <summary>
    /// 
    /// The DestroyThis() function is for the animation event when it
    /// comes time to implement in the final release, and the timer will be deleted at the same time.
    /// 
    /// </summary>

    public void DestroyThis()
    {
        FindObjectOfType<Player>().hasItem = false;
        Destroy(gameObject);
    }
}
