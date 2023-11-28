using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatteringSkulls : MonoBehaviour
{
    private GameObject target;
    public float rotationSpeed = 10f;
    private float rotation;

    // Start is called before the first frame update
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            // Rotate skulls
            rotation += rotationSpeed * Time.deltaTime;
            transform.position = target.transform.position;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
    }
}
