using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatteringSkulls : MonoBehaviour
{
    public GameObject target;
    public float rotationSpeed = 10f;
    private float rotation;
    public bool centeredOnPlayer;
    public string enemyTag;

    public Health enemyGrabber;
    public Transform hitTransform;
    public GameObject theBigBang;
    public Collider[] skulls;

    public float timeSinceUsed;
    public int timeOut;
    public ChatteringSkulls originalSkull;
    public string scriptName;
    public bool massDelete;
    public string originalObjectPrefabName;

    // Start is called before the first frame update
    private void Start()
    {
        skulls = GetComponentsInChildren<Collider>();

        if (enemyTag == "Player")
        {
            target = GameObject.FindGameObjectWithTag(enemyTag);
        }
        else
        {

            target = transform.parent.gameObject;
        }

        originalSkull = GameObject.Find(originalObjectPrefabName + "(Clone)").GetComponent<ChatteringSkulls>();

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



            timeSinceUsed += Time.deltaTime;


        if(timeSinceUsed >= timeOut)
        {
            DeleteSkulls();
        }

        if(originalSkull == null && originalSkull.massDelete)
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnEvent()
    {

        if(enemyGrabber.hasSpawnItem != true && theBigBang != null)
        {
            Instantiate(theBigBang, hitTransform);
            enemyGrabber.hasSpawnItem = true;
        }

    }

    public void DeleteSkulls()
    {
        Destroy(this.gameObject);
    }
}
