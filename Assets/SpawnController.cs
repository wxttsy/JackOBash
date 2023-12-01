using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public float enemiesToSpawn;
    public int timeInterval;
    public int baseAmount;
    public int timeMultiple;
    public int doorOpenNumber;
    public float timePlayed;
    public int timeDiv;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesToSpawn <= doorOpenNumber)
        {
            //door open
        }
    }

    public void CalculateEnemiesToSpawn()
    {
        //divide time by a time interval and round it to an int
        //Apply that to number to a multiple to get time spawn ramping
        //enemiesToSpawn = base amount of enemies (beginning of game) + above number
        timePlayed = FindObjectOfType<BarDisplayUI>().timePlayed;
        timeDiv = Mathf.RoundToInt(timePlayed / timeInterval);
        enemiesToSpawn = baseAmount + (timeDiv * timeMultiple);

        //get enemies to spawn number and get 35% value and round to int
        float doorOpen = enemiesToSpawn * 0.35f;
        doorOpenNumber = Mathf.RoundToInt(doorOpen);
    }
}
