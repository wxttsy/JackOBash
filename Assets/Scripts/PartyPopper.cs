using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPopper : MonoBehaviour
{
    EffectsManager em;
    float timer;
    public int deleteTime;

    void Start()
    {
        em = FindObjectOfType<EffectsManager>();
        Instantiate(em.candyPickUpParticle, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= deleteTime)
        {
            Destroy(this.gameObject);
        }
    }
}
