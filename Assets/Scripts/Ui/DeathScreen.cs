using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public TMP_Text finalScore;
    GameObject player;
    Player playerScript;
    public static bool GamePaused = false;
    public static bool wasPaused = false;
    public GameObject deathUI;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }
    private void Start()
    {
        deathUI.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            finalScore.text = "Score: " + playerScript.playerScore;
           
        }
    }
    
  
}
