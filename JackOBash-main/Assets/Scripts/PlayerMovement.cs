using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References:
    private CharacterController cc;
    private PlayerInput input;
    private Animator animator;
    private DamageCollision attackHitbox;

    // Movement:
    private Vector3 movementVelocity;
    private float verticalVelocity;
    private Vector3 oldMovementDirection;
    private Quaternion startOrientation;
    public float rotationTime = 0f;
    public float rotationDuration = 0.2f;
    private float gravity = -20f;
    [Header("Movement")]
    [Tooltip("Any changes to this setting will not be saved! This is displayed for visual purposes only. " +
        "This is the current speed at which the player will move based on the \nFormula: moveSpeedBase + (moveSpeedLevel * moveMultiplier).")]
    public float currentMoveSpeed = 7f;
    [Tooltip("This is the starting amount of movement speed. Formula: moveSpeedBase + (moveSpeedLevel * moveMultiplier)")]
    public float moveSpeedBase = 7f;
    [Tooltip("The amount to * the speed level by. \nFormula: moveSpeedBase + (moveSpeedLevel * moveMultiplier).")]
    public float moveMultiplier = 1;
    [Tooltip("Current movement speed upgraded level.")]
    public int moveSpeedLevel = 1;
    public float dashSpeed = 9f;

    // Sliding: This is for slight movement after attacking.
    // NOTE: This will make the attack animation look smoother once implemented.
    private float attackStartTime;
    [Tooltip("This is how long to move forward when attacking.")]
    public float attackSlideDuration = 0.2f;
    [Tooltip("This is the speed at which to move when attacking.")]
    public float attackSlideSpeed = 0.02f;

    // State variables/Enum
    enum STATE{
        FREE,
        ATTACKING,
        DASH,
        HIT,
        DEAD
    }
    private STATE currentState;
    [Tooltip("This is the current combo we have accumulated.")]
    public int combo = 0;
    //=============================================Unity Built-in Methods===============================================
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        attackHitbox = GetComponentInChildren<DamageCollision>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Update movement speed.
        // !!Needs to be changed to a one time thing when upgrades happen so its not updating every frame, only when the values are changed.
        currentMoveSpeed = moveSpeedBase + (moveSpeedLevel * moveMultiplier);
        Debug.Log(currentState);
        // Update Player based on state:
        switch (currentState){
            case STATE.FREE:
                CalculateMovement();
                break;
            case STATE.ATTACKING:
                // Set movement to 0. 
                //animator.SetFloat("Speed", 0f);
                //movementVelocity = Vector3.zero;
                // Slide the player slightly when attacking.
                CalculateMovement();
                if (Time.time < attackStartTime + attackSlideDuration)
                {
                    float timePassed = Time.time - attackStartTime;
                    float lerpTime = timePassed / attackSlideDuration;
                    movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                }
                break;
            case STATE.DASH:

                movementVelocity = transform.forward * dashSpeed * Time.deltaTime;
                animator.SetFloat("Speed", 0f);
                ChangeToAttackCheck();
                break;
        }

        // Apply Gravity:
        if (!cc.isGrounded) { verticalVelocity = gravity;} 
        else                { verticalVelocity = gravity * 0.3f;}

        // Apply movement:
        movementVelocity += verticalVelocity * Vector3.up * Time.deltaTime;
        cc.Move(movementVelocity);
    }
    //=============================================Calculate Movement===============================================
    private void CalculateMovement()
    {
        // Update states:
        // Change to attack state
        ChangeToAttackCheck();
        ChangeToDashCheck();
        // Calculate Movement based on Input.
        movementVelocity.Set(input.horizontalInput, 0f, input.verticalInput);
        movementVelocity.Normalize();

        // The game is rotated by -45 degrees to give an isometric view:
        // Update rotation of y-axis for movement to match input with the game view.
        movementVelocity = Quaternion.Euler(0, -45, 0) * movementVelocity;

        // Update movement to 'movementVelocity' variable which is used to apply movement.
        movementVelocity *= moveSpeedBase * Time.deltaTime;

        // Update player direction - smooth rotation:
        if (movementVelocity != Vector3.zero)
        {
            if (movementVelocity.normalized != oldMovementDirection)
            {
                startOrientation = transform.rotation;
                rotationTime = rotationDuration;
            }
            Quaternion endOrientation = Quaternion.LookRotation(movementVelocity);
            if (rotationTime < 0)
            {
                transform.rotation = endOrientation;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(endOrientation, startOrientation,
                    rotationTime / rotationDuration);
                rotationTime -= Time.deltaTime;
            }
            oldMovementDirection = movementVelocity.normalized;
        }
        animator.SetFloat("Speed", movementVelocity.magnitude);
    }
//=============================================DamageCollider/Apply Damage==========================================
    public void EnableDamageCollider()
    {
        attackHitbox.EnableDamageCollider();

    }
    public void DisableDamageCollider()
    {
        attackHitbox.DisableDamageCollider();

    }
    public void AttackAnimationEnd()
    {
        SwitchStateTo(STATE.FREE);
    }
    public void DashAnimationEnd()
    {
        if (currentState == STATE.DASH) //This is a safe guard since using the same animation for move and dash. Remove this and add animationevent to dash when fin. Remember to remove this method from walk animation event.
        {
            SwitchStateTo(STATE.FREE);
        }
    }
    //==============================================StateChanges===============================
    private void SwitchStateTo(STATE _newState)
    {
        // Update variables:
        input.ClearCache();

        // Exit current state
        switch (currentState)
        {
            case STATE.FREE:
                break;
            case STATE.ATTACKING:
                break;
            case STATE.DASH:
                break;
            case STATE.HIT:
                break;
            case STATE.DEAD:
                break;
        }
        // Enter new state
        switch (_newState)
        {
            case STATE.FREE:
                break;
            case STATE.ATTACKING:
                // Update Animator: Play animation for attacking.
                animator.SetTrigger("Attacking");
                // Stop Movement
               // movementVelocity = Vector3.zero;
                // Update Rotation to face the direction immediately
                transform.rotation = Quaternion.LookRotation(oldMovementDirection);
                // Get time from entering new state for attack sliding.
                attackStartTime = Time.time;
                break;
            case STATE.DASH:
                // Update Animator: Play animation for Dashing.
                animator.SetTrigger("Dashing");
                // Stop Movement
                movementVelocity = Vector3.zero;
                // Update Rotation to face the direction immediately
                transform.rotation = Quaternion.LookRotation(oldMovementDirection);
                break;
            case STATE.HIT:
                
                break;
            case STATE.DEAD:

                break;
        }
        currentState = _newState;
    }
    //=============================================Tempory State change checks===============================================
    private void ChangeToDashCheck()
    {
        if (input.dashButtonPressed)
        {
            SwitchStateTo(STATE.DASH);
            
            return;
        }
    }
    private void ChangeToAttackCheck()
    {
        if (input.attackButtonPressed)
        {
            SwitchStateTo(STATE.ATTACKING);
            return;
        }
    }
}