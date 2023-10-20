using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{ 
    public Transform orientation;
    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        orientation = GetComponent<Transform>();
        gameObject.transform.SetParent(null, true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = orientation.forward * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }
}
