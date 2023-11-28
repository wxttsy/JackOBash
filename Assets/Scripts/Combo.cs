using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour
{
    public TMP_Text comboDisplay;
    public float comboIncreaseFactor;
    public float comboDecreaseFactor;
    public float comboIncreaseMultiplier;
    public float comboMeter;
    GameObject player;
    PlayerMovement playerScript;
    public float sugarRushOverride;
    public bool isSugarRushing;
    public bool isPrimed;
    


    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        comboMeter = 0;
        UpdateCombo();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCombo();

        if (comboMeter > 0 && comboMeter < 100)
        {
            DepreciateMeter();
        }
        else if (comboMeter >= 100)
        {
            comboMeter = 100;
            DepreciateMeter();

        }
        
        if(comboMeter > 80)
        {
            isPrimed = true;
        }


        if (comboMeter <= 0 && isSugarRushing)
        {
            EndSugarRush();
            comboMeter = 0;
            isSugarRushing = false;
        }


    }
    //====================================Update the text by getting the combo variable from the Player==========================================
    private void UpdateCombo()
    {


        if (player != null)
        {

            comboDisplay.text = "" + playerScript.combo;

            if (playerScript.doComboMeterIncrease)
            {
                comboMeter += comboIncreaseFactor * comboIncreaseMultiplier;
                playerScript.doComboMeterIncrease = false;
            }
        }

    }

    private void DepreciateMeter()
    {



        comboMeter -= comboDecreaseFactor * Time.deltaTime;
    }

    public void GoGoSugarRush()
    {
        playerScript.sugarRushValue = sugarRushOverride;
    }

    public void EndSugarRush()
    {
        //Debug.Log("OH MY GOD IM LITERALLY ZOOMING RIGHT NOW");
        playerScript.sugarRushValue = 1;
    }
}

