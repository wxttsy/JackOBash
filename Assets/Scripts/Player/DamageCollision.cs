using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageCollision : MonoBehaviour
{
    //*******************************************************************************************************************
    //------------------------------------------Initialize Variables-----------------------------------------------------
    //*******************************************************************************************************************
    // References:
    private Collider _damageCollider;

    // Variables:
    [Tooltip("The amount of damage that will be applyed when the targetTag object has been hit.")]
    public int damage = 20;
    [Tooltip("The tag of the object this object can hit.")]
    public string targetTag;
    [Tooltip("Tick this box if the object is an Item or Candy.")]
    public bool isItem;

    private List<Collider> damagedTargetList;
    //*******************************************************************************************************************
    //-----------------------------------------------Awake/ Setup--------------------------------------------------------
    //*******************************************************************************************************************
    private void Awake(){
        // Assign references:
        _damageCollider = GetComponent<Collider>();

        // Disable collider at start if is not an Item:
        if (!isItem){
            _damageCollider.enabled = false;
        }
        damagedTargetList = new List<Collider>();
    }
    //*******************************************************************************************************************
    //---------------------------------------------On Trigger Enter------------------------------------------------------
    //*******************************************************************************************************************
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we are colliding with has the same tag as the TargetTag:
        if (other.tag == targetTag && !damagedTargetList.Contains(other))
        {
            Health healthScriptOfCollidingObject = other.GetComponent<Health>();
            if (healthScriptOfCollidingObject != null)
            {
                // Apply damage to the object we are colliding with.
                //Check if we are the player
                if (gameObject == GameObject.FindGameObjectWithTag("Player"))
                {
                    //Are we in sugar rush? Deal double damage if we are.
                    Player playerScript = gameObject.GetComponent<Player>();
                    if (playerScript.sugarRushIsActivated)
                    {
                        healthScriptOfCollidingObject.ApplyDamage(damage * 2);
                    }
                    else
                    {
                        healthScriptOfCollidingObject.ApplyDamage(damage);
                    }

                }
                else
                {
                    //Otherwise apply damage normally.
                    healthScriptOfCollidingObject.ApplyDamage(damage);
                }
            }
            damagedTargetList.Add(other);
        }
    }
    //*******************************************************************************************************************
    //----------------------------------------Animation Event Methods----------------------------------------------------
    //*******************************************************************************************************************
    public void EnableDamageCollider()
    {// Enables the box collider attached to this object - This event is called through animation events.
        // Clear List
        damagedTargetList.Clear();
        _damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {// Disables the box collider attached to this object - This event is called through animation events.
        // Clear List
        damagedTargetList.Clear();
        _damageCollider.enabled = false;
    }
}
