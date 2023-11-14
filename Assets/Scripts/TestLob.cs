using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLob : MonoBehaviour
{
    Vector3 targetPos;
    public float travelSpeed;
    public float arcHeight;
    Vector3 startPos;
    public float maxDistance;
    Vector3 nextpos;

    public bool targetPlayer;
    public Transform playerTarget;

    void Start()
    {
        startPos = transform.position;

        if (transform.parent != null)
        {
            this.gameObject.transform.forward = transform.parent.forward;
        }


        if (targetPlayer)
        {
            playerTarget = FindObjectOfType<PlayerInput>().gameObject.transform;
            targetPos = playerTarget.transform.position;
        }
        else
        {
            targetPos = new Vector3(0, 0, transform.forward.z * maxDistance);

        }


        Vector3 lookDirection = (targetPos - transform.position).normalized;
        transform.forward = lookDirection;

    }

    // Update is called once per frame
    void Update()
    {

        //create float references to initial starting points of object, and where it aims to be, then calculate the distance between those two points
        float startZ = startPos.z;
        float endZ = targetPos.z;
        float dist = endZ - startZ;

        float startX = startPos.x;
        float endX = targetPos.x;
        


        float nextZ = Mathf.MoveTowards(transform.position.z, endZ, travelSpeed * Time.deltaTime);
        float nextX = Mathf.MoveTowards(transform.position.x, endX, travelSpeed * Time.deltaTime);

        float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextZ - startZ) / dist);
        


        float arc = arcHeight * (nextZ - startZ) * (nextZ - endZ) / (-0.25f * dist * dist);


        nextpos = new Vector3(nextX, baseY + arc, nextZ);
        
        transform.position = nextpos;
        
        if(nextpos == targetPos)
        {
            Debug.Log("I hit");
            Destroy(this.gameObject);
        }

        



    }
}
