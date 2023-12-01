using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenHandler : MonoBehaviour
{

    // references
    Player _player;
    GameManager _gm;
    [SerializeField] EnemySpawner[] _enSpawns;
    public Animator _anim;

    // values
    int enemiesInRoom;
    public int killsToGet;
    public bool hasOpenedDoor;

    void Start()
    {

        //initialise the door as being closed to ensure that the player is not able to enter
        hasOpenedDoor = false;



        _player = FindObjectOfType<Player>();
        _gm = FindObjectOfType<GameManager>();
        _enSpawns = GetComponentsInChildren<EnemySpawner>();

        if (_gm.currentRoom != this.gameObject.transform.root.gameObject)
        {
            this.enabled = false;
        }


        // get the total number of enemies in the room as an int
        for (int i = 0; i < _enSpawns.Length; i++)
        {
            enemiesInRoom += _enSpawns[i].enemiesToSpawn;
        }

        // feed that number into the current kill count after halving it and casting it as an int. This gives the 
        // number of kills needed for the player to advance to the next room
        killsToGet = _player.killCounter + ((int)(enemiesInRoom/2));
    }

    // Update is called once per frame
    void Update()
    {
        if(_player.killCounter >= killsToGet && !hasOpenedDoor)
        {

            Debug.Log("I settathetrigger");
            _anim.SetTrigger("Open");
            hasOpenedDoor = true;

        }

    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && hasOpenedDoor == true)
        {
            _anim.SetTrigger("Closed");
            hasOpenedDoor = false;
        }
    }
}
