using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkullSpawnerTest : MonoBehaviour
{
    public ChatteringSkulls skullParent;

    // Start is called before the first frame update
    void Start()
    {
        skullParent = GetComponentInParent<ChatteringSkulls>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            skullParent.enemyGrabber = other.GetComponent<Health>();
            skullParent.hitTransform = skullParent.enemyGrabber.GetComponentInParent<Transform>();
            skullParent.SpawnEvent();
        }


    }


}
