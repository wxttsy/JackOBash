using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public GameObject SpawnController;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();

        SpawnController.SetActive(false);
    }

    public void Update()
    {
        if(gameManager.currentRoom == this.transform.parent.gameObject)
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
