using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float maxTime = 100;
    public float currentTime;
    public TMP_Text timeDisplay;
    public GameObject player;
    Health playerHp;
    Combo combo;

    public Slider timeSlider;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerHp = player.GetComponent<Health>();
        combo = player.GetComponent<Combo>();
        currentTime = maxTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!combo.isSugarRushing)
        {
            currentTime = playerHp.currentHealth;
            playerHp.currentHealth -= 1 * Time.deltaTime;
            timeDisplay.text = "" + (int)currentTime;
        }
        else if (combo.isSugarRushing)
        {
            currentTime = playerHp.currentHealth;
            playerHp.currentHealth = 99;
            timeDisplay.text = "" + (int)currentTime;
        }

    }
}
