using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public GameObject SpawnController;
    RoomSpawner rs;
    // Start is called before the first frame update
    void Start()
    {
        SpawnController.SetActive(false);
        rs = FindObjectOfType<RoomSpawner>();
    }

    public void Update()
    {
        if(rs.currentRoom == this.transform.parent.gameObject)
        {
            SpawnController.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SpawnController.gameObject.SetActive(false);
        }
    }
}
