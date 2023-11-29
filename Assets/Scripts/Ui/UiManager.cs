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
    public GameObject Ui1;
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
    }

    //Quits game intended to use on a button
    public void QuitGame()
    {
        Application.Quit();
    }


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
            Time.timeScale = 0;
            GamePaused = true;
        }
    }




    public void OptionsButton()
    {
        if (Ui1 != null)
        {
            Ui1.SetActive(false);
            optionsMenu.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("onClickOptions CLicked");
        }

    }

    public void BackButton()
    {
        if (Ui1 != null)
        {
            Ui1.SetActive(true);
            optionsMenu.SetActive(false);
            Time.timeScale = 0f;
            Debug.Log("BACK CLICKED");
        }
    }

}
