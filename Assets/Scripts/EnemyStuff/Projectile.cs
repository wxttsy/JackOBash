using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject curveprefab;
    
    public float speed;
    public float arcHeight;
    public int projectileDamage;

    private float sampleTime;
    private GameObject curveObject;
    private QuadraticCurve curve;
    private Vector3 originalPostition;
    private void Start()
    {
        transform.parent = null;
        curveObject = Instantiate(curveprefab, transform.parent);
        curve = curveObject.GetComponent<QuadraticCurve>();

        sampleTime = 0f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 a = transform.position;
        Vector3 b = player.transform.position;

        // Set the QuadraticCurve A and B object positions relative to the enemy and player position.
        // Set the control curve to the center between Object A and Object B.
        curve.A.position = a;
        curve.B.position = b;
        curve.control.position = new Vector3((a.x + b.x) / 2, arcHeight, (a.z + b.z) / 2);

        originalPostition = a;
    }

    private void Update() {
        // Move the projectile along the curve.
        sampleTime += Time.deltaTime * speed;
        transform.position = curve.Evaluate(sampleTime);
        transform.forward = curve.Evaluate(sampleTime + 0.001f) - originalPostition;

        // Destroy this projectile after a certian amount of time.
        if (sampleTime >= 1f) {
            Destroy(curveObject);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //If we are colliding with the player - deal damage and destroy this projectile.
        GameObject player = GameObject.FindWithTag("Player");
        if (other.tag == player.tag)
        {
            Player playerScript = player.GetComponent<Player>();
            if (!playerScript.sugarRushIsActivated)
            {
                Health playerHealthScript = other.GetComponent<Health>();
                playerHealthScript.ApplyDamage(projectileDamage);
            }
            Destroy(curveObject);
            Destroy(gameObject);
        }
    }
}
