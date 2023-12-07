using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// An item that replenishes some health and contributes to sugar rush. 
/// 
/// </summary>
public class HealthCandy : MonoBehaviour
{
    //*******************************************************************************************************************
    //------------------------------------------Initialize Variables-----------------------------------------------------
    //*******************************************************************************************************************
    public int candyHealthAmount = 1;
    public int candyBarAmount = 8;
    //*******************************************************************************************************************
    //--------------------------------------------On Trigger Enter-------------------------------------------------------
    //*******************************************************************************************************************
    private void OnTriggerEnter(Collider other)
    {
        // If the object we have collided with is the player:
        if (other.tag == "Player"){
            // Get player's health component
            Health playerHealthScript = other.GetComponent<Health>();
            if (playerHealthScript != null) {
                // Add candy amount to player's health.
                if (playerHealthScript.currentHealth > playerHealthScript.maxHealth)
                {
                    playerHealthScript.currentHealth = playerHealthScript.maxHealth;
                }
                else
                {
                    if (!other.GetComponent<Player>().sugarRushIsActivated)
                    {
                        playerHealthScript.currentHealth += candyHealthAmount;
                    }
                    else
                    {
                        playerHealthScript.currentHealth += candyHealthAmount / 2;
                    }


                    Debug.Log("add time");
                }
                //Play Sound
                GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>(); 
                audioManager.PlayAudio(audioManager.sfCandyPickup);

                //Play Effect
                Debug.Log("Picked up Time Candy");
                GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
                GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
                Instantiate(gameManager.candyPickUpParticle, transform.position, transform.rotation);

                //Add candy to candy bar
                GameObject barDisplayUIObject = GameObject.FindWithTag("BarDisplayUI");
                BarDisplayUI barDisplayUIScript = barDisplayUIObject.GetComponent<BarDisplayUI>();
                barDisplayUIScript.sugarRushSlider.value += candyBarAmount;

                //Destroy this candy object.
                Destroy(gameObject);
            }
        }
    }
}
