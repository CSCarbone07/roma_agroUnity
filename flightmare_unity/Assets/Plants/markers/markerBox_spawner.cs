using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markerBox_spawner : SpawnerAndSwitch
{

    public GameObject[] spawnPoints;
    public GameObject[] markers;

    public GameObject box;
    private GameObject spawnedMarker;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Spawn()
    {
        /*
        float Density = 75;
        if ((Density / 100) >= Random.Range(0.0f, 1.0f))
        {

        }
        */

        spawnedMarker = Instantiate(markers[0], spawnPoints[0].transform.position, spawnPoints[0].transform.rotation, this.gameObject.transform);
        spawnedMarker.transform.localScale = spawnPoints[0].transform.localScale;
        //createdPrefab.transform.SetParent(this.gameObject.transform); // = this.transform;

        spawnedMarker.transform.Rotate(0, 90 * Random.Range(0, 4), 0, Space.Self);
        box.transform.Rotate(0, 90 * Random.Range(0, 4), 0, Space.Self);
    }

    public override void Unspawn()
    {
        if(spawnedMarker != null)
        {
            Destroy(spawnedMarker);
        }
    }



}
