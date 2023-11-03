using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    private List<NavMeshBuildSource> navMeshDatas = new List<NavMeshBuildSource>();
    private List<NavMeshBuildMarkup> navMeshBuildMarkup = new List<NavMeshBuildMarkup>();

    void Start()
    {

        for (int i = 0; i < maxRooms; i++)
        {
            SpawnRoom();
            
        }
        NavMeshBuilder.CollectSources(null, LayerMask.GetMask("Ground"), NavMeshCollectGeometry.PhysicsColliders, 0, navMeshBuildMarkup, navMeshDatas);
        //Debug.Log(navMeshDatas.Count);
        var nmData = NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByIndex(0), navMeshDatas, new Bounds(Vector3.zero, new Vector3(4096, 4096, 4096)), this.transform.position, this.transform.rotation);
        Debug.Log(nmData.sourceBounds);
        NavMeshDataInstance d = NavMesh.AddNavMeshData(nmData);
        Debug.Log(d.valid);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRoom.gameObject == currentRooms[currentRooms.Count-2].gameObject)
        {
            Destroy(currentRooms[0]);
            currentRooms.Remove(currentRooms[0]);
            SpawnRoom();
            //UnityEngine.AI.NavMeshBuilder.UpdateNavMeshDataAsync();
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
