using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lob : MonoBehaviour
{
    public Transform targetPos;
    Vector3 startPosVector;
    Vector3 targetPosVector;
    public float travelSpeed;
    public float arcHeight;
    public Transform startPos;
    public float maxDistance;
    Vector3 nextpos;
    float sampleTime = 0;
    public bool targetPlayer;
    public Transform playerTarget;
    public GameObject dorb;
    public Vector3 midPointPos;

    void Start()
    {
        startPos = transform;
        startPosVector = startPos.position;

        if (transform.parent != null)
        {
            this.gameObject.transform.forward = transform.parent.forward;
            transform.parent = null;
        }


        if (targetPlayer)
        {
            playerTarget = FindObjectOfType<PlayerInput>().gameObject.transform;
            targetPos = playerTarget.transform;
            targetPosVector = targetPos.position;
        }
        //else
        //{
        //    targetPos = new Vector3(0, 0, transform.forward.z * maxDistance);
        //
        //}


        midPointPos = new Vector3((startPos.position.x + targetPos.position.x) / 2, 0, (startPos.position.z + targetPos.position.z) / 2);
        midPointPos.y = arcHeight;

    }

    // Update is called once per frame
    void Update()
    {

        //create float references to initial starting points of object, and where it aims to be, then calculate the distance between those two points



        sampleTime += travelSpeed * Time.deltaTime;
        transform.position = evaluate(sampleTime);
        transform.forward = evaluate(sampleTime * .001f) - transform.position;


        if (sampleTime > 1f)
        {
            GameObject ddorb = Instantiate(dorb, this.transform);
            ddorb.transform.parent = null;
            Destroy(this.gameObject);
        }

    }

    public Vector3 evaluate(float t)
    {

        //midPointPos = new Vector3((startPos.position.x + targetPos.position.x) / 2, 0, (startPos.position.z + targetPos.position.z) / 2);
        //midPointPos.y = arcHeight;


        Vector3 ac = Vector3.Lerp(startPosVector, midPointPos, t);
        Vector3 cb = Vector3.Lerp(midPointPos, targetPosVector, t);
        return Vector3.Lerp(ac, cb, t);
    }


    private void OnDrawGizmos()
    {



        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(evaluate(i / 20f), 0.1f);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject ddorb = Instantiate(dorb, this.transform);
            ddorb.transform.parent = null;
            Destroy(this.gameObject);
        }
    }


}
