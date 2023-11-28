using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpilledPunch : MonoBehaviour
{
    private GameObject target;
    public float rotationSpeed = 1f;
    private float rotation;

    public float fireRate = 0.8f;
    public GameObject launcher;
    public GameObject pumpkinBullet;
    public float timer = 0;
    public float timerMax = 100;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (target != null)
        {
            rotation += rotationSpeed;
            launcher.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        
            timer += 1;
            if (timer >= timerMax)
            {
                FirePumpkin(pumpkinBullet, launcher.transform.position);
                timer = 0;
            }
        }

        
    }

    private void FirePumpkin(GameObject bullet, Vector3 _position)
    {

        GameObject pumpkinBullet = Instantiate(bullet, _position, Quaternion.identity);
        pumpkinBullet.transform.rotation = transform.rotation;
        pumpkinBullet.GetComponent<Rigidbody>().AddForce(launcher.transform.up * 500, ForceMode.Force);
        pumpkinBullet.GetComponent<Rigidbody>().AddForce(launcher.transform.forward * 250, ForceMode.Force);

    }
}
