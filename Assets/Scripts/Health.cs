using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: This script is for managing Health and a simple placeholder for a white hit flash.
//I am aware there is a red code error saying : "UnassignedReferenceException: The variable body of Health has not been assigned"
//Ignore this, this is fine. It's happenening because the player only has one mesh renderer in it compared to the enemies which have two.
//Its just a warning that the variable "body" is not defined but it doesn't need to be with the way I've set up safeguarding in FlashStart() and FlashStop(). - Sarah.
public class Health : MonoBehaviour
{
    // Flash variables:
    public GameObject head;
    public GameObject body;
    private MeshRenderer meshRendererHead;
    private MeshRenderer meshRendererBody;
    public Color origColor;
    private float flashTime = 0.1f;

    // Health variables:
    public int currentHealth;
    public int maxHealth = 100;
    //=============================================Unity Built-in Methods===============================================
    // Set health to max health on awake.
    void Awake()
    {
        currentHealth = maxHealth;
        meshRendererHead = head.GetComponent<MeshRenderer>();
        meshRendererBody = body.GetComponent<MeshRenderer>();
    }
    //=============================================Methods to manage Health=====================================
    public void ApplyDamage(int damage)
    {
        GameObject go = this.gameObject;
        //Apply the damage
        currentHealth -= damage;
 
        //Add 1 to the combo if enemy is killed.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null){
            PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
            playerScript.combo += 1;
        }
        //Destroy enemy if health is less than 0
        if (currentHealth <= 0)
        {
            Destroy(go);
            return;
        }
        //Flash the object: White
        FlashStart();
    }
    //=============================================Methods to manage Hit Flash====================================
    public void FlashStart()
    {
        if (meshRendererHead != null){ meshRendererHead.material.color = Color.white; }
        if (meshRendererBody != null){ meshRendererBody.material.color = Color.white; }

        Invoke("FlashStop", flashTime);
            
    }
    public void FlashStop()
    {
        if (meshRendererHead != null) { meshRendererHead.material.color = origColor; }
        if (meshRendererBody != null) { meshRendererBody.material.color = origColor; }
    }
}
