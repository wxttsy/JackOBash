using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        // If what we have collided with, is the player:
        if (other.tag == "Player")
        {
            // Add candy amount to player's health.
            Debug.Log("Picked up Item");
            GameObject effectsManagerObject = GameObject.FindWithTag("EffectsManager");
            EffectsManager effectsManager = effectsManagerObject.GetComponent<EffectsManager>();
            Instantiate(effectsManager.candyPickUpParticle, transform.position, transform.rotation);
            //Destroy this candy object.
            Destroy(gameObject);
        }
    }
}
