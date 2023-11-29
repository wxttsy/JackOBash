using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    //*******************************************************************************************************************
    //------------------------------------------Initialize Variables-----------------------------------------------------
    //*******************************************************************************************************************
    // Prefabs:
    [Header("____Prefabs____")]
    [Header("****Candy****")]
    public GameObject healthCandy;
    [Header("****Items****")]
    public GameObject[] items;
    public GameObject[] itemsSR;
    [Header("****Effects****")]
    public GameObject hitParticle;
    public GameObject candyPickUpParticle;
    public GameObject dashParticle;
    [Header("____Room Manager____")]
    // References:
    public GameObject[] roomsToSpawn;
    private GameObject _playerObject;
    [HideInInspector] public List<GameObject> currentRooms = new List<GameObject>();

    // Room management Variables:
    private GameObject currentSpawnedRoom;
    public GameObject currentRoom;
    public int maxRooms;
    [HideInInspector] public int roomCount;
    private float positionOverride = 0;

    // NavMesh building:
    private List<NavMeshBuildSource> navMeshDatas = new List<NavMeshBuildSource>();
    private List<NavMeshBuildMarkup> navMeshBuildMarkup = new List<NavMeshBuildMarkup>();
    private NavMeshData nmdata;
    //*******************************************************************************************************************
    //--------------------------------------------------Start-----------------------------------------------------------
    //*******************************************************************************************************************
    void Start() {
        _playerObject = GameObject.FindWithTag("Player");
        for (int i = 0; i < maxRooms; i++) {
            SpawnRoom();
        }

        NavMeshBuilder.CollectSources(null, LayerMask.GetMask("Ground"), NavMeshCollectGeometry.PhysicsColliders, 0, navMeshBuildMarkup, navMeshDatas);
        nmdata = NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByIndex(0), navMeshDatas, new Bounds(Vector3.zero, new Vector3(4096, 4096, 4096)), this.transform.position, this.transform.rotation);
        NavMeshDataInstance d = NavMesh.AddNavMeshData(nmdata);
    }

    //*******************************************************************************************************************
    //--------------------------------------------------Update-----------------------------------------------------------
    //*******************************************************************************************************************
    void Update(){
        RaycastHit hit;
        if (Physics.Raycast(_playerObject.transform.position, Vector3.down, out hit, 10, LayerMask.GetMask("Ground"))){
            currentRoom = hit.collider.gameObject.transform.parent.gameObject;
        }

        if (currentRoom.gameObject == currentRooms[currentRooms.Count - 2].gameObject){
            Destroy(currentRooms[0]);
            currentRooms.Remove(currentRooms[0]);
            SpawnRoom();
            RecreateNavMesh();
        }
    }
    //*******************************************************************************************************************
    //------------------------------------------------Room Manager-------------------------------------------------------
    //*******************************************************************************************************************
    public void SpawnRoom() {
        GameObject newRoom = Instantiate(roomsToSpawn[Random.Range(0, roomsToSpawn.Length)], new Vector3(0, 0, positionOverride), Quaternion.identity);
        currentSpawnedRoom = newRoom;
        positionOverride += currentSpawnedRoom.GetComponentInChildren<Collider>().bounds.size.z;
        currentRooms.Add(newRoom);

        roomCount++;
    }

    public void RecreateNavMesh()
    {
        Debug.Log("Re-create nav mesh for new room.");
        NavMeshBuilder.CollectSources(null, LayerMask.GetMask("Ground"), NavMeshCollectGeometry.PhysicsColliders, 0, navMeshBuildMarkup, navMeshDatas);
        NavMeshBuilder.UpdateNavMeshDataAsync(nmdata, NavMesh.GetSettingsByIndex(0), navMeshDatas, new Bounds(currentRoom.transform.position, new Vector3(512, 512, 512)));
        
        NavMeshDataInstance d = NavMesh.AddNavMeshData(nmdata);
    }
}
