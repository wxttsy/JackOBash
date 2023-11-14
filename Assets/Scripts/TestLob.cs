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

    void Start()
    {
        startPos = transform.position;

        if (transform.parent != null)
        {
            this.gameObject.transform.forward = transform.parent.forward;
        }

        targetPos = new Vector3(0, 0, transform.forward.z * maxDistance);


    }

    // Update is called once per frame
    void Update()
    {

        //create float references to initial starting points of object, and where it aims to be, then calculate the distance between those two points
        float startZ = startPos.z;
        float endZ = targetPos.z;
        float dist = endZ - startZ;
        

        //
        float nextZ = Mathf.MoveTowards(transform.position.z, endZ, travelSpeed * Time.deltaTime);


        float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextZ - startZ) / dist);
        


        float arc = arcHeight * (nextZ - startZ) * (nextZ - endZ) / (-0.25f * dist * dist);


        nextpos = new Vector3(transform.position.x, baseY + arc, nextZ);
        
        transform.position = nextpos;
        
        if(nextpos == targetPos)
        {
            Debug.Log("I hit");
            Destroy(this.gameObject);
        }

        



    }
}
