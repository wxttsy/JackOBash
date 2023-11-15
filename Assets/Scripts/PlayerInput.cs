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

    public float horizontalRotation;
    public float verticalRotation;

    public GameObject itemInSlot;

    Vector3 mousePos;
    Vector3 hitPoint;
    LayerMask isGround;
    public Transform fireOrientation;
    Vector3 newDirection;

    public void Start()
    {
        rs = GameObject.Find("RoomManager").GetComponent<RoomSpawner>();
        isGround = LayerMask.GetMask("Ground");
    }

    // Update Input recieved:
    void Update()
    {
        dashButtonPressed = Input.GetButtonDown("Dash");
        attackButtonPressed = Input.GetButtonDown("Attack");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //horizontalRotation = Input.GetAxis("HorizontalRotation");
        //verticalRotation = Input.GetAxis("VerticalRotation");

        mousePos = Input.mousePosition;
        hitPoint.y = transform.position.y;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 1000, isGround))
        {
            hitPoint.x = hit.point.x;
            hitPoint.z = hit.point.z;
        }
        Debug.DrawLine(transform.position, hitPoint);

        Vector3 targetDirection = hitPoint - fireOrientation.position;

        //newDirection = Vector3.RotateTowards(fireOrientation.forward, targetDirection, 1, 1);

        fireOrientation.forward = targetDirection;




        DetermineFloor();
        ItemCheck();
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
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 10))
        {
            //Debug.Log("I am on the ground");
            rs.currentRoom = hit.collider.gameObject.transform.parent.gameObject;
        }
    }


    //check whether the item can be used when a key is pressed. spit out a debug if item is null.
    public void ItemCheck()
    {
        if (Input.GetKeyDown(KeyCode.Q) && itemInSlot != null)
        {
            UseItem();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && itemInSlot == null)
        {
            Debug.Log("I don't hava the item");
        }
    }

    public void UseItem()
    {
        //create the item in respect to player transform
            Instantiate(itemInSlot, this.transform);
            itemInSlot = null;
    }
}