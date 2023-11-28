using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public static bool wasPaused = false;
    public GameObject pauseMenuUI;
    public GameObject UiPanel;

    private void Awake()
    {
        Resume();
    }

    void Update()
    {
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        UiPanel.SetActive(true);
        Time.timeScale = 1.0f;
        GamePaused = false;
    }

    //Completly pauses the game setting the games time to zero and the pause menu to be active
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        UiPanel.SetActive(false);
        Time.timeScale = 0;
        GamePaused = true;
    }
}
