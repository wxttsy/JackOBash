using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public GameObject item;
    public GameObject ps;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInput>().itemInSlot = item;
            GameObject psOb = Instantiate(ps, null);
            psOb.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
