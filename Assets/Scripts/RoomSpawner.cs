using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject currentSpawnedRoom;
    public GameObject currentRoom;
    public int roomCount;
    public int maxRooms;
    public GameObject[] roomsToSpawn;
    public List<GameObject> currentRooms = new List<GameObject> ();
    public float positionOverride = 0;
    public int roomToDelete = 0;

    void Start()
    {

        for (int i = 0; i < maxRooms; i++)
        {
            SpawnRoom();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRoom.gameObject == currentRooms[currentRooms.Count-2].gameObject)
        {
            Destroy(currentRooms[0]);
            currentRooms.Remove(currentRooms[0]);
            SpawnRoom();
        }

    }


    public void SpawnRoom()
    {
        GameObject newRoom = Instantiate(roomsToSpawn[Random.Range(0, roomsToSpawn.Length)], new Vector3 (0, 0, positionOverride), Quaternion.identity);
        currentSpawnedRoom = newRoom;
        positionOverride += currentSpawnedRoom.GetComponentInChildren<Collider>().bounds.size.z;
        currentRooms.Add(newRoom);
        roomCount++;
    }
}
