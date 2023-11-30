using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.InputSystem;

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

    PlayerControls controls;

    Vector2 move;

    Vector2 rot;

    public bool attackIsTrue;

    bool dashIsTrue;

    //*******************************************************************************************************************
    //-------------------------------------------------Awake-------------------------------------------------------------
    //*******************************************************************************************************************
    private void Awake()
    {

        //stuff that performs input actions
        controls = new PlayerControls();

      
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();

        
        controls.Player.Rotate.performed += ctx => rot = ctx.ReadValue<Vector2>();



    }
    //*******************************************************************************************************************
    //--------------------------------------------------Update-----------------------------------------------------------
    //*******************************************************************************************************************
    void Update()
    {

        dashButtonPressed = controls.Player.Dash.triggered;
        attackButtonPressed = controls.Player.Attack.triggered;
        horizontalInput = move.x;
        verticalInput = move.y;
        horizontalRotation = rot.x;
        verticalRotation = rot.y;
    }
    //*******************************************************************************************************************
    //-----------------------------------------------Things--------------------------------------------------------------
    //*******************************************************************************************************************
    

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
        ClearCache();
    }


    //*******************************************************************************************************************
    //-----------------------------------------------Cache Update--------------------------------------------------------
    //*******************************************************************************************************************
   

    public void ClearCache()
    {
        horizontalInput = 0;
        verticalInput = 0;
        attackButtonPressed = false;
        dashButtonPressed = false;
    }
}