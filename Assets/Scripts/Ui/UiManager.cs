using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class UiManager : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject mainMenuFirst;
    [SerializeField] GameObject optionsBack;
    [SerializeField] GameObject pauseFirst;
    [SerializeField] GameObject deathFirst;



    //DeathScript
    public TMP_Text finalScore;
    GameObject player;
    Player playerScript;
    public GameObject deathUI;
    //DeathScript

    //PauseMenu
    public static bool GamePaused = false;
    public static bool wasPaused = false;
    public GameObject pauseMenuUI;
    //PauseMenu

    //OptionsMenu
    public GameObject optionsMenu;
    public GameObject mainORpause;
    public GameObject gameUI;
    //OptionsMenu

    private void Awake()
    {

        player = GameObject.FindWithTag("Player");
        if (player != null)
            playerScript = player.GetComponent<Player>();

        Resume();

        if (optionsMenu != null)
        {
            optionsMenu.SetActive(false);
        }
    }
    private void Start()
    {
        eventSystem.firstSelectedGameObject = mainMenuFirst;

        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }


    }

    void Update()
    {
        if (player != null)
        {
            finalScore.text = "" + playerScript.playerScore;

        }


        //=====================================================PAUSE MENU========================================================================
        PlayerInput _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        if (_input.pauseButtonPressed)
        {
            if (GamePaused)
            {
                
                Resume();
            }
            else
            {
                
                Pause();
            }
                _input.ClearCache();
        }

    }
    //=============================================================MAIN MENU============================================
    public void LoadDebugScene()
    {
        SceneManager.LoadScene("DebugScene");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        eventSystem.SetSelectedGameObject(mainMenuFirst);
    }

    //Quits game intended to use on a button
    public void QuitGame()
    {
        Application.Quit();
    }

    //===========================================PAUSE MENU==================================================================================

    public void Resume()
    {
        PlayerInput _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        _input.controls.Player.Enable();
        _input.controls.UI.Disable();
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            
            gameUI.SetActive(true);
            Time.timeScale = 1.0f;
            GamePaused = false;
            Debug.Log("Game Resumed");
        }
    }

    //Completly pauses the game setting the games time to zero and the pause menu to be active
    public void Pause()
    {
        PlayerInput _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        _input.controls.UI.Enable();
        _input.controls.Player.Disable();
        pauseMenuUI.SetActive(true);
        eventSystem.SetSelectedGameObject(pauseFirst);

        gameUI.SetActive(false);

        Time.timeScale = 0;
        GamePaused = true;

        Debug.Log("Game Paused");
    }



    //================================================OPTIONS MENU=============================================================================
    public void OptionsButtonpPressed()
    {
        if (optionsMenu != null)
        {
            eventSystem.SetSelectedGameObject(optionsBack);

            optionsMenu.SetActive(true);
            mainORpause.SetActive(false);
        }

        //if (gameUI != null)
        //{
        //    gameUI.SetActive(false);
        //}

        Time.timeScale = 0f;
        Debug.Log("onClickOptions CLicked");


    }

    public void BackButton()
    {
        eventSystem.SetSelectedGameObject(mainMenuFirst);
        eventSystem.SetSelectedGameObject(pauseFirst);

        mainORpause.SetActive(true);

        optionsMenu.SetActive(false);
        Time.timeScale = 0f;
        Debug.Log("BACK CLICKED");

    }

}
