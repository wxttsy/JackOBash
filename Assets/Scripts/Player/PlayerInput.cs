using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //*******************************************************************************************************************
    //------------------------------------------Initialize Variables-----------------------------------------------------
    //*******************************************************************************************************************
    // Input variables:
    public float horizontalInput;
    public float verticalInput;
    public bool attackButtonPressed;
    public bool dashButtonPressed;

    public float horizontalRotation;
    public float verticalRotation;

    //*******************************************************************************************************************
    //--------------------------------------------------Update-----------------------------------------------------------
    //*******************************************************************************************************************
    void Update(){
        dashButtonPressed = Input.GetButtonDown("Dash");
        attackButtonPressed = Input.GetButtonDown("Attack");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //horizontalRotation = Input.GetAxis("HorizontalRotation");
        //verticalRotation = Input.GetAxis("VerticalRotation");
    }
    //*******************************************************************************************************************
    //-----------------------------------------------Cache Update--------------------------------------------------------
    //*******************************************************************************************************************
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
}