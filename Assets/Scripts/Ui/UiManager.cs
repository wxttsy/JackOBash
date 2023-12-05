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
    public GameObject mainMenuUI;
    public GameObject optionsMenu;
    public GameObject gameUI;
    //OptionsMenu

    private void Awake()
    {
        optionsMenu.SetActive(false); 
        pauseMenuUI.SetActive(false);

        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
        }

        Resume();
    }
    private void Start()
    {
        eventSystem.firstSelectedGameObject = mainMenuFirst;

        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }

        if (player != null)
        {
            pauseMenuUI.SetActive(false);
        }

        optionsMenu.SetActive(false);

    }

    void Update()
    {
//=====================================================PAUSE MENU========================================================================
        if (player != null)
        {
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
        if (player != null)
        {
            PlayerInput _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

            _input.controls.Player.Enable();
            _input.controls.UI.Disable();

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
        if (player != null)
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
    }



    //================================================OPTIONS MENU=============================================================================
    public void OptionsButtonpPressed()
    {
        if (optionsMenu != null)
        {
            eventSystem.SetSelectedGameObject(optionsBack);
            optionsMenu.SetActive(true);

            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);

            }
        }

        mainMenuUI.SetActive(false);
        if (gameUI != null)
        {
            gameUI.SetActive(false);
        }

        Time.timeScale = 0f;
        Debug.Log("onClickOptions CLicked");


    }

    public void BackButton()
    {
        optionsMenu.SetActive(false);
        mainMenuUI.SetActive(true);
        eventSystem.SetSelectedGameObject(mainMenuFirst);
        //if (player != null)
        //{
        //    eventSystem.SetSelectedGameObject(pauseFirst);
        //    pauseMenuUI.SetActive(true);
        //}



        Time.timeScale = 0f;
        Debug.Log("BACK CLICKED");

    }

}
