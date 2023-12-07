using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This script is for controlling the player state machine, sugar rush mechanic and store the player's score.
/// </summary>
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
    public UiManager _uiManager;

    // Rotation:
    private Vector3 rot;
    private Vector3 oldRot;
    private Vector3 moveDir;

    //Item check to not allow for duplicate items
    [HideInInspector] public bool hasItem = false;

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
        [Tooltip("This is the multiplier rate to increase sugar rush decay")]
    public float sRushDecayMult;
    float sRushDecayStorage;

    // Attack sliding: This is for slight movement after attacking.
    // NOTE: This will make the attack animation look smoother once implemented.
    [Header("****Attacks****")]
    private float attackStartTime;
        [Tooltip("This is how long to move forward when attacking.")]
    public float attackSlideDuration = 0.2f;
        [Tooltip("This is the speed at which to move when attacking.")]
    public float attackSlideSpeed = 0.02f;
    private float attackAnimationDuration;
    // States:
    public enum STATE
    {
        FREE,
        ATTACKING,
        DASH,
        DEAD,

    }
    //[HideInInspector]
    public STATE currentState;

    // Other: 
    [Header("****Other****")]
    [Tooltip("This is the current killCounter we have accumulated.")]
    public int killCounter = 0;
    public int playerScore;

    //*******************************************************************************************************************
    //-------------------------------------------------Methods-----------------------------------------------------------
    //*******************************************************************************************************************
    private void Start(){
        _characterController = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _attackHitbox = GetComponentInChildren<DamageCollision>();
        currentMoveSpeed = moveSpeedNormal;
        float temp = sRushDecayMult;
        sRushDecayMult = temp / 100;
        sRushDecayStorage = sRushDecayMult;
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
                //stop movement??
                CalculateMovement();
                Vector3 temp = movementVelocity;
                movementVelocity = temp / 2;
                //look in direction of right stick (aim)
                transform.rotation = Quaternion.LookRotation(oldRot);

                // Slide the player in that direction slightly when attacking.
                if (Time.time < attackStartTime + attackSlideDuration) {
                    float timePassed = Time.time - attackStartTime;
                    float lerpTime = timePassed / attackSlideDuration;
                    movementVelocity = Vector3.Lerp(rot * attackSlideSpeed, Vector3.zero, lerpTime);
                }

                if (_input.attackButtonPressed && _characterController.isGrounded) {
                    string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    attackAnimationDuration = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                    if (currentClipName != "Attack_03" && attackAnimationDuration > 0.5f && attackAnimationDuration < 0.7f)  {
                        _input.attackButtonPressed = false;
                        SwitchStateTo(STATE.ATTACKING);
                        CalculateMovement();
                    }
                }
                break;
            
            case STATE.DASH:
                _animator.SetFloat("Speed", 0f);
                movementVelocity = transform.forward * dashSpeed * Time.deltaTime;
                ChangeToAttackCheck(_input.attackButtonPressed, UiManager.wasPaused);
                break;
            case STATE.DEAD:
                // Stop Movement
                _animator.SetFloat("Speed", 0f);
                dashSpeed = 0f;
                movementVelocity = Vector3.zero;
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
    /// <summary>
    /// FOR THE PLAYER: This is for updating the movementvelocity variable to apply movement in update.
    /// </summary>
    private void CalculateMovement(){
        // Update states:
        ChangeToAttackCheck(_input.attackButtonPressed, UiManager.wasPaused);
        ChangeToDashCheck(_input.dashButtonPressed);
        
        
        // Calculate Movement based on Input.
        movementVelocity.Set(_input.horizontalInput, 0f, _input.verticalInput);
        movementVelocity.Normalize();

        // The game is rotated by -45 degrees to give an isometric view:
        // Update rotation of y-axis for movement to match input with the game view.
        movementVelocity = Quaternion.Euler(0, -45, 0) * movementVelocity;

        // Update movement to 'movementVelocity' variable which is used to apply movement.
        if(movementVelocity == Vector3.zero){
            _animator.SetTrigger("NotMoving");
        } else {
            _animator.SetTrigger("Moving");
        }

        movementVelocity *= currentMoveSpeed * Time.deltaTime;
        //*********************************************************************
        //     New rotation working with xbox controller. (right stick)
        //*********************************************************************
        // Update current rotation with right stick input.
        // Keep old rot if player moves joystick in a direction:
        if (rot != Vector3.zero)
            oldRot = rot;

        rot = new Vector3(_input.horizontalRotation, 0f, _input.verticalRotation);
        rot = Quaternion.Euler(0, -45, 0) * rot;
        // If player isnt moving:
        if (moveDir == Vector3.zero) {
            // If player isnt looking around (right stick 0):
            if (rot == Vector3.zero){
                // Player transform looks in movement direction before they stopped.
                transform.rotation = Quaternion.LookRotation(oldRot);
            }
            // If player is looking around (right stick 0):
            else if (rot != Vector3.zero){
                // Player transform looks in direction player is directing right stick.
                transform.rotation = Quaternion.LookRotation(rot);
            }
        }

        // Movement direction with controller (left stick)
        // Update movement direction:
        moveDir = new Vector3(_input.horizontalInput, 0f, _input.verticalInput);
        moveDir = Quaternion.Euler(0, -45, 0) * moveDir;

        //If player is moving (with new movementdirection)
        if (moveDir != Vector3.zero) {
            //Player transform looks in the direction of the movement
            transform.rotation = Quaternion.LookRotation(moveDir);

            if (rot == Vector3.zero)
                rot = moveDir;
        }
    }
    //*******************************************************************************************************************
    //-------------------------------------------------Sugar Rush--------------------------------------------------------
    //*******************************************************************************************************************
    /// <summary>
    /// Method for updating the all of the sugar rush mechanic variables.
    /// Including: Playing sound effects, updating the sugar rush slider value in UI, increasing player movement 
    /// in this state and reverting it back when not in this state.
    /// </summary>
    private void ManageSugarRushCheck(){
        GameObject barDisplayUIObject = GameObject.FindWithTag("BarDisplayUI");
        BarDisplayUI barDisplayUIScript = barDisplayUIObject.GetComponent<BarDisplayUI>();

        // Sugar rush is enabled:
        if (sugarRushIsActivated){
            barDisplayUIScript.sugarRushSlider.value -= 6 * Time.deltaTime * (1 + sRushDecayMult);
            sRushDecayMult += sRushDecayMult / 4 * Time.deltaTime;
            if (barDisplayUIScript.sugarRushSlider.value == barDisplayUIScript.sugarRushSlider.minValue){
                //Play SugarRushExit sound
                GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                audioManager.PlayAudio(audioManager.sfSugarRushExit);

                _sugarRushParticleEffect.Stop();
                sRushDecayMult = sRushDecayStorage;
                sugarRushIsActivated = false;
                currentMoveSpeed = moveSpeedNormal;
            }
        }else{ // Sugar rush is disabled:
            currentMoveSpeed = moveSpeedNormal;
          
            if (barDisplayUIScript.sugarRushSlider.value >= barDisplayUIScript.sugarRushSlider.maxValue){
                //Play SugarRushEntry sound
                GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                audioManager.PlayAudio(audioManager.sfSugarRushEntry);

                currentMoveSpeed = moveSpeedSugarRush;
                sugarRushIsActivated = true;
                _sugarRushParticleEffect.Play();
            }
        }
    }
    //*******************************************************************************************************************
    //------------------------------------------------State Management---------------------------------------------------
    //*******************************************************************************************************************
    /// <summary>
    /// For changing the state the player is in.
    /// </summary>
    /// <param name="_newState">The state to change to. Type: Player.STATE</param>
    public void SwitchStateTo(STATE _newState){
        // Update variables:
        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
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
                if (_attackHitbox != null)
                    _attackHitbox.DisableDamageCollider();
                break;

            case STATE.DASH:
                //Play dash sound
                audioManager.PlayAudio(audioManager.sfPlayerDash);
                _animator.SetTrigger("Dashing");
                // Stop Movement
                movementVelocity = Vector3.zero;
                // Spawn dash particle
                SpawnDashParticle();
                break;

            case STATE.DEAD:
                Debug.Log("Deathstate switch");
                _animator.SetTrigger("Death");
                break;
        }
        currentState = _newState;
    }
    //*******************************************************************************************************************
    //-------------------------------------Colliders & Animation Event Methods-------------------------------------------
    //*******************************************************************************************************************
    /// <summary>
    /// This is called through animation events.
    /// Enable the hitbox attached to this player.
    /// </summary>
    public void EnableDamageCollider(){ _attackHitbox.EnableDamageCollider(); }
    /// <summary>
    /// /// This is called through animation events.
    /// Disable the hitbox attached to this player.
    /// </summary>
    public void DisableDamageCollider(){ _attackHitbox.DisableDamageCollider(); }
    /// <summary>
    /// This is called through animation events.
    /// Switch state to free after attacking.
    /// </summary>
    public void AttackAnimationEnd() { SwitchStateTo(STATE.FREE); }
    /// <summary>
    /// This is called through animation events.
    /// Switch state to free after dashing.
    /// </summary>
    public void DashAnimationEnd() { SwitchStateTo(STATE.FREE); }

    /// <summary>
    /// This is called through animation events.
    /// Enable the death screen and end the game loop at the end of death animation.
    /// </summary>
    public void DeathAnimationEnd() {
        if (_uiManager == null){
            Debug.Log("ui manager is null.");
        } else {
            _uiManager.onDeath();
        }
    }
    //*******************************************************************************************************************
    //-----------------------------------------------State Change Checks-------------------------------------------------
    //*******************************************************************************************************************
    /// <summary>
    /// Switch state to STATE.DASH after recieving dash input.
    /// </summary>
    private void ChangeToDashCheck(bool dashInputPressed){
        if (dashInputPressed) {
            SwitchStateTo(STATE.DASH);
        }
    }
    /// <summary>
    /// Switch state to STATE.ATTACKING after recieving attack input.
    /// </summary>
    private void ChangeToAttackCheck(bool attackButtonPressed, bool gameIsPaused) {
        if (attackButtonPressed && !gameIsPaused){
            SwitchStateTo(STATE.ATTACKING);
        }
    }
    //*******************************************************************************************************************
    /// <summary>
    /// Display dash particle effect.
    /// </summary>
    public void SpawnDashParticle(){
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
        Instantiate(gameManager.dashParticle, transform.position, transform.rotation);
    }
    /// <summary>
    /// Play Bat swing sound effect.
    /// </summary>
    public void PlayBatSwing() {
        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
        //Play bat swing sound
        audioManager.PlayAudio(audioManager.sfBatSwing);
    }
    /// <summary>
    /// Play death sound effect.
    /// </summary>
    public void PlayDeathSound() {
        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
        audioManager.PlayAudio(audioManager.sfPlayerDeath);
    }
}