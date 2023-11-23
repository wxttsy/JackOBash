using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    PlayerControls1 controls;

    Vector2 move;

    Vector2 rot;

    Boolean attackIsTrue;

    Boolean dashIsTrue;

    //*******************************************************************************************************************
    //-------------------------------------------------Awake-------------------------------------------------------------
    //*******************************************************************************************************************
    private void Awake()
    {
        
        //stuff that performs input actions
        controls = new PlayerControls1();

        controls.Gameplay.Attack.performed += ctx => Attack(ctx);
        controls.Gameplay.Attack.canceled += ctx => AttackReset(ctx);

        controls.Gameplay.Dash.performed += ctx => Dash();
        controls.Gameplay.Dash.canceled += ctx => DashReset();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rot = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rot = Vector2.zero;


    }
    //*******************************************************************************************************************
    //--------------------------------------------------Update-----------------------------------------------------------
    //*******************************************************************************************************************
    void Update(){

        dashButtonPressed = dashIsTrue;
        attackButtonPressed = attackIsTrue;
        horizontalInput = move.x;
        verticalInput = move.y;
        horizontalRotation = rot.x;
        verticalRotation = rot.y;
    }
    //*******************************************************************************************************************
    //-----------------------------------------------Things--------------------------------------------------------------
    //*******************************************************************************************************************
    void Attack(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        attackIsTrue = true;
    }

    void AttackReset(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        attackIsTrue = false;
    }

    void Dash()
    {
        dashIsTrue = true;
    }

    void DashReset()
    {
        dashIsTrue = false;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }


    //*******************************************************************************************************************
    //-----------------------------------------------Cache Update--------------------------------------------------------
    //*******************************************************************************************************************
    //private void OnDisable()
    //{
    //    ClearCache();
    //}

    public void ClearCache()
    {
        horizontalInput = 0;
        verticalInput = 0;
        attackButtonPressed = false;
        dashButtonPressed = false;
    }
}