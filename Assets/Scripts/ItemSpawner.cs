using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject candy;
    public GameObject[] spawnedItem;
    public Combo ct;
    public Transform spawnTransform;


    void Start()
    {
        ct = FindObjectOfType<Combo>();
    }

    public void SpawnCandy()
    {
        GameObject spCan = Instantiate(candy, null);
        spCan.transform.position = spawnTransform.position;
        


    }

    public void SpawnItem()
    {
        GameObject spIt = Instantiate(spawnedItem[Random.Range(0, spawnedItem.Length - 1)]);
        spIt.transform.position = spawnTransform.position;
    }
}
