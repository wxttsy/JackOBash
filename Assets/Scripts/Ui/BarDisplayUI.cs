using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarDisplayUI : MonoBehaviour
{
    //*******************************************************************************************************************
    //------------------------------------------Initialize Variables-----------------------------------------------------
    //*******************************************************************************************************************
    // References:
    public TMP_Text healthText;
    public Slider healthSlider;
    public Slider sugarRushSlider;
    private GameObject player;
    Health playerHealthScript;

    public TMP_Text timeSurvived;
    public TMP_Text timeSurvivedDeath;
    public TMP_Text scoreDeath;
    private float timePlayed = 0;

    //*******************************************************************************************************************
    //---------------------------------------------------Awake-----------------------------------------------------------
    //*******************************************************************************************************************
    private void Awake()
    {
        timePlayed = 0;
        player = GameObject.FindWithTag("Player");
        playerHealthScript = player.GetComponent<Health>();
        playerHealthScript.currentHealth = playerHealthScript.maxHealth;
    }

    //*******************************************************************************************************************
    //---------------------------------------------------Update----------------------------------------------------------
    //*******************************************************************************************************************
    private void Update()
    {
        timePlayed += 1 * Time.deltaTime;
        timeSurvivedDeath.SetText(Mathf.Round(timePlayed).ToString());
        timeSurvived.SetText(Mathf.Round(playerHealthScript.currentHealth).ToString());
        Player playerScript = player.GetComponent<Player>();
        
        // Do not decrement the player's health if it is going to be less than 0: Instead display 0.
        if (playerHealthScript.currentHealth < 0)
        {
            playerHealthScript.currentHealth = 0;
            playerScript.SwitchStateTo(Player.STATE.DEAD);
            return;
        }
        // Do not increment the player's health if it is going to be more than the max health value: Instead display max health value.
        if (playerHealthScript.currentHealth > playerHealthScript.maxHealth)
        {
            playerHealthScript.currentHealth = playerHealthScript.maxHealth;
            UpdateSugarRushTimer();
            return;
        }
        //Otherwise, increment appropriately.
        UpdateSugarRushTimer();
        healthText.text = "" + playerScript.playerScore;
        scoreDeath.text = "" + playerScript.playerScore;
        healthSlider.value = (int)playerHealthScript.currentHealth;
    }
    void UpdateSugarRushTimer()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript.sugarRushIsActivated)
        {
            if (playerHealthScript.currentHealth > 0) playerHealthScript.currentHealth += 1 * Time.deltaTime;
        }
        else
        {
            if (playerHealthScript.currentHealth > 0) playerHealthScript.currentHealth -= 1 * Time.deltaTime;
        }
    }
}