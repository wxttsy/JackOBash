using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour
{
    public TMP_Text comboDisplay;
    GameObject player;
    PlayerMovement playerScript;
    public bool isSugarRushing;

    public float maxCombo = 5;
    public bool comboFull = false;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
       
        UpdateCombo();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCombo();


    }
    private void UpdateCombo()
    {

        if (player != null)
        {
            //comboDisplay.text = "" + playerScript.combo;

        }

    }

    //checks to see if combo is full 
    public bool ComboFull()
    {
        if (playerScript.combo >= maxCombo)
        {
            //enters sugar rush state
            return true;
        }
        //continue as normal
        return false;
    }

}

