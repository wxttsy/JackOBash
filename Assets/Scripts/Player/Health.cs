using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //*******************************************************************************************************************
    //-------------------------------------------Initialize Variables----------------------------------------------------
    //*******************************************************************************************************************
    // References:
    GameManager gameManager;

    // Health variables:
    public float currentHealth;
    public float maxHealth = 100;
    public int scoreFromKill = 1;
    private int pumpkinCrawlerCandyDropAmount = 10;
    //*******************************************************************************************************************
    //---------------------------------------------------Awake-----------------------------------------------------------
    //*******************************************************************************************************************
    void Awake(){
        // Set health to maxHealth on start.
        currentHealth = maxHealth;

        // Initialize references:
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();

    }
    //*******************************************************************************************************************
    //------------------------------------------------Apply Damage-------------------------------------------------------
    //*******************************************************************************************************************
    public void ApplyDamage(int damage){
        // Get current game object this health script is attached to.
        GameObject go = this.gameObject;
        ApplyHitEffect();
        //Apply the damage
        currentHealth -= damage;
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != go){ 
            // We are an enemy.
            // Destroy enemy if health is less than 0:
            if (currentHealth <= 0) {
                //Add killCounter (For item drop management).
                UpdateThisKill(player);

                // We are a Melee enemy and we died from this hit.
                EnemyMelee meleeEnemyScript = go.GetComponent<EnemyMelee>();
                if (meleeEnemyScript != null){
                    CharacterController cc = go.GetComponent<CharacterController>();
                    Destroy(cc);
                    meleeEnemyScript.SwitchStateTo(EnemyMelee.STATE.DEAD);
                    return;
                }
                //We are a Pumpkin Crawler and we died from this hit.
                PumpkinCrawler pumpkinCrawlerScript = go.GetComponent<PumpkinCrawler>();
                if (pumpkinCrawlerScript != null) {
                    Destroy(go);
                    // Drop multiple Time Candies:
                    for (int i = 0; i < pumpkinCrawlerCandyDropAmount; i++) {

                        Vector3 scatter = new Vector3(Random.Range(-5, 5),0, Random.Range(-5, 5));
                        Instantiate(gameManager.healthCandy, transform.position + scatter, transform.rotation);
                    }
                    return;
                }
            }
        }else{
            // We are the player and we died from this hit.
            if (currentHealth <= 0){
                Player playerScript = player.GetComponent<Player>();
                playerScript.SwitchStateTo(Player.STATE.DEAD);
            }
        }
    }
    //*******************************************************************************************************************
    //-----------------------------------------------UpdateThisKill------------------------------------------------------
    //*******************************************************************************************************************
    void UpdateThisKill(GameObject player){
        // Get Player script attached to the player.
        Player playerScript = player.GetComponent<Player>();
        
        // Add score bonus from this enemy to player's total score & increase the kill counter associated with the player.
        playerScript.playerScore += scoreFromKill;
        playerScript.killCounter += 1;

        // Check if this enemy should drop: Item/Candy/Nothing.
        float killCounter = playerScript.killCounter;
        float candyDropCheck = killCounter % 5; //candyDropCheck will == 0 if true.
        float itemDropCheck = killCounter % 2; //itemDropCheck will == 0 if true.

        // Enemy drops a Candy:
        if (candyDropCheck == 0 && itemDropCheck != 0) {
            Instantiate(gameManager.healthCandy,transform.position,transform.rotation);
        }
        // Enemy drops an Item:
        if (candyDropCheck == 0 && itemDropCheck == 0) {
            GameObject itemDrop = gameManager.items[Random.Range(0, gameManager.items.Length)];
            GameObject go = Instantiate(itemDrop, transform.position, transform.rotation);
        }
    }
    //*******************************************************************************************************************
    //------------------------------------------Apply Hit Particle Effect------------------------------------------------
    //*******************************************************************************************************************
    void ApplyHitEffect(){
        //Create a the hit effect particle object prefab that is being held in the effects manager.
        GameObject effect = Instantiate(gameManager.hitParticle, transform.position, transform.rotation);
        
    }
}