using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabInstatiation_patches : PrefabInstatiation
{

    private PlantDistribution distribution;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (useSeed)
        { Random.seed = seed; }
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (updateInstantiation)
        {
            if (useSeed)
            { Random.seed = seed; }
        }

        
    }

    public override List<GameObject> procedural_Instantiate(GameObject inGameObject)
    {
        if (inGameObject != null)
        {
            prefab = inGameObject;
        }
        if(createdPrefabs != null)
        {
            createdPrefabs.Clear();
        }

        List<GameObject> childrenToDestroy = new List<GameObject>();
        foreach (Transform child in transform) //this.gameObject.transform)
        {
            childrenToDestroy.Add(child.gameObject);
            //DestroyImmediate(child.gameObject);
            //GameObject.DestroyImmediate(child.gameObject);
        }
        foreach (GameObject o in childrenToDestroy) //this.gameObject.transform)
        {
            GameObject.DestroyImmediate(o);
        }

        getDistributionParameters();

        print("amount of plants to spawn " + distribution.n_plants);


        for(int i = 0; i < distribution.n_plants; i++)
        {
            Vector2 weed_position = distribution.distribution.getNextPosition();
            //Debug.Log("Spawning weed number " + i + " at position " + weed_position);
            //Vector2 weed_position = new Vector2(0,0);
            Vector3 pos = new Vector3(weed_position[0], 0, weed_position[1]);
            GameObject createdPrefab = Instantiate(prefab, pos, Quaternion.Euler(Vector3.zero));
            createdPrefabs.Add(createdPrefab);

            createdPrefab.transform.SetParent(this.gameObject.transform); // = this.transform;
                                                                //prefab.transform.parent = transform;

                                                                    
            if (createdPrefab.GetComponent<SpawnerAndSwitch>())
            {
                createdPrefab.GetComponent<SpawnerAndSwitch>().setPlantScale(Random.Range(-scaleRandomness, scaleRandomness));
                createdPrefab.GetComponent<SpawnerAndSwitch>().Spawn();

            }
            createdPrefab.transform.localScale = Scale;// + newScaleRandomness;
            
        }






        return createdPrefabs;

    }


    void getDistributionParameters()
    {
        // Wait for parameter script to load parameters        

        // Get the spawn parameters
        gameObject.GetComponent<LoadParameters>().Start();

        distribution = gameObject.GetComponent<LoadParameters>().weedDistribution;

        //yield return new WaitUntil(() => gameObject.GetComponent<LoadParameters>().IsLoaded);

        //Debug.Log(distribution.name);
        //Debug.Log(distribution.distribution_type);  

    }




}
