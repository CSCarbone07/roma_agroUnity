using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;

public class Potato_Spawner : SpawnerAndSwitch
{
    private bool DEBUG_ALL = false;
    
    public bool SpawnOnStart = false;

    public Material TestMaterial;

    public GameObject[] stems;
    public GameObject[] stems_NIR;
    public GameObject[] stems_TAG;


    public GameObject[] headLeaves;
    public GameObject[] headLeaves_NIR;
    public GameObject[] headLeaves_TAG;


    public GameObject[] stemLeaves;
    public GameObject[] stemLeaves_NIR;
    public GameObject[] stemLeaves_TAG;

    public int minStems = 7;
    public int maxStems = 12;
    private int totalStems = 7;
    public Vector3 stemPositionNoise = new Vector3(0, 0, 0);
    public Vector3 stemScale = new Vector3(1, 1, 1);
    public Vector3 leafRotation = new Vector3(0, 0, 0);
    public Vector3 leafScale = new Vector3(1, 1, 1);
    public Vector3 headRotation = new Vector3(0, 0, 0);




    [HideInInspector]
    public List<GameObject> createdPrefabStems;
    [HideInInspector]
    public List<int> createdPrefabStemsType;
    [HideInInspector]
    public List<GameObject> createdPrefabStemLeaves;
    [HideInInspector]
    public List<int> createdPrefabStemLeavesType;
    [HideInInspector]
    public List<GameObject> createdPrefabHeadLeaves;
    [HideInInspector]
    public List<int> createdPrefabHeadLeavesType;

    [HideInInspector]
    public List<GameObject> plantComponents;// = new List<GameObject>;

    protected Vector3 randomRotationValue;
    protected Quaternion newRotation;
    private Vector3 stemRotation = new Vector3(0f, 0f, 0f);


    private bool didSpawnedOnce = false;

    // Start is called before the first frame update
    public override void Start()
    {


        //base.Start();
        if (SpawnOnStart)
        { Spawn(); }

    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void setPlantScale(float inScale)
    {
	if(DEBUG_ALL)
	{
	  print("set plant scale " + inScale);
	}

        stemScale.x += inScale;
        stemScale.y += inScale;
        stemScale.z += inScale;
        
        leafScale.x += (inScale * leafScale.x/stemScale.x);
        leafScale.y += (inScale * leafScale.y/stemScale.y);
        leafScale.z += (inScale * leafScale.z/stemScale.z);
    }


    [ExecuteInEditMode]
    public override void Spawn()
    {
        totalStems = UnityEngine.Random.Range(minStems,maxStems);

	if(DEBUG_ALL)
	{
	  print("total stems " + totalStems);
	}

        foreach (Transform child in transform) //this.gameObject.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
            //GameObject.Destroy(child.gameObject);
        }

        //Debug.Log("I am a Good Plant");
        //GameObject createdPrefabStem = new GameObject();
        createdPrefabStems.Clear();
        createdPrefabStemsType.Clear();
        createdPrefabStemLeaves.Clear();
        createdPrefabStemLeavesType.Clear();
        createdPrefabHeadLeaves.Clear();
        createdPrefabHeadLeavesType.Clear();
        plantComponents.Clear();

        // Stem Spawn
        Vector3 tempPosition = this.transform.position;
        tempPosition[1] += 0.8f;


        for (int x = 0; x < totalStems; x++)
        {
            if (UnityEngine.Random.Range(0f, 10f) < 9.5f)
            {

                float randomPosX = Random.Range(-stemPositionNoise.x, stemPositionNoise.x);
                float randomPosY = Random.Range(-stemPositionNoise.y, stemPositionNoise.y);
                float randomPosZ = Random.Range(-stemPositionNoise.z, stemPositionNoise.z);
                Vector3 randomPos = new Vector3(randomPosX, randomPosY, randomPosZ);

                //Debug.Log("spawning leaf: " + x);
                randomRotationValue = new Vector3(0f, x * 360.0f/totalStems, 0f);
                stemRotation[1] += UnityEngine.Random.Range(-15.0f, 15.0f);
                //beetLeafRotation[0] += UnityEngine.Random.Range(-5.0f, 5.0f);
                newRotation = Quaternion.Euler(stemRotation + randomRotationValue);
                GameObject createdPrefabStem;

                int typeOfStem = Random.Range(0, stems.Length - 1);
                createdPrefabStemsType.Add(typeOfStem);
                createdPrefabStem = Instantiate(stems[typeOfStem], tempPosition + randomPos, newRotation);

                createdPrefabStem.transform.localScale = stemScale * UnityEngine.Random.Range(0.5f, 1f);
                //createdPrefabStem.isStatic = true;
                //createdPrefabStem.SetActive(true);
                createdPrefabStem.transform.SetParent(this.transform);

                createdPrefabStems.Add(createdPrefabStem);
                createdPrefabStemsType.Add(typeOfStem);
                plantComponents.Add(createdPrefabStem);
            }
        }

        foreach (GameObject s in createdPrefabStems)
        {
            int typeOfLeaf;
            GameObject createdPrefabLeaf;
            Vector3 flipRotation;

            for (int l = 1; l < s.transform.childCount - 2; l++)
            {
                if (UnityEngine.Random.Range(0f, 10f) < 9.5f)
                {
                    float randomOffset = UnityEngine.Random.Range(-45.0f, 0.0f);

                    flipRotation = new Vector3(0, -90 + randomOffset, 0);
                    newRotation = Quaternion.Euler(leafRotation + flipRotation + s.transform.GetChild(l).rotation.eulerAngles);


                    typeOfLeaf = Random.Range(0, stemLeaves.Length);
                    createdPrefabLeaf = Instantiate(stemLeaves[typeOfLeaf], s.transform.GetChild(l).position, newRotation);
                    createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.5f, 1f);
                    createdPrefabLeaf.transform.SetParent(this.transform);

                    createdPrefabStemLeaves.Add(createdPrefabLeaf);
                    createdPrefabStemLeavesType.Add(typeOfLeaf);
                    plantComponents.Add(createdPrefabLeaf);


                    randomOffset = UnityEngine.Random.Range(0.0f, 45.0f);
                    flipRotation = new Vector3(0, 0 + randomOffset, 0);
                    newRotation = Quaternion.Euler(leafRotation + flipRotation + s.transform.GetChild(l).rotation.eulerAngles);


                    typeOfLeaf = Random.Range(0, stemLeaves.Length);
                    createdPrefabLeaf = Instantiate(stemLeaves[typeOfLeaf], s.transform.GetChild(l).position, newRotation);
                    createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.5f, 1f);
                    createdPrefabLeaf.transform.localScale = new Vector3(createdPrefabLeaf.transform.localScale.x, createdPrefabLeaf.transform.localScale.y * UnityEngine.Random.Range(0.75f, 1f), createdPrefabLeaf.transform.localScale.z);
                    createdPrefabLeaf.transform.SetParent(this.transform);

                    createdPrefabStemLeaves.Add(createdPrefabLeaf);
                    createdPrefabStemLeavesType.Add(typeOfLeaf);
                    plantComponents.Add(createdPrefabLeaf);

                }
            }

