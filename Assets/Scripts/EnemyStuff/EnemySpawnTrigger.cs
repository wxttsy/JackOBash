using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public GameObject SpawnController;
    private GameManager gameManager;

    public DoorOpenHandler _doh;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();

        SpawnController.SetActive(false);

        _doh = GetComponent<DoorOpenHandler>();

        if(gameManager.currentRoom != this.gameObject.transform.root.gameObject)
        {
            _doh.enabled = false;
        }
    }

    public void Update()
    {
        if(gameManager.currentRoom == this.transform.parent.root.gameObject)
        {
            SpawnController.SetActive(true);
        }
        else
        {
            SpawnController.SetActive(false);
        }

        if (gameManager.currentRoom == this.gameObject.transform.root.gameObject)
        {
            _doh.enabled = true;
        }
    }

}
