using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Input variables:
    public float horizontalInput;
    public float verticalInput;
    public bool attackButtonPressed;
    public bool dashButtonPressed;
    public float horizontalRotation;
    public float verticalRotation;

    public GameObject itemInSlot;

    public void Start()
    {

    }

    // Update Input recieved:
    void Update()
    {
        dashButtonPressed = Input.GetButtonDown("Dash");
        attackButtonPressed = Input.GetButtonDown("Attack");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //horizontalRotation = Input.GetAxis("HorizontalRotation");
        //verticalRotation = Input.GetAxis("VerticalRotation");

        //DetermineFloor();
        ItemCheck();
    }

    private void OnDisable()
    {
        ClearCache();
    }

    public void ClearCache()
    {
        horizontalInput = 0;
        verticalInput = 0;
        attackButtonPressed = false;
        dashButtonPressed = false;
    }


    //check whether the item can be used when a key is pressed. spit out a debug if item is null.
    public void ItemCheck()
    {
        if (Input.GetKeyDown(KeyCode.Q) && itemInSlot != null)
        {
            UseItem();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && itemInSlot == null)
        {
            Debug.Log("I don't hava the item");
        }
    }

    public void UseItem()
    {
        //create the item in respect to player transform
            Instantiate(itemInSlot, this.transform);
            itemInSlot = null;
    }
}