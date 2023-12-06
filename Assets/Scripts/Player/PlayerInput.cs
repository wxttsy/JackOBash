using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

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
    public bool pauseButtonPressed;

    public float horizontalRotation;
    public float verticalRotation;

    public PlayerControls controls;

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
        if (controls == null) controls = new PlayerControls();
        controls.Player.Enable();
    }
    //*******************************************************************************************************************
    //--------------------------------------------------Update-----------------------------------------------------------
    //*******************************************************************************************************************
    void Update()
    {
        move = controls.Player.Move.ReadValue<Vector2>();
        rot = controls.Player.Rotate.ReadValue<Vector2>();

        pauseButtonPressed = controls.Player.Pause.triggered;
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
    

    public void OnEnable()
    {
        controls.Player.Enable();
    }

    public void OnDisable()
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
        pauseButtonPressed = false;
    }
}