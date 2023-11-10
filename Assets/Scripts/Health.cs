using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    DeathScreen death;
    CandyManager candyManager;
    ItemSpawner itemManager;
    EffectsManager effectsManager;
    // Health variables:
    public float currentHealth;
    public float maxHealth = 100;
    public int scoreFromKill = 1;
    private int pumpkinCrawlerCandyDropAmount = 10;
    //=============================================Unity Built-in Methods===============================================
    // Set health to max health on awake.
    void Awake()
    {
        //Get references to candy manager and effects manager
        //-Candy for dropping if an enemy is killed, effects for applying a hit effect in apply damage.
        currentHealth = maxHealth;
        GameObject candyManagerObject = GameObject.FindWithTag("CandyManager");
        candyManager = candyManagerObject.GetComponent<CandyManager>();

        //Find the item manager for spawning items on certain combo levels
        GameObject itemManagerObject = GameObject.FindWithTag("ItemManager");
        itemManager = itemManagerObject.GetComponent<ItemSpawner>();

        GameObject effectsManagerObject = GameObject.FindWithTag("EffectsManager");
        effectsManager = effectsManagerObject.GetComponent<EffectsManager>();
    }
    //=============================================Methods to manage Health=====================================
    public void ApplyDamage(int damage)
    {
        //Get current game object this health script is attached to
        GameObject go = this.gameObject;
        ApplyHitEffect();
        //Apply the damage
        if (currentHealth > 0){

            currentHealth -= damage;
        }
        
        //Add 1 to the combo if enemy is killed.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != go){ //We are an enemy
            //Destroy enemy if health is less than 0
            if (currentHealth <= 0)
            {
                //Add combo
                UpdateCombo(player);

                //Melee enemy death
                EnemyMelee meleeEnemyScript = go.GetComponent<EnemyMelee>();
                if (meleeEnemyScript != null)
                {
                    CharacterController cc = go.GetComponent<CharacterController>();
                    Destroy(cc);
                    meleeEnemyScript.SwitchStateTo(EnemyMelee.STATE.DEAD);
                    return;
                }
                //Pumpkin Crawler enemy death
                PumpkinCrawler pumpkinCrawlerScript = go.GetComponent<PumpkinCrawler>();
                if (pumpkinCrawlerScript != null)
                {
                    Destroy(go);
                    for (int i = 0; i < pumpkinCrawlerCandyDropAmount; i++)
                    {

                        Vector3 scatter = new Vector3(Random.Range(-5, 5),0, Random.Range(-5, 5));
                        Instantiate(candyManager.timeCandy, transform.position + scatter, transform.rotation);
                    }
                    return;
                }

                //Ranged enemy death: Not implemented yet. Awaiting models and animations. - Sarah
                /*EnemyRanged rangedEnemyScript = go.GetComponent<EnemyRanged>();
                if (rangedEnemyScript != null)
                {
                    rangedEnemyScript.SwitchStateTo(EnemyRanged.STATE.DEAD);
                    return;
                }*/
                return;
            }
        }else{
            //We are the player
            if (currentHealth <= 0)
            {
                PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
                playerScript.SwitchStateTo(PlayerMovement.STATE.DEAD);
            }
        }
    }
    void UpdateCombo(GameObject player)
    {
        //Get PlayerMovement script attached to the player.
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        //Increase combo by 1.
        playerScript.combo += 1;
        //Add score bonus from this enemy to player's total score.
        playerScript.playerScore += scoreFromKill;

        //Check if current combo is mod by 5.(Basically if its a multiple of 5, it will drop a candy.
        //comboCandyCheck will = 0 if this is true.
        float combo = playerScript.combo;
        float comboCandyCheck = combo % 5;
        float comboItemCheck = combo % 2;
        //Debug.Log(comboCandyCheck);
        if (comboCandyCheck == 0 && comboItemCheck != 0)
        {
            //Drop Candy
            Instantiate(candyManager.timeCandy,transform.position,transform.rotation);
        }

        if (comboCandyCheck == 0 && comboItemCheck == 0)
        {
            //Drop item
            Instantiate(itemManager.spawnedItem[Random.Range(0, itemManager.spawnedItem.Length-1)], transform.position, transform.rotation);
        }
    }

    void ApplyHitEffect()
    {
        //Create a the hit effect particle object prefab that is being held in the effects manager.
        GameObject effect = Instantiate(effectsManager.hitParticle, transform.position, transform.rotation);
        //Below is making sure to delete the effect after its been displayed. But only AFTER its done it's effect, 0.5f seems to work well. - Sarah.
        Destroy(effect, 0.5f);
        
    }
}
