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

    public Slider timeSlider;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerHp = player.GetComponent<Health>();
        currentTime = maxTime;
    }

    // Update is called once per frame
    private void Update()
    {
        //Do not increment the timer if it is going to be less than 0. Instead display 0.
        if (currentTime < 0)
        {
            currentTime = 0;
            timeDisplay.text = "0";
            return;
        }
        //Otherwise, increment appropriately.
        currentTime = playerHp.currentHealth;
        playerHp.currentHealth -= 1 * Time.deltaTime;
        timeDisplay.text = "" + (int)currentTime;

        
    }
}
