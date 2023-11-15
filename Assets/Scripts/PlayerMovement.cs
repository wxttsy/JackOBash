using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // References:
    private CharacterController cc;
    private PlayerInput input;
    private Animator animator;
    private DamageCollision attackHitbox;
    private Combo comboScript;
    public ParticleSystem srParticles;

    // Movement:
    private Vector3 movementVelocity;
    private float verticalVelocity;
    private Vector3 oldMovementDirection;
    private Quaternion startOrientation;
    private float rotationTime = 0f;
    private float rotationDuration = 0.05f;
    private float gravity = -20f;
    public float sugarRushValue;
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

    //SUGAR RUSH
    public float moveSpeedSR = 10f;
    public bool sugarRushIsActivated = false;
    public float SRTimer = 0;
    public float sugarRushDuration = 26f;
    public Timer hp;

    // Sliding: This is for slight movement after attacking.
    // NOTE: This will make the attack animation look smoother once implemented.
    private float attackStartTime;
    [Tooltip("This is how long to move forward when attacking.")]
    public float attackSlideDuration = 0.2f;
    [Tooltip("This is the speed at which to move when attacking.")]
    public float attackSlideSpeed = 0.02f;
    Vector3 rot;
    // State variables/Enum
    public enum STATE
    {
        FREE,
        ATTACKING,
        DASH,
        HIT,
        DEAD,
        SUGAR_RUSH,
        SUGAR_RUSH_ATTACK,
        SUGAR_RUSH_DASH
    }
    private STATE currentState;
    [Tooltip("This is the current combo we have accumulated.")]
    public int combo = 0;

    public int playerScore;
    public GameObject deadUI;
    //=============================================Unity Built-in Methods===============================================
    private void Start()
    {
        deadUI.SetActive(false);

    }
    private void Awake()
    {
        deadUI.SetActive(false);
        cc = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        attackHitbox = GetComponentInChildren<DamageCollision>();
        comboScript = GetComponent<Combo>();
        currentMoveSpeed = moveSpeedBase + (moveSpeedLevel * moveMultiplier);
        hp = GetComponent<Timer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Update Player based on state:
        switch (currentState)
        {
            case STATE.FREE:
                CalculateMovement();
                ChangeToSugarRushCheck();

                break;
            case STATE.ATTACKING:
                // Set movement to 0. 
                animator.SetFloat("Speed", 0f);
                movementVelocity = Vector3.zero;
                // Slide the player slightly when attacking.
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

            case STATE.SUGAR_RUSH:
                CalculateMovement();

                break;

            case STATE.SUGAR_RUSH_DASH:

                movementVelocity = transform.forward * dashSpeed * Time.deltaTime;
                animator.SetFloat("Speed", 0f);
                ChangeToAttackCheck();
                break;

            case STATE.SUGAR_RUSH_ATTACK:
                // Set movement to 0. 
                animator.SetFloat("Speed", 0f);
                movementVelocity = Vector3.zero;
                // Slide the player slightly when attacking.
                if (Time.time < attackStartTime + attackSlideDuration)
                {
                    float timePassed = Time.time - attackStartTime;
                    float lerpTime = timePassed / attackSlideDuration;
                    movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                }
                break;
        }
        //Update sugar rush timer
        if (sugarRushIsActivated)
        {
            GameObject timerGo = GameObject.FindWithTag("Timer");
            Timer timer = timerGo.GetComponent<Timer>();
            timer.candySlider.value -= 4 * Time.deltaTime;

            if (timer.candySlider.value == timer.candySlider.minValue)
            {
                SwitchStateTo(STATE.FREE);

                srParticles.Stop();
                sugarRushIsActivated = false;
                
                comboScript.isSugarRushing = false;
                SRTimer = 0;
                combo = 0;
                currentMoveSpeed = moveSpeedBase + (moveSpeedLevel * moveMultiplier);
            }
        }
            // Apply Gravity:
            if (!cc.isGrounded) { verticalVelocity = gravity; }
        else { verticalVelocity = gravity * 0.3f; }

        // Apply movement:
        movementVelocity += verticalVelocity * Vector3.up * Time.deltaTime;
        cc.Move(movementVelocity * sugarRushValue);
    }
    //=============================================Calculate Movement===============================================
    private void CalculateMovement()
    {
        // Update states:
        // Change to attack state
        ChangeToAttackCheck();
        ChangeToDashCheck();
       
        //ChangeToSugarRushCheck();
        
        // Calculate Movement based on Input.
        movementVelocity.Set(input.horizontalInput, 0f, input.verticalInput);
        movementVelocity.Normalize();

        // The game is rotated by -45 degrees to give an isometric view:
        // Update rotation of y-axis for movement to match input with the game view.
        movementVelocity = Quaternion.Euler(0, -45, 0) * movementVelocity;

        // Update movement to 'movementVelocity' variable which is used to apply movement.
        movementVelocity *= currentMoveSpeed * Time.deltaTime;

        //IN PROGRESS: New rotation working with xbox controller.
        /* rot = new Vector3(input.horizontalRotation, 0f, input.verticalRotation);
         rot = Quaternion.Euler(0, -45, 0) * rot;
         if (rot != Vector3.zero)
         {
             transform.rotation = Quaternion.LookRotation(rot);
         }*/

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
        if(currentState == STATE.SUGAR_RUSH_ATTACK)
        {
            SwitchStateTo(STATE.SUGAR_RUSH);
        }
        else SwitchStateTo(STATE.FREE);
    }
   
    public void DashAnimationEnd()
    {
        if (currentState == STATE.DASH) //This is a safe guard since using the same animation for move and dash. Remove this and add animationevent to dash when fin. Remember to remove this method from walk animation event.
        {
            SwitchStateTo(STATE.FREE);
            
        }
        if(currentState == STATE.SUGAR_RUSH_DASH)
        {
            SwitchStateTo(STATE.SUGAR_RUSH);
        }
    }

    public void DeathAnimationEnd()
    {
        deadUI.SetActive(true);
        Time.timeScale = 0;
        
    }
    //==============================================StateChanges===============================
    public void SwitchStateTo(STATE _newState)
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
            case STATE.SUGAR_RUSH:
                
                break;
            case STATE.SUGAR_RUSH_DASH:
                break;
            case STATE.SUGAR_RUSH_ATTACK:

                break;
        }
        // Enter new state
        switch (_newState)
        {
            case STATE.FREE:
                Debug.Log("State: FREE");
                break;
            case STATE.ATTACKING:
                // Update Animator: Play animation for attacking.
                animator.SetTrigger("Attacking");
                // Stop Movement
                movementVelocity = Vector3.zero;
                // Update Rotation to face the direction immediately
                // Get time from entering new state for attack sliding.
                attackStartTime = Time.time;
                Debug.Log("State: ATTACK");
                break;
            case STATE.DASH:
                // Update Animator: Play animation for Dashing.
                animator.SetTrigger("Dashing");
                // Stop Movement
                movementVelocity = Vector3.zero;
                //Spawn dash particle
                SpawnDashParticle();
                Debug.Log("State: DASH");
                break;
            case STATE.HIT:

                break;
            case STATE.DEAD:
                animator.SetTrigger("Death");
                
                Debug.Log("State: DEAD");
                break;

            case STATE.SUGAR_RUSH:
                currentMoveSpeed = moveSpeedSR + (moveSpeedLevel * moveMultiplier);
                //some damage multipler
                //health invruablity thingo here to
                if (!sugarRushIsActivated)
                {
                    srParticles.Play();
                    sugarRushIsActivated = true;
                    comboScript.isSugarRushing = true;
                    SRTimer = 0;
                }
                
                Debug.Log("State: SUGAR_RUSH");
                break;
            case STATE.SUGAR_RUSH_ATTACK:
                currentMoveSpeed = moveSpeedSR + (moveSpeedLevel * moveMultiplier);
                // Update Animator: Play animation for attacking.
                animator.SetTrigger("Attacking");
                // Stop Movement
                movementVelocity = Vector3.zero;
                // Update Rotation to face the direction immediately
                // Get time from entering new state for attack sliding.
                attackStartTime = Time.time;
                Debug.Log("State: SURGAR_RUSH_ATTACK");
                break;

            case STATE.SUGAR_RUSH_DASH:
                // Update Animator: Play animation for Dashing.
                animator.SetTrigger("Dashing");
                // Stop Movement
                movementVelocity = Vector3.zero;
                // Update Rotation to face the direction immediately
                Debug.Log("State: SUGAR_RUSH_DASH");
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
        if (input.attackButtonPressed && !PauseMenu.wasPaused)
        {
            if (currentState == STATE.SUGAR_RUSH)
            {
                SwitchStateTo(STATE.SUGAR_RUSH_ATTACK);
            }
            else
            { SwitchStateTo(STATE.ATTACKING); }

            return;
        }
    }

    private void ChangeToSugarRushCheck()
    {
        GameObject timerGo = GameObject.FindWithTag("Timer");
        Timer timer = timerGo.GetComponent<Timer>();
        
        if (timer.candySlider.value == timer.candySlider.maxValue)
        {

            SwitchStateTo(STATE.SUGAR_RUSH);
            return;
        }
    }
    public void SpawnDashParticle()
    {
        GameObject effectsManagerObject = GameObject.FindWithTag("EffectsManager");
        EffectsManager effectsManager = effectsManagerObject.GetComponent<EffectsManager>();

        Instantiate(effectsManager.dashParticle, transform.position, transform.rotation);
    }

}