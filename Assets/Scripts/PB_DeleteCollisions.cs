using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_DeleteCollisions : MonoBehaviour
{
    public GameObject collision; 
    public int timer;
    public int timerMax = 400; 
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (timer > timerMax)
        {
            Destroy(this.gameObject);
        }
        timer++; 
    }
}
