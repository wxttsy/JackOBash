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
    void Awake()
    {
        // Set health to maxHealth on start.
        currentHealth = maxHealth;

        // Initialize references:
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();

    }
    //*******************************************************************************************************************
    //------------------------------------------------Apply Damage-------------------------------------------------------
    //*******************************************************************************************************************
    public void ApplyDamage(int damage)
    {
        // Get current game object this health script is attached to.
        GameObject go = this.gameObject;
        ApplyHitEffect();
        //Apply the damage
        currentHealth -= damage;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != go)
        {
            // We are an enemy.
            // Destroy enemy if health is less than 0:
            if (currentHealth <= 0)
            {
                //Add killCounter (For item drop management).
                UpdateThisKill(player);

                // We are a Melee enemy and we died from this hit.
                EnemyMelee meleeEnemyScript = go.GetComponent<EnemyMelee>();
                if (meleeEnemyScript != null)
                {
                    //Get random number for death type
                    int rand;
                    rand = Random.Range(0, 2);
                    Debug.Log(rand);
                    //MeleeDeath 1 sound
                    if (rand == 0)
                    {
                        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                        audioManager.PlayAudio(audioManager.sfMeleeDeath1);
                    }
                    //MeleeDeath 2 sound
                    else if (rand == 1)
                    {
                        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                        audioManager.PlayAudio(audioManager.sfMeleeDeath2);
                    }
                    CharacterController cc = go.GetComponent<CharacterController>();
                    Destroy(cc);
                    meleeEnemyScript.SwitchStateTo(EnemyMelee.STATE.DEAD);
                    return;
                }
                //We are a Pumpkin Crawler and we died from this hit.
                PumpkinCrawler pumpkinCrawlerScript = go.GetComponent<PumpkinCrawler>();
                if (pumpkinCrawlerScript != null)
                {
                    Destroy(go);
                    //PumpkinCrawlerDeath Sound
                    GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                    AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                    audioManager.PlayAudio(audioManager.sfPumpkinCrawlerDeath);

                    // Drop multiple Time Candies:
                    for (int i = 0; i < pumpkinCrawlerCandyDropAmount; i++)
                    {

                        Vector3 scatter = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                        Instantiate(gameManager.healthCandy, transform.position + scatter, transform.rotation);
                    }
                    return;
                }
                //We are a RANGED enemy and died
                EnemyRanged rangedEnemyScript = go.GetComponent<EnemyRanged>();
                if (rangedEnemyScript != null)
                {
                    //Get random number for death type
                    int rand;
                    rand = Random.Range(0, 2);
                    Debug.Log(rand);
                    //MeleeDeath 1 sound
                    //if (rand == 0)
                    //{
                    //    GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                    //    AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                    //    audioManager.PlayAudio(audioManager.sfMeleeDeath1);
                    //}
                    ////MeleeDeath 2 sound
                    //else if (rand == 1)
                    //{
                    //    GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                    //    AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                    //    audioManager.PlayAudio(audioManager.sfMeleeDeath2);
                    //}
                    CharacterController cc = go.GetComponent<CharacterController>();
                    Destroy(cc);
                    Debug.Log("DESTROYED");
                    rangedEnemyScript.SwitchStateTo(EnemyRanged.STATE.DEAD);
                    return;
                }

                //We are an enemy and health is above 0 after hit
                if (currentHealth > 0)
                {
                    //Play BatHit sound (As if enemies are hit by the bat)
                    GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                    AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                    audioManager.PlayAudio(audioManager.sfBatHit);
                }
            }
            

        }
        

        if(go == player)
        {
                Debug.Log("I am in ELSE");
                //We are the player and we died from this hit.
                if (currentHealth <= 0)
                {
                    Player playerScript = player.GetComponent<Player>();
                    //if (playerScript.currentState != Player.STATE.DEAD)
                    //
                    Debug.Log("I am in IF");
                        playerScript.SwitchStateTo(Player.STATE.DEAD);
                    //}
                }
                //We are the player and we didnt die from this hit
                else if (currentHealth > 0)
                {
                    //Play PlayerHurt sound
                    GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                    AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                    audioManager.PlayAudio(audioManager.sfPlayerHurt1);
                }
        }
        //*******************************************************************************************************************
        //-----------------------------------------------UpdateThisKill------------------------------------------------------
        //*******************************************************************************************************************
        void UpdateThisKill(GameObject player)
        {
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
            if (candyDropCheck == 0 && itemDropCheck != 0)
            {

                Instantiate(gameManager.healthCandy, transform.position, transform.rotation);
            }
            // Enemy drops an Item:
            if (candyDropCheck == 0 && itemDropCheck == 0)
            {
                GameObject itemDrop = gameManager.items[Random.Range(0, gameManager.items.Length)];
                //Play PowerPickUp sound
                GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                audioManager.PlayAudio(audioManager.sfPowerPickup);

                GameObject go = Instantiate(itemDrop, player.transform);
            }
        }
        //*******************************************************************************************************************
        //------------------------------------------Apply Hit Particle Effect------------------------------------------------
        //*******************************************************************************************************************
        void ApplyHitEffect()
        {
            //Create a the hit effect particle object prefab that is being held in the effects manager.
            GameObject effect = Instantiate(gameManager.hitParticle, transform.position, transform.rotation);

        }
    }
}
