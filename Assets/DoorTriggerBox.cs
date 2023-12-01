using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerBox : MonoBehaviour
{
    Animator animator1;
    Animator animator2;
    public GameObject door1;
    public GameObject door2; 
    // Start is called before the first frame update
    void Start()
    {
        animator1 = door1.GetComponent<Animator>();
        animator2 = door2.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (tag == "Player")
        {
            animator1.SetTrigger("DoorClose");
            animator2.SetTrigger("DoorClose");
        }
    }
}
