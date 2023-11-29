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
    private Vector3 rot;
    private Vector3 oldRot;
    private Vector3 moveDir;
    

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
                //stop movement??
                movementVelocity = Vector3.zero;
                //look in direction of right stick (aim)
                transform.rotation = Quaternion.LookRotation(oldRot);

                // Slide the player in that direction slightly when attacking.
                if (Time.time < attackStartTime + attackSlideDuration)
                {
                    float timePassed = Time.time - attackStartTime;
                    float lerpTime = timePassed / attackSlideDuration;
                    movementVelocity = Vector3.Lerp(rot * attackSlideSpeed, Vector3.zero, lerpTime);
                }

                if (_input.attackButtonPressed && _characterController.isGrounded)
                {
                    string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    attackAnimationDuration = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                    if (currentClipName != "Attack_03" && attackAnimationDuration > 0.5f && attackAnimationDuration < 0.7f)
                    {
                        _input.attackButtonPressed = false;
                        SwitchStateTo(STATE.ATTACKING);
                        CalculateMovement();
                    }
                }
                break;
            
            case STATE.DASH:
                _animator.SetFloat("Speed", 0f);
                movementVelocity = transform.forward * dashSpeed * Time.deltaTime;
                ChangeToAttackCheck(_input.attackButtonPressed, PauseMenu.wasPaused);
                break;
            case STATE.DEAD:
                // Stop Movement
                _animator.SetFloat("Speed", 0f);
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
        if(movementVelocity == Vector3.zero)
        {
            _animator.SetFloat("Speed", 0);
        }
        else
        {
            _animator.SetFloat("Speed", 1);
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

        //*** New movement direction with controller (left stick)
        //update movement direction
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
    private void ManageSugarRushCheck(){
        GameObject barDisplayUIObject = GameObject.FindWithTag("BarDisplayUI");
        BarDisplayUI barDisplayUIScript = barDisplayUIObject.GetComponent<BarDisplayUI>();

        // Sugar rush is enabled:
        if (sugarRushIsActivated){

            barDisplayUIScript.sugarRushSlider.value -= 4 * Time.deltaTime;
            if (barDisplayUIScript.sugarRushSlider.value == barDisplayUIScript.sugarRushSlider.minValue){
                //Play SugarRushExit sound
                GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
                AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
                audioManager.PlayAudio(audioManager.sfSugarRushExit);

                _sugarRushParticleEffect.Stop();
                sugarRushIsActivated = false;
                currentMoveSpeed = moveSpeedNormal;
            }
        }else{ // Sugar rush is disabled:
            currentMoveSpeed = moveSpeedNormal;
          
            if (barDisplayUIScript.sugarRushSlider.value >= barDisplayUIScript.sugarRushSlider.maxValue)
            {
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
    public void SwitchStateTo(STATE _newState){
        // Update variables:

        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
        _input.ClearCache();
        // Enter new state:
        switch (_newState) {
            case STATE.FREE:
                //Get random number for footstep type
                //int rand;
                //rand = Random.Range(0, 4);
                ////Footstep1 sound
                //if (rand == 0)
                //{
                //    audioManager.PlayAudio(audioManager.sfPlayerFootsteps1);
                //}
                ////Footstep2 sound
                //else if (rand == 1)
                //{
                //    audioManager.PlayAudio(audioManager.sfPlayerFootsteps2);
                //}
                ////Footstep3 sound
                //else if (rand == 2)
                //{
                //    audioManager.PlayAudio(audioManager.sfPlayerFootsteps3);
                //}
                ////Footstep4 sound
                //else if (rand == 3)
                //{
                //    audioManager.PlayAudio(audioManager.sfPlayerFootsteps4);
                //}

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

    public void PlayBatSwing()
    {
        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
        //Play bat swing sound
        audioManager.PlayAudio(audioManager.sfBatSwing);
    }

    public void PlayDeathSound()
    {
        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
        audioManager.PlayAudio(audioManager.sfPlayerDeath);
    }
}