            // HEAD part
            flipRotation = new Vector3(0, -45, 0);
            newRotation = Quaternion.Euler(headRotation + flipRotation + s.transform.GetChild(s.transform.childCount - 2).rotation.eulerAngles);

            typeOfLeaf = Random.Range(0, headLeaves.Length);
            createdPrefabLeaf = Instantiate(headLeaves[typeOfLeaf], s.transform.GetChild(s.transform.childCount - 2).position, newRotation);
            createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.75f, 1.25f);
            createdPrefabLeaf.transform.SetParent(this.transform);

            createdPrefabHeadLeaves.Add(createdPrefabLeaf);
            createdPrefabHeadLeavesType.Add(typeOfLeaf);
            plantComponents.Add(createdPrefabLeaf);

        }






        if (didSpawnedOnce == false)
        { didSpawnedOnce = true; }

        base.Spawn();


    }

    public override void SwitchToRGB()
    {
        base.SwitchToRGB();
        /*
        List<GameObject> newPlantStems = new List<GameObject>();
        List<GameObject> newPlantStemLeaves = new List<GameObject>();
        List<GameObject> newPlantHeadLeaves = new List<GameObject>();
        */
        if (createdPrefabStems.Count > 0)
        {
            for (int x = 0; x < createdPrefabStems.Count; x++)
            {
            GameObject createdPrefab = Instantiate(stems[createdPrefabStemsType[x]], createdPrefabStems[x].transform.position, createdPrefabStems[x].transform.rotation);
            createdPrefab.transform.SetParent(this.transform);
            createdPrefab.transform.localScale = createdPrefabStems[x].transform.localScale;
            Destroy(createdPrefabStems[x]);
            createdPrefabStems[x] = createdPrefab;
            //Debug.Log(createdPrefabStemsType[x]); 
            }
        }

        if (createdPrefabStemLeaves.Count > 0)
        {
            for (int x = 0; x < createdPrefabStemLeaves.Count; x++)
            {
            GameObject createdPrefab = Instantiate(stemLeaves[createdPrefabStemLeavesType[x]], createdPrefabStemLeaves[x].transform.position, createdPrefabStemLeaves[x].transform.rotation);
            createdPrefab.transform.SetParent(this.transform);
            createdPrefab.transform.localScale = createdPrefabStemLeaves[x].transform.localScale;
            Destroy(createdPrefabStemLeaves[x]);
            createdPrefabStemLeaves[x] = createdPrefab;
            //Debug.Log(createdPrefabStemsType[x]); 
            }
        }

        if (createdPrefabHeadLeaves.Count > 0)
        {
            for (int x = 0; x < createdPrefabHeadLeaves.Count; x++)
            {
            GameObject createdPrefab = Instantiate(headLeaves[createdPrefabHeadLeavesType[x]], createdPrefabHeadLeaves[x].transform.position, createdPrefabHeadLeaves[x].transform.rotation);
            createdPrefab.transform.SetParent(this.transform);
            createdPrefab.transform.localScale = createdPrefabHeadLeaves[x].transform.localScale;
            Destroy(createdPrefabHeadLeaves[x]);
            createdPrefabHeadLeaves[x] = createdPrefab;
            //Debug.Log(createdPrefabStemsType[x]); 
            }
        }

        /*        
        createdPrefabStem.Clear();
        createdPrefabStem = null;
        createdPrefabStem = newPlantStems;

        createdPrefabStemLeaves.Clear();
        createdPrefabStemLeaves = null;;
        createdPrefabStemLeaves = newPlantStemLeaves;

        createdPrefabHeadLeaves.Clear();
        createdPrefabHeadLeaves = null;;
        createdPrefabHeadLeaves = newPlantHeadLeaves;
        */
    }
    /*
    public override void SwitchToNIR()
    {
        base.SwitchToNIR();

        if (createdPrefabStems.Length > 0)
        {
            for (int x = 0; x < maxStems; x++)
            {
                GameObject createdPrefabStem = Instantiate(beetLeaf_NIR[createdPrefabStemsType[x]], createdPrefabStems[x].transform.position, createdPrefabStems[x].transform.rotation);
                //(beetLeaf_NIR[createdPrefabStemsType[x]], createdPrefabStems[x]);
                createdPrefabStem.transform.SetParent(this.transform);
                createdPrefabStem.transform.localScale = createdPrefabStems[x].transform.localScale;
                Destroy(createdPrefabStems[x]);
                createdPrefabStems[x] = createdPrefabStem;
                //Debug.Log(createdPrefabStemsType[x]);
                //print(x);
            }
        }

    }
    */
    public override void SwitchToTAG()
    {
        base.SwitchToTAG();

        if (createdPrefabStems.Count > 0)
        {
            for (int x = 0; x < createdPrefabStems.Count; x++)
            {
            GameObject createdPrefab = Instantiate(stems_TAG[createdPrefabStemsType[x]], createdPrefabStems[x].transform.position, createdPrefabStems[x].transform.rotation);
            createdPrefab.transform.SetParent(this.transform);
            createdPrefab.transform.localScale = createdPrefabStems[x].transform.localScale;
            Destroy(createdPrefabStems[x]);
            createdPrefabStems[x] = createdPrefab;
            //Debug.Log(createdPrefabStemsType[x]); 
            }
        }

        if (createdPrefabStemLeaves.Count > 0)
        {
            for (int x = 0; x < createdPrefabStemLeaves.Count; x++)
            {
            GameObject createdPrefab = Instantiate(stemLeaves_TAG[createdPrefabStemLeavesType[x]], createdPrefabStemLeaves[x].transform.position, createdPrefabStemLeaves[x].transform.rotation);
            createdPrefab.transform.SetParent(this.transform);
            createdPrefab.transform.localScale = createdPrefabStemLeaves[x].transform.localScale;
            Destroy(createdPrefabStemLeaves[x]);
            createdPrefabStemLeaves[x] = createdPrefab;
            //Debug.Log(createdPrefabStemsType[x]); 
            }
        }

        if (createdPrefabHeadLeaves.Count > 0)
        {
            for (int x = 0; x < createdPrefabHeadLeaves.Count; x++)
            {
            GameObject createdPrefab = Instantiate(headLeaves_TAG[createdPrefabHeadLeavesType[x]], createdPrefabHeadLeaves[x].transform.position, createdPrefabHeadLeaves[x].transform.rotation);
            createdPrefab.transform.SetParent(this.transform);
            createdPrefab.transform.localScale = createdPrefabHeadLeaves[x].transform.localScale;
            Destroy(createdPrefabHeadLeaves[x]);
            createdPrefabHeadLeaves[x] = createdPrefab;
            //Debug.Log(createdPrefabStemsType[x]); 
            }
        }


    }
    /*
    public void getLeaves()
    {
        print("types of leaves");
        //print(this.transform.GetChilds);
        //print(createdPrefabStemsType[0]);
        foreach (Transform child in transform)
        {
            print(beetLeaf[1].name);
            print(child.name);
            print(child.GetType());
            print(createdPrefabStems[1]);

            //if (child == beetLeaf[1])


        }
        didSpawnedOnce = true; // used to have the right reference
    }
    */
}
