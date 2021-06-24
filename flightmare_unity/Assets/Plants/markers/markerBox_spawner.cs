using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markerBox_spawner : SpawnerAndSwitch
{

    public GameObject[] spawnPoints;
    public GameObject[] markers;


    // Start is called before the first frame update
    void Start()
    {
        float Density = 75;
        if ((Density / 100) >= Random.Range(0.0f, 1.0f))
        {

            GameObject createdPrefab = Instantiate(markers[0], spawnPoints[0].transform.position, spawnPoints[0].transform.rotation, this.gameObject.transform);
            createdPrefab.transform.localScale = spawnPoints[0].transform.localScale;
            //createdPrefab.transform.SetParent(this.gameObject.transform); // = this.transform;

        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
