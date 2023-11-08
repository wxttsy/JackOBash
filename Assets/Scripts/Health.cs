using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Health variables:
    public float currentHealth;
    public float maxHealth = 100;
    //=============================================Unity Built-in Methods===============================================
    // Set health to max health on awake.
    void Awake()
    {
        currentHealth = maxHealth;
    }
    //=============================================Methods to manage Health=====================================
    public void ApplyDamage(int damage)
    {
        GameObject go = this.gameObject;
        //Apply the damage
        currentHealth -= damage;
        
        //Add 1 to the combo if enemy is killed.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != go){
            //Destroy enemy if health is less than 0
            if (currentHealth <= 0)
            {
                PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
                playerScript.combo += 1;
                playerScript.doComboMeterIncrease = true;
                Destroy(go);
                return;
            }
        }
    }
}
