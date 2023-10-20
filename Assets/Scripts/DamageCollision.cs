using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageCollision : MonoBehaviour
{
    // References:
    private Collider _damageCollider;
    // Variables:
    public int damage = 20;
    public string targetTag;
    [SerializeField] private List<Collider> damagedTargetList;

    public bool isDamageOrb;
    //=============================================Unity Built-in Methods===============================================
    private void Awake()
    {
        // Set up variables:
        _damageCollider = GetComponent<Collider>();
        // Disable collider at start.
        if (!isDamageOrb)
        {
            _damageCollider.enabled = false;
            Debug.Log("I am switched off");
        }

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
        _damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {// Disables the box collider attached to this object - This event is called through animation events.
        // Clear List
        damagedTargetList.Clear();
        _damageCollider.enabled = false;
    }
}
