using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject mainMenuFirst;
    [SerializeField] GameObject optionsBack;

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
            finalScore.text = "Score: " + playerScript.playerScore;

        }


        //=====================================================PAUSE MENU========================================================================

        //wasPaused makes it so theres a frame before the pause menu closes
        wasPaused = GamePaused;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void LoadDebugScene()
    {
        SceneManager.LoadScene("DebugScene");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        eventSystem.firstSelectedGameObject = mainMenuFirst;
    }

    //Quits game intended to use on a button
    public void QuitGame()
    {
        Application.Quit();
    }

    //===========================================PAUSE MENU==================================================================================

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            gameUI.SetActive(true);
            Time.timeScale = 1.0f;
            GamePaused = false;
        }
    }

    //Completly pauses the game setting the games time to zero and the pause menu to be active
    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            GamePaused = true;
        }
    }



    //================================================OPTIONS MENU=============================================================================
    public void OptionsButtonpPressed()
    {

        if (mainORpause != null)
        {
            optionsMenu.SetActive(true);
            mainORpause.SetActive(false);

            if (gameUI != null)
            {
                gameUI.SetActive(false);
            }

            Time.timeScale = 0f;
            Debug.Log("onClickOptions CLicked");
            eventSystem.firstSelectedGameObject = optionsBack;
        }

    }

    public void BackButton()
    {

        if (mainORpause != null)
        {
            mainORpause.SetActive(true);


            optionsMenu.SetActive(false);
            Time.timeScale = 0f;
            Debug.Log("BACK CLICKED");
            eventSystem.firstSelectedGameObject = mainMenuFirst;
        }
    }

}
