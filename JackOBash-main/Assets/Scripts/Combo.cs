using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour
{
    public TMP_Text comboDisplay;

    // Start is called before the first frame update
    private void Awake()
    {
        UpdateCombo();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCombo();
    }
    //====================================Update the text by getting the combo variable from the Player==========================================
    private void UpdateCombo()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
            comboDisplay.text = "" + playerScript.combo;
        }
    }
}
