using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [SerializeField] int candyIncreaseValue;
    public GameObject ps;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Combo>().isPrimed)
            {
                other.GetComponent<Combo>().GoGoSugarRush();
                Debug.Log("this is been eatne");
                GameObject psOb = Instantiate(ps, null);
                psOb.transform.position = this.transform.position;
                Destroy(this.gameObject);
            }
            else
            {
                var pHP = other.GetComponent<Health>();
                pHP.currentHealth += candyIncreaseValue;
                Debug.Log("this is been eatne");
                GameObject psOb = Instantiate(ps, null);
                psOb.transform.position = this.transform.position;
                Destroy(this.gameObject);
            }


        }
    }
}
