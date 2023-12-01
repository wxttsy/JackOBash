using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    private GameManager gameManager;
    public SpawnController spawnController;
  
    
    void Awake()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        spawnController = this.gameObject.GetComponentInParent<SpawnController>();

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (gameManager.currentRoom == this.gameObject.transform.parent.root.gameObject)
            {
                spawnController.CalculateEnemiesToSpawn();
                Destroy(this);
            }
        }
    }
}
