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

    public Slider timeSlider;
    // Start is called before the first frame update
    private void Awake()
    {
        currentTime = maxTime;
    }

    // Update is called once per frame
    private void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        timeDisplay.text = "" + (int)currentTime;
    }
}
