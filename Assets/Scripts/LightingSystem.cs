using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightingSystem : MonoBehaviour
{
    public int timeInRoom;
    public Light[] roomLights;
    public Light nextLevelLight;
    public float timer;
    public bool isInRoom;

    // Start is called before the first frame update
    void Start()
    {
        roomLights = GetComponentsInChildren<Light>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInRoom)
        {
            Destroy(this.gameObject);
        }

        if (isInRoom)
        {
            LightEnableLogic();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isInRoom = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        isInRoom = true;
    }

    public void LightEnableLogic()
    {
        timer += Time.deltaTime;

        if (timer >= timeInRoom)
        {
            nextLevelLight.enabled = true;

        }


    }
}
