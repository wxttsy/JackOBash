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
    private float rotationTime = 0f;
    public float rotationDuration = 0.2f;
    public float gravity = -20f;
    [Tooltip("Any changes to this setting will not be saved! This is displayed for visual purposes only. " +
        "This is the current speed at which the player will move based on the \nFormula: moveSpeedBase + (moveSpeedLevel * moveMultiplier).")]
    public float currentMoveSpeed = 7f;
    [Tooltip("This is the starting amount of movement speed. Formula: moveSpeedBase + (moveSpeedLevel * moveMultiplier)")]
    public float moveSpeedBase = 7f;
    [Tooltip("The amount to * the speed level by. \nFormula: moveSpeedBase + (moveSpeedLevel * moveMultiplier).")]
    public float moveMultiplier = 1;
    [Tooltip("Current movement speed upgraded level.")]
    public int moveSpeedLevel = 1;

    public SpawnerPlayer sp;


    enum STATE
    {
        FREE,
        ATTACKING
    }
    private STATE currentState;
    // Start is called before the first frame update
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        attackHitbox = GetComponentInChildren<DamageCollision>();
        sp = GetComponent<SpawnerPlayer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Update movement speed.
        // !!Needs to be changed to a one time thing when upgrades happen so its not updating every frame, only when the values are changed.
        currentMoveSpeed = moveSpeedBase + (moveSpeedLevel * moveMultiplier);

        // Update Player based on state:
        switch (currentState){
            case STATE.FREE:
                CalculateMovement();

                // Change to attack state
                if (Input.GetMouseButtonDown(0)) 
                {
                    animator.SetTrigger("Attacking");
                    currentState = STATE.ATTACKING;
                    movementVelocity = Vector3.zero;
                    //transform.rotation = Quaternion.LookRotation(oldMovementDirection);
                    return;
                }
                break;
            case STATE.ATTACKING:
                break;
        }

        // Apply Gravity:
        if (!cc.isGrounded) { verticalVelocity = gravity;} 
        else                { verticalVelocity = gravity * 0.3f;}

        // Apply movement:
        movementVelocity += verticalVelocity * Vector3.up * Time.deltaTime;
        cc.Move(movementVelocity);
    }

    private void CalculateMovement()
    {
        // Calculate Movement based on Input.
        movementVelocity.Set(input.horizontalInput, 0f, input.verticalInput);
        movementVelocity.Normalize();

        // The game is rotated by -45 degrees to give an isometric view:
        // Update rotation of y-axis for movement to match input with the game view.
        movementVelocity = Quaternion.Euler(0, -45, 0) * movementVelocity;

        // Update movement to 'movementVelocity' variable which is used to apply movement.
        movementVelocity *= moveSpeedBase * Time.deltaTime;

        // Update player direction - smooth:
        //if (movementVelocity != Vector3.zero)
        //{
        //    if (movementVelocity.normalized != oldMovementDirection)
        //    {
        //        startOrientation = transform.rotation;
        //        rotationTime = rotationDuration;
        //    }
        //    Quaternion endOrientation = Quaternion.LookRotation(movementVelocity);
        //    if (rotationTime < 0)
        //    {
        //        transform.rotation = endOrientation;
        //    }
        //    else
        //    {
        //        transform.rotation = Quaternion.Slerp(endOrientation, startOrientation,
        //            rotationTime / rotationDuration);
        //        rotationTime -= Time.deltaTime;
        //    }
        //    oldMovementDirection = movementVelocity.normalized;
        //}


        transform.rotation = Quaternion.LookRotation(-sp.newDirection);

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
        currentState = STATE.FREE;
    }
}