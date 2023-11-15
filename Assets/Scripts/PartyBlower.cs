using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBlower : MonoBehaviour
{
    private GameObject target;
    public GameObject damageCollision;
    public int blowLength = 4;
    Vector3 nextSpawnPosition;
    int distanceBetweenCol = 4;
    int currentIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
        nextSpawnPosition = transform.position;
        //nextSpawnPosition.z += distanceBetweenCol;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
        if (Input.GetKeyDown(KeyCode.P))
        {
            startBlow();
        }
        
    }

    void startBlow()
    {
        
        
        if (currentIndex != blowLength)
        {
            Instantiate(damageCollision, nextSpawnPosition, Quaternion.identity);
            
            nextSpawnPosition += transform.right;
            currentIndex++;
        }
        
    }
}
