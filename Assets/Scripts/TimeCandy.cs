using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCandy : MonoBehaviour
{
    public int candyTimeAmount = 5;
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
                playerHealthScript.currentHealth += candyTimeAmount;
                Debug.Log("Picked up Time Candy");
                GameObject effectsManagerObject = GameObject.FindWithTag("EffectsManager");
                EffectsManager effectsManager = effectsManagerObject.GetComponent<EffectsManager>();
                Instantiate(effectsManager.candyPickUpParticle, transform.position, transform.rotation);
                //Destroy this candy object.
                Destroy(gameObject);
            }
        }
    }
}
