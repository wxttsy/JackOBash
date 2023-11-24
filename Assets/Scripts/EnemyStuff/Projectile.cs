using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public QuadraticCurve curve;
    public float speed;
    public float arcHeight;
    public int projectileDamage;

    private float sampleTime;

    private void Start()
    {
        sampleTime = 0f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 a = transform.position;
        Vector3 b = player.transform.position;

        // Set the QuadraticCurve A and B object positions relative to the enemy and player position.
        // Set the control curve to the center between Object A and Object B.
        curve.A.position = a;
        curve.B.position = b;
        curve.control.position = new Vector3((a.x + b.x) / 2, arcHeight, (a.z + b.z) / 2);
    }

    private void Update() {
        // Move the projectile along the curve.
        sampleTime += Time.deltaTime * speed;
        transform.position = curve.Evaluate(sampleTime);
        transform.forward = curve.Evaluate(sampleTime + 0.001f) - transform.position;

        // Destroy this projectile after a certian amount of time.
        if (sampleTime >= 1f) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //If we are colliding with the player - deal damage and destroy this projectile.
        GameObject player = GameObject.FindWithTag("Player");
        if (other.tag == player.tag)
        {
            Health playerHealthScript = GetComponent<Health>();
            playerHealthScript.currentHealth -= projectileDamage;
            Destroy(gameObject);
        }
    }
}
