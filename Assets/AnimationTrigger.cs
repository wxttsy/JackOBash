using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{

    public Player player;
    private int currentKillCount;
    private int killsToGet;
    Animator animator1;
    Animator animator2;
    public GameObject door1;
    public GameObject door2;

    // Start is called before the first frame update
    void Start()
    {
        currentKillCount = player.GetComponent<Player>().killCounter;
        killsToGet = currentKillCount + 10;
        animator1 = door1.GetComponent<Animator>();
        animator2 = door2.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentKillCount = player.GetComponent<Player>().killCounter;
        if (currentKillCount >= killsToGet)
        {
            Debug.Log("kill count achieved");
            animator1.SetTrigger("DoorOpen");
            animator2.SetTrigger("DoorOpen");
        }
    }
}
