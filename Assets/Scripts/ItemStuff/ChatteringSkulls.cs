using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatteringSkulls : MonoBehaviour
{
    //*******************************************************************************************************************
    //------------------------------------------Initialize Variables-----------------------------------------------------
    //*******************************************************************************************************************
    private GameObject player;
    private float rotation;

    public float rotationSpeed = 10f;
    public float itemDurationTimer;
    public int itemDuration = 5;

    //*******************************************************************************************************************
    //---------------------------------------------------Start-----------------------------------------------------------
    //*******************************************************************************************************************
    private void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //*******************************************************************************************************************
    //---------------------------------------------------Update----------------------------------------------------------
    //*******************************************************************************************************************
    private void Update() {
        if (player != null){ //Check that player exists: If it does, update this item.
            // Rotate skulls
            rotation += rotationSpeed * Time.deltaTime;
            transform.position = player.transform.position;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
        // Update the time left on this item: Destroy it if item has expired.
        itemDurationTimer += Time.deltaTime;
        if (itemDurationTimer > itemDuration){ Destroy(this.gameObject); }
    }
}