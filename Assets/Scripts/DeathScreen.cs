using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public TMP_Text finalScore;
    GameObject player;
    PlayerMovement playerScript;
    public static bool GamePaused = false;
    public static bool wasPaused = false;
    public GameObject deathUI;

  
    private void Start()
    {
        deathUI.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            finalScore.text = "" + playerScript.playerScore;
           
        }
    }
    
  
}
