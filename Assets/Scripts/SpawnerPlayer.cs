using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerPlayer : MonoBehaviour
{
    public Transform fireOrientation;
    public KeyCode fireKey;
    public bool isOnPlayer = true;
    public Vector3 mousePos;
    public Vector3 fireDirection;
    public Vector3 mouseCorrectedPos;
    public Vector3 hitPoint;
    public LayerMask isGround;
    public Vector3 newDirection;

    public GameObject instantiatedObject;
    public Transform spawnPoint;

    void Start()
    {
        

        if(this.gameObject.tag != "Player")
        {
            Debug.Log("I am not on a player.");
            isOnPlayer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        mousePos = Input.mousePosition;
        hitPoint.y = transform.position.y;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 1000, isGround))
        {
            hitPoint.x = hit.point.x;
            hitPoint.z = hit.point.z;
        }
        
        Debug.DrawLine(transform.position, hitPoint);

        Vector3 targetDirection = fireOrientation.position - hitPoint;

        newDirection = Vector3.RotateTowards(fireOrientation.forward, targetDirection, 1, 1);

        fireOrientation.rotation = Quaternion.LookRotation(newDirection);




        if (isOnPlayer)
        {
            if (Input.GetKeyDown(fireKey))
            {
                Instantiate(instantiatedObject, spawnPoint.transform);
                Debug.Log("Spawned");
            }
        }

    }
}
