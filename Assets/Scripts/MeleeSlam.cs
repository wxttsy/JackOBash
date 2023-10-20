using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSlam : MonoBehaviour
{
    // Start is called before the first frame update
    public bool animReady;
    public bool animFinished;
    public SphereCollider sc;

    void Start()
    {
        animReady = false;
        animFinished = false;
        sc = GetComponent<SphereCollider>();
        sc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animReady)
        {
            sc.enabled = true;
        }

        if (animFinished)
        {
            Destroy(this.gameObject);
        }
    }
}
