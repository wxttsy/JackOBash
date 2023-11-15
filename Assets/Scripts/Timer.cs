using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float maxTime = 100;
    public TMP_Text timeDisplay;
    public GameObject player;
    public Health playerHp;
    PlayerMovement _playerMovement;
    private float timePlayed = 0;
    public Slider timeSlider;
    public Slider candySlider;
    public float depreciationValue;
    // Start is called before the first frame update
    private void Awake()
    {
        timePlayed = 0;
        player = GameObject.FindWithTag("Player");
        playerHp = player.GetComponent<Health>();
        playerHp.currentHealth = maxTime;
        _playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        timePlayed += 1 * Time.deltaTime;
        //Do not increment the timer if it is going to be less than 0. Instead display 0.
        if (playerHp.currentHealth < 0)
        {
            playerHp.currentHealth = 0;
            timeDisplay.text = "0";
            timeSlider.value = (int)playerHp.currentHealth;
            return;
        }
        if (playerHp.currentHealth >= 100)
        {
            playerHp.currentHealth = 100;
            timeDisplay.text = "100";
            timeSlider.value = (int)playerHp.currentHealth;
            return;
        }
        //Otherwise, increment appropriately.

        if (_playerMovement.sugarRushIsActivated)
        {

            playerHp.currentHealth += 1 * Time.deltaTime;
        }
        else
        {

            playerHp.currentHealth -= 1 * Time.deltaTime;
        }




        timeDisplay.text = "" + ((int)playerHp.currentHealth+1) + "/100";
        timeSlider.value = (int)playerHp.currentHealth;



    }
}
