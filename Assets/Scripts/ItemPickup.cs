using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public GameObject item;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //get and update reference to current item in player slot on player input script
            other.GetComponent<PlayerInput>().itemInSlot = item;

            //play particle effect before death
            GameObject effectsManagerObject = GameObject.FindWithTag("EffectsManager");
            EffectsManager effectsManager = effectsManagerObject.GetComponent<EffectsManager>();
            Instantiate(effectsManager.candyPickUpParticle, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
