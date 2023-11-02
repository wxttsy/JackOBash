using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Input variables:
    public float horizontalInput;
    public float verticalInput;
    public bool attackButtonPressed;
    public bool dashButtonPressed;

    public RoomSpawner rs;
    public int checkDistance;

    public float horizontalRotation;
    public float verticalRotation;

    public void Start()
    {
        rs = GameObject.Find("RoomManager").GetComponent<RoomSpawner>();
    }

    // Update Input recieved:
    void Update()
    {
        dashButtonPressed = Input.GetButtonDown("Dash");
        attackButtonPressed = Input.GetButtonDown("Attack");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalRotation = Input.GetAxis("HorizontalRotation");
        verticalRotation = Input.GetAxis("VerticalRotation");

        DetermineFloor();
    }

    private void OnDisable()
    {
        ClearCache();
    }

    public void ClearCache()
    {
        horizontalInput = 0;
        verticalInput = 0;
        attackButtonPressed = false;
        dashButtonPressed = false;
    }

    public void DetermineFloor()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            rs.currentRoom = hit.collider.gameObject.transform.parent.gameObject;
        }
    }
}