using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Health variables:
    public float currentHealth;
    public float maxHealth = 100;
    public GameObject partSys;
    //=============================================Unity Built-in Methods===============================================
    // Set health to max health on awake.
    void Awake()
    {
        currentHealth = maxHealth;
    }
    //=============================================Methods to manage Health=====================================
    public void ApplyDamage(int damage)
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject go = this.gameObject;
        //Apply the damage
        GameObject ps = Instantiate(partSys, this.gameObject.transform);
        ps.transform.parent = null;


        currentHealth -= damage;
        
        //Add 1 to the combo if enemy is killed.

        if (player != go){
            //Destroy enemy if health is less than 0
            if (currentHealth <= 0)
            {
                PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
                playerScript.combo += 1;
                playerScript.doComboMeterIncrease = true;

                if(playerScript.combo % 5 == 0 && playerScript.combo % 2 == 0)
                {
                    ItemSpawner itemSpawner = FindObjectOfType<ItemSpawner>();
                    itemSpawner.spawnTransform = transform;
                    itemSpawner.SpawnItem();

                }

                if (playerScript.combo % 5 == 0 && playerScript.combo % 2 != 0)
                {
                    ItemSpawner itemSpawner = FindObjectOfType<ItemSpawner>();
                    itemSpawner.spawnTransform = transform;
                    itemSpawner.SpawnCandy();
                }

                Destroy(go);
                return;
            }
        }
    }


}
