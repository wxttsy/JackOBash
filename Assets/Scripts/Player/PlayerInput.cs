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


            controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();


            controls.Gameplay.Rotate.performed += ctx => rot = ctx.ReadValue<Vector2>();



        }
        //*******************************************************************************************************************
        //--------------------------------------------------Update-----------------------------------------------------------
        //*******************************************************************************************************************
        void Update()
        {

            dashButtonPressed = controls.Gameplay.Dash.triggered;
            attackButtonPressed = controls.Gameplay.Attack.triggered;
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
            controls.Gameplay.Enable();
        }

        void OnDisable()
        {
            controls.Gameplay.Disable();
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