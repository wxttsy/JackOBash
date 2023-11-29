using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPopper : MonoBehaviour
{
    BoxCollider _damageCollider;
    public float timeToBeActive;
    public float timeActive;

    void Start()
    {
        transform.parent = FindObjectOfType<PlayerInput>().gameObject.transform;
        _damageCollider = GetComponent<BoxCollider>();
        timeActive = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        timeActive += Time.deltaTime;

        if(timeActive> timeToBeActive)
        {
            DestroyThis();
        }


    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
