using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public void AttackAnimationEnd()
    {
        //This method is called in an animation event at the end of the attack animation.
        //It uses the EnemyMelee script on the visual object.
        //This means we need to get the melee script attached to the parented object "Zombie" and update its state from there.
        GameObject parentObject = transform.parent.gameObject;
        EnemyMelee parentScript = parentObject.GetComponent<EnemyMelee>();
        parentScript.SwitchStateTo(EnemyMelee.STATE.CHASE);
    }
    public void ThrowAnimationEnd()
    {
        //This method is called in an animation event at the end of the attack animation.
        //It uses the EnemyMelee script on the visual object.
        //This means we need to get the melee script attached to the parented object "Zombie" and update its state from there.
        GameObject parentObject = transform.parent.gameObject;
        EnemyRanged parentScript = parentObject.GetComponent<EnemyRanged>();
        parentScript.SwitchStateTo(EnemyRanged.STATE.CHASE);
    }
    public void DeathAnimationEnd()
    {
        //This method is called in an animation event at the end of the attack animation.
        //It uses the EnemyMelee script on the visual object and destroy's its parent object.
        Destroy(transform.parent.gameObject);
    }

    public void EnableDamageCollider()
    {
        GameObject parentObject = transform.parent.gameObject;
        EnemyMelee parentScript = parentObject.GetComponent<EnemyMelee>();
        parentScript.attackHitbox.EnableDamageCollider();

    }
    public void DisableDamageCollider()
    {
        GameObject parentObject = transform.parent.gameObject;
        EnemyMelee parentScript = parentObject.GetComponent<EnemyMelee>();
        parentScript.attackHitbox.DisableDamageCollider();

    }
    public void CreateProjectile()
    {
        //This method is called in an animation event at the end of the attack animation.
        //It uses the EnemyMelee script on the visual object.
        //This means we need to get the melee script attached to the parented object "Zombie" and update its state from there.
        GameObject parentObject = transform.parent.gameObject;
        EnemyRanged parentScript = parentObject.GetComponent<EnemyRanged>();
        Instantiate(parentScript.projectile, parentObject.transform);
    }
    public void PlayDeathSound()
    {
        return;
    }
}
