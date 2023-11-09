using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text score;
    GameObject player;
    PlayerMovement playerScript;
    public GameObject deadUI;
    private void Awake()
    {
        deadUI.SetActive(false);
        player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            playerScript = player.GetComponent<PlayerMovement>();
            Debug.Log("Found Player [SCOREDISPLAY]");

        }
        else
        {
            Debug.Log("Couldnt Find Player [SCOREDISPLAY");
        }
    }
    //private void Start()
    //{
    //    deadUI.SetActive(false);
    //}

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
           score.text = "" + playerScript.playerScore;
        }
    }
}
