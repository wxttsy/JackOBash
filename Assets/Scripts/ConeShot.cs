using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConeShot : MonoBehaviour
{
   
        public float angle = 45f; // Cone angle in degrees
        public float range = 5f; // Cone range
        public int damage = 10;
        public Transform coneOrigin; // The point from which the cone originates

        void Update()
        {
            // Draw the cone shape
            DrawCone(coneOrigin.position, coneOrigin.forward, angle, range, Color.red);
        if (Input.GetMouseButtonDown(0))
        {
            ShootCone();
        }
        }

        void DrawCone(Vector3 origin, Vector3 direction, float coneAngle, float coneRange, Color color)
        {
            // Calculate the half-angle in radians
            float halfAngle = coneAngle / 2f;
            float halfAngleRad = halfAngle * Mathf.Deg2Rad;

            // Calculate the end points of the cone
            Vector3 endpoint1 = origin + direction * coneRange;
            Vector3 endpoint2 = origin + Quaternion.Euler(0, halfAngle, 0) * direction * coneRange;
            Vector3 endpoint3 = origin + Quaternion.Euler(0, -halfAngle, 0) * direction * coneRange;

            // Draw the lines representing the cone
            Debug.DrawLine(origin, endpoint1, color);
            Debug.DrawLine(origin, endpoint2, color);
            Debug.DrawLine(origin, endpoint3, color);
        }

    void ShootCone()
    {
        // Detect enemies within the cone
        Collider[] hitColliders = Physics.OverlapCapsule(transform.position, transform.position + transform.forward * range, transform.lossyScale.x / 2f);

        foreach (Collider hitCollider in hitColliders)
        {
            
            // Check if the hit object is an enemy (you can tag your enemies for this)
            if (hitCollider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit");
                // Check if the enemy is within the cone angle
                Vector3 directionToEnemy = hitCollider.transform.position - transform.position;
                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                if (angleToEnemy <= angle / 2)
                {
                    // Apply damage to the enemy
                    hitCollider.gameObject.GetComponent<Health>().ApplyDamage(damage);
                   
                       
                    
                }
            }
        }
    }
}

