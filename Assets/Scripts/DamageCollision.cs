using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageCollision : MonoBehaviour
{
    // References:
    private Collider damageCollider;
    // Variables:
    [Tooltip("The amount of damage that will be applyed when the targetTag object has been hit.")]
    public int damage = 20;
    [Tooltip("The tag of the object this object can hit.")]
    public string targetTag;
    private List<Collider> damagedTargetList;
    //=============================================Unity Built-in Methods===============================================
    private void Awake()
    {
        // Set up variables:
        damageCollider = GetComponent<Collider>();
        // Disable collider at start.
        damageCollider.enabled = false;
        damagedTargetList = new List<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // If thing we are colliding with is the same as the TargetTag we set in editor:
        if (other.tag == targetTag && !damagedTargetList.Contains(other))
        {
            Health targetCC = other.GetComponent<Health>();
            if (targetCC != null)
            {
                // Apply damage to the thing we are colliding with.
                targetCC.ApplyDamage(damage);
            }
            damagedTargetList.Add(other);
        }
    }
    //=============================================Other methods===============================================
    public void EnableDamageCollider()
    {// Enables the box collider attached to this object - This event is called through animation events.
        // Clear List
        damagedTargetList.Clear();
        damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {// Disables the box collider attached to this object - This event is called through animation events.
        // Clear List
        damagedTargetList.Clear();
        damageCollider.enabled = false;
    }
}
