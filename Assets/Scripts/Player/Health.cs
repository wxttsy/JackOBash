using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class responsible for managing health. 
/// </summary>
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

    //Drop Variables (Intervals for Modulus)
    [SerializeField] private int candyInt;
    [SerializeField] private int itemInt;


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

    /// <summary>
    /// Applies specified damage amount to this game object. 
    /// </summary>
    /// <param name="damage">The amount of damage to apply. Type: Health.int </param>
    public void ApplyDamage(int damage){
        // Get current game object this health script is attached to.
        GameObject go = this.gameObject;


        ApplyHitEffect();
        //Apply the damage
        currentHealth -= damage;

        //We are an enemy and health is above 0 after hit
        if (currentHealth > 0)
        {
            //Play BatHit sound (As if enemies are hit by the bat)
            GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
            AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
            audioManager.PlayAudio(audioManager.sfBatHit);
        }

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
                if (pumpkinCrawlerScript != null && !pumpkinCrawlerScript.isDead)
                {
                    pumpkinCrawlerScript.isDead = true;
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
                // We are a Ranged enemy and we died from this hit.
                EnemyRanged rangedEnemyScript = go.GetComponent<EnemyRanged>();
                if (rangedEnemyScript != null)
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
                    rangedEnemyScript.SwitchStateTo(EnemyRanged.STATE.DEAD);
                    return;
                }
            }

        }
        else
        {

            //We are the player and we died from this hit.
            if (currentHealth <= 0)
            {
                Player playerScript = player.GetComponent<Player>();
                if (playerScript.currentState != Player.STATE.DEAD)
                {
                    playerScript.SwitchStateTo(Player.STATE.DEAD);
                }
            }
            //We are the player and we didnt die from this hit
            else if (currentHealth > 0 )
            {
                Player playerScript = player.GetComponent<Player>();
                if (!playerScript.sugarRushIsActivated)
                {
                    ApplyHitEffect();
                    //Apply the damage
                    currentHealth -= damage;
                }
                

                //Play PlayerHurt sound
                GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                audioManager.PlayAudio(audioManager.sfPlayerHurt1);
            }

        }
    }
    //*******************************************************************************************************************
    //-----------------------------------------------UpdateThisKill------------------------------------------------------
    //*******************************************************************************************************************

    /// <summary>
    /// Updates player score values and rewards after kills. 
    /// </summary>
    /// <param name="player"> The player to update the kills for. Type: Health.GameObject </param>
    void UpdateThisKill(GameObject player){
        // Get Player script attached to the player.
        Player playerScript = player.GetComponent<Player>();
        Health pHP = player.GetComponent<Health>();
        candyInt = pHP.candyInt;
        itemInt = pHP.itemInt;
        // Add score bonus from this enemy to player's total score & increase the kill counter associated with the player.
        playerScript.playerScore += scoreFromKill;
        playerScript.killCounter += 1;

        // Check if this enemy should drop: Item/Candy/Nothing.
        float killCounter = playerScript.killCounter;
        float candyDropCheck = killCounter % candyInt; //candyDropCheck will == 0 if true.
        float itemDropCheck = killCounter % itemInt; //itemDropCheck will == 0 if true.

        // Enemy drops a Candy:
        if (candyDropCheck == 0 && itemDropCheck != 0) {

            Instantiate(gameManager.healthCandy,transform.position,transform.rotation);
        }
        // Enemy drops an Item:
        if (itemDropCheck == 0) {
            //Check if we are dropping a sugar rush item.
            int item = Random.Range(0, gameManager.items.Length);
            GameObject itemDrop = null;

            if (!playerScript.sugarRushIsActivated)
            {

                if (!playerScript.hasItem)
                {
                    //Get normal item
                    itemDrop = gameManager.items[item];
                    playerScript.hasItem = true;
                }

            }
            else
            {

                if (!playerScript.hasItem)
                {
                    //Get sugar rush item
                    itemDrop = gameManager.itemsSR[item];
                    playerScript.hasItem = true;
                }

            }
            


            if(itemDrop != null)
            {
                //Play PowerPickUp sound
                GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                audioManager.PlayAudio(audioManager.sfPowerPickup);
                GameObject go = Instantiate(itemDrop, player.transform);
            }

        }
    }
    //*******************************************************************************************************************
    //------------------------------------------Apply Hit Particle Effect------------------------------------------------
    //*******************************************************************************************************************


    /// <summary>
    /// Spawns a hit effect upon damage. 
    /// </summary>
    void ApplyHitEffect(){
        //Create a the hit effect particle object prefab that is being held in the effects manager.
        GameObject effect = Instantiate(gameManager.hitParticle, transform.position, transform.rotation);
        
    }
}
