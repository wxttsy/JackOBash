using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCandy : MonoBehaviour
{
    public int candyTimeAmount = 1;
    public int candyBarAmount = 8;
    private void OnTriggerEnter(Collider other)
    {
        // If what we have collided with, is the player:
        if (other.tag == "Player")
        {
            // Get player's health component
            Health playerHealthScript = other.GetComponent<Health>();
            if (playerHealthScript != null)
            {
                // Add candy amount to player's health.
                if (playerHealthScript.currentHealth > 100)
                {
                    playerHealthScript.currentHealth = 100;
                }
                else
                {
                    playerHealthScript.currentHealth += candyTimeAmount;
                    Debug.Log("add time");
                }
                
                Debug.Log("Picked up Time Candy");
                GameObject effectsManagerObject = GameObject.FindWithTag("EffectsManager");
                EffectsManager effectsManager = effectsManagerObject.GetComponent<EffectsManager>();
                Instantiate(effectsManager.candyPickUpParticle, transform.position, transform.rotation);

                //Add candy to candy bar
                GameObject timerGo = GameObject.FindWithTag("Timer");
                Timer timer = timerGo.GetComponent<Timer>();
                timer.candySlider.value += candyBarAmount;

                //Destroy this candy object.
                Destroy(gameObject);
            }
        }
    }
}
