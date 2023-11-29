<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
=======
using TMPro;
>>>>>>> Stashed changes
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
<<<<<<< Updated upstream
   
=======
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject MenuFirst;


    //DeathScript
    public TMP_Text finalScore;
    GameObject player;
    Player playerScript;
    public GameObject deathUI;
    //DeathScript

    //PauseMenu
    public GameObject pauseMenuUI;
    public static bool GamePaused = false;
    public static bool wasPaused = false;
    //PauseMenu

    //OptionsMenu
    public GameObject optionsMenu;
    public GameObject PauseMainmenu;
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
        eventSystem.firstSelectedGameObject = MenuFirst;
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
>>>>>>> Stashed changes

    //loads a manually inputed scene by name
    public void LoadDebugScene()
    {
        SceneManager.LoadScene("DebugScene");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        eventSystem.firstSelectedGameObject = MenuFirst;
    }

    //loads the next scene in build settings
    //will be updated so we can randomly load scenes
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    //Quits game intended to use on a button
    public void QuitGame()
    {
        Application.Quit();
    }

<<<<<<< Updated upstream
   
=======

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
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
            eventSystem.firstSelectedGameObject = pauseMenuUI;
            Time.timeScale = 0;
            GamePaused = true;
        }
    }




    public void OptionsButton()
    {
        optionsMenu.SetActive(true);
        PauseMainmenu.SetActive(false);
        eventSystem.firstSelectedGameObject = optionsMenu;

        Time.timeScale = 0f;
        Debug.Log("onClickOptions CLicked");

        //if (PauseMainmenu != null && gameUI != null)
        //{
        //    gameUI.SetActive(false);
        //    PauseMainmenu.SetActive(false);
        //}

    }



    public void BackButton()
    {
        eventSystem.firstSelectedGameObject = PauseMainmenu;
        optionsMenu.SetActive(false);

        if (PauseMainmenu != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("BACK CLICKED");
            eventSystem.firstSelectedGameObject = PauseMainmenu;
        }

    }
>>>>>>> Stashed changes
}


