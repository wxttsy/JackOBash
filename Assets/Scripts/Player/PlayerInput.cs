using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

        //controls.Gameplay.Attack.performed += ctx => Attack(ctx);
        //controls.Gameplay.Attack.canceled += ctx => AttackReset(ctx);

        //controls.Gameplay.Dash.performed += ctx => Dash(ctx);
        //
        //
        //controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        //
        //controls.Gameplay.Rotate.performed += ctx => rot = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Rotate.canceled += ctx => rot = Vector2.zero;


    }
    //*******************************************************************************************************************
    //--------------------------------------------------Update-----------------------------------------------------------
    //*******************************************************************************************************************
    void Update()
    {

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
    public void Attack(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);


        if (context.started)
        {
            attackIsTrue = true;
        }
        if (context.performed && !context.started)
        {
            attackIsTrue = false;
        }
        if (context.canceled)
        {
            attackIsTrue = false;
        }

    }



    public void Dash(InputAction.CallbackContext context)
    {

        {
            dashIsTrue = false;
        }
        if (context.started)
        {
            dashIsTrue = true;
        }
        if (context.performed && !context.started)
        {
            dashIsTrue = false;
        }
        if (context.canceled)
        {
            dashIsTrue = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            move = context.ReadValue<Vector2>();
        }


    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rot = context.ReadValue<Vector2>();
        }


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