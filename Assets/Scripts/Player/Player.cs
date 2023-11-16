using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //*******************************************************************************************************************
    //------------------------------------------Initialize Variables-----------------------------------------------------
    //*******************************************************************************************************************
    // References:
    private CharacterController _characterController;
    private PlayerInput _input;
    private Animator _animator;
    private DamageCollision _attackHitbox;
    public ParticleSystem _sugarRushParticleEffect;

    // Rotation:
    private Vector3 oldMovementDirection;
    private Quaternion startOrientation;
    private float rotationTime = 0f;
    private float rotationDuration = 0.05f;
    private Vector3 rot;

    // Movement/Dash:
    private Vector3 movementVelocity;
    private float verticalVelocity;
    private float gravity = -20f;
    [Header("****Movement****")]
        [Tooltip("Any changes to this setting will not be saved! This is displayed for visual purposes only. This is the current speed at which the player is moving.")]
    public float currentMoveSpeed = 7f;
        [Tooltip("This is the value at which the player will move normally.")]
    public float moveSpeedNormal = 7f;
        [Tooltip("This value is the speed at which the player will dash.")]
    public float dashSpeed = 9f;

    // Sugar Rush:
    [Header("****Sugar Rush****")]
        [Tooltip("This is the value at which the player will move while in sugar rush state.")]
    public float moveSpeedSugarRush = 10f;
        [Tooltip("A true or false value which enables and disables Sugar rush.")]
    public bool sugarRushIsActivated = false;


    // Attack sliding: This is for slight movement after attacking.
    // NOTE: This will make the attack animation look smoother once implemented.
    [Header("****Attacks****")]
    private float attackStartTime;
        [Tooltip("This is how long to move forward when attacking.")]
    public float attackSlideDuration = 0.2f;
        [Tooltip("This is the speed at which to move when attacking.")]
    public float attackSlideSpeed = 0.02f;
    
    // States:
    public enum STATE
    {
        FREE,
        ATTACKING,
        DASH,
        DEAD,

    }
    private STATE currentState;

    // Other: ????
    [Header("****Other****")]
    [Tooltip("This is the current killCounter we have accumulated.")]
    public int killCounter = 0;

    public int playerScore;
    public GameObject deadUI;
    //*******************************************************************************************************************
    //-------------------------------------------------Methods-----------------------------------------------------------
    //*******************************************************************************************************************
    private void Start()
    {
        deadUI.SetActive(false);
    }
    private void Awake(){
        deadUI.SetActive(false);
        _characterController = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _attackHitbox = GetComponentInChildren<DamageCollision>();
        currentMoveSpeed = moveSpeedNormal;
    }
    //*******************************************************************************************************************
    //--------------------------------------------------Update-----------------------------------------------------------
    //*******************************************************************************************************************
    private void Update() {
        // Update Player based on state:
        switch (currentState){
            case STATE.FREE: 
                CalculateMovement();
                break;
            
            case STATE.ATTACKING: 
                _animator.SetFloat("Speed", 0f);
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
                _animator.SetFloat("Speed", 0f);
                movementVelocity = transform.forward * dashSpeed * Time.deltaTime;
                ChangeToAttackCheck(_input.attackButtonPressed, PauseMenu.wasPaused);
                break;
        }
        // Check if we can change into Sugar Rush:
        ManageSugarRushCheck();

        // Apply Gravity:
        if (!_characterController.isGrounded) { verticalVelocity = gravity; }
        else { verticalVelocity = gravity * 0.3f; }

        // Apply movement:
        movementVelocity += verticalVelocity * Vector3.up * Time.deltaTime;
        _characterController.Move(movementVelocity);
    }
    //*******************************************************************************************************************
    //---------------------------------------------Calculate Movement----------------------------------------------------
    //*******************************************************************************************************************
    private void CalculateMovement(){
        // Update states:
        ChangeToAttackCheck(_input.attackButtonPressed, PauseMenu.wasPaused);
        ChangeToDashCheck(_input.dashButtonPressed);
        
        // Calculate Movement based on Input.
        movementVelocity.Set(_input.horizontalInput, 0f, _input.verticalInput);
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
        _animator.SetFloat("Speed", movementVelocity.magnitude);
    }
    //*******************************************************************************************************************
    //-------------------------------------------------Sugar Rush--------------------------------------------------------
    //*******************************************************************************************************************
    private void ManageSugarRushCheck(){
        GameObject barDisplayUIObject = GameObject.FindWithTag("BarDisplayUI");
        BarDisplayUI barDisplayUIScript = barDisplayUIObject.GetComponent<BarDisplayUI>();

        // Sugar rush is enabled:
        if (sugarRushIsActivated){
            barDisplayUIScript.sugarRushSlider.value -= 4 * Time.deltaTime;
            if (barDisplayUIScript.sugarRushSlider.value == barDisplayUIScript.sugarRushSlider.minValue){
                _sugarRushParticleEffect.Stop();
                sugarRushIsActivated = false;
                currentMoveSpeed = moveSpeedNormal;
            }
        }else{ // Sugar rush is disabled:
            currentMoveSpeed = moveSpeedNormal;
            if (barDisplayUIScript.sugarRushSlider.value >= barDisplayUIScript.sugarRushSlider.maxValue)
            {
                currentMoveSpeed = moveSpeedSugarRush;
                sugarRushIsActivated = true;
                _sugarRushParticleEffect.Play();
            }
        }
    }

    //*******************************************************************************************************************
    //------------------------------------------------State Management---------------------------------------------------
    //*******************************************************************************************************************
    public void SwitchStateTo(STATE _newState){
        // Update variables:
        _input.ClearCache();
        // Enter new state:
        switch (_newState) {
            case STATE.FREE:
                break;

            case STATE.ATTACKING:
                _animator.SetTrigger("Attacking");
                // Stop Movement
                movementVelocity = Vector3.zero;
                // Get time from entering new state for attack sliding.
                attackStartTime = Time.time;
                break;

            case STATE.DASH:
                _animator.SetTrigger("Dashing");
                // Stop Movement
                movementVelocity = Vector3.zero;
                // Spawn dash particle
                SpawnDashParticle();
                break;

            case STATE.DEAD:
                _animator.SetTrigger("Death");
                break;
        }
        currentState = _newState;
    }
    //*******************************************************************************************************************
    //-------------------------------------Colliders & Animation Event Methods-------------------------------------------
    //*******************************************************************************************************************
    public void EnableDamageCollider(){ _attackHitbox.EnableDamageCollider(); }
    public void DisableDamageCollider(){ _attackHitbox.DisableDamageCollider(); }
    public void AttackAnimationEnd() { SwitchStateTo(STATE.FREE); }
    public void DashAnimationEnd() { SwitchStateTo(STATE.FREE); }

    public void DeathAnimationEnd(){
        deadUI.SetActive(true);
        Time.timeScale = 0;
    }
    //*******************************************************************************************************************
    //-----------------------------------------------State Change Checks-------------------------------------------------
    //*******************************************************************************************************************
    private void ChangeToDashCheck(bool dashInputPressed){
        if (dashInputPressed) {
            SwitchStateTo(STATE.DASH);
        }
    }
    private void ChangeToAttackCheck(bool attackButtonPressed, bool gameIsPaused) {
        if (attackButtonPressed && !gameIsPaused){
            SwitchStateTo(STATE.ATTACKING);
        }
    }
    //*******************************************************************************************************************
    public void SpawnDashParticle()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        GameManager gameManager = gameManagerObject.GetComponent<GameManager>();

        Instantiate(gameManager.dashParticle, transform.position, transform.rotation);
    }

    
}