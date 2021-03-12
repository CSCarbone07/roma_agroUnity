using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;

public class Potato_Spawner : SpawnerAndSwitch
{
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

    public int maxStems = 7;
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
    public List<GameObject> plantComponents;

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

    [ExecuteInEditMode]
    public override void Spawn()
    {


        foreach (Transform child in transform) //this.gameObject.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
            //GameObject.Destroy(child.gameObject);
        }

        //Debug.Log("I am a Good Plant");
        //GameObject createdPrefabStem = new GameObject();
        createdPrefabStems.Clear();
        createdPrefabStemsType.Clear();

        // Stem Spawn
        Vector3 tempPosition = this.transform.position;
        tempPosition[1] += 0.8f;


        for (int x = 0; x < maxStems; x++)
        {
            if (UnityEngine.Random.Range(0f, 10f) < 9.5f)
            {

                float randomPosX = Random.Range(-stemPositionNoise.x, stemPositionNoise.x);
                float randomPosY = Random.Range(-stemPositionNoise.y, stemPositionNoise.y);
                float randomPosZ = Random.Range(-stemPositionNoise.z, stemPositionNoise.z);
                Vector3 randomPos = new Vector3(randomPosX, randomPosY, randomPosZ);

                //Debug.Log("spawning leaf: " + x);
                randomRotationValue = new Vector3(0f, x * 360.0f/maxStems, 0f);
                stemRotation[1] += UnityEngine.Random.Range(-15.0f, 15.0f);
                //beetLeafRotation[0] += UnityEngine.Random.Range(-5.0f, 5.0f);
                newRotation = Quaternion.Euler(stemRotation + randomRotationValue);
                GameObject createdPrefabStem;

                int typeOfStem = Random.Range(0, stems.Length - 1);
                createdPrefabStemsType.Add(typeOfStem);
                createdPrefabStem = Instantiate(stems[typeOfStem], tempPosition + randomPos, newRotation);

                createdPrefabStem.transform.localScale = stemScale * UnityEngine.Random.Range(0.5f, 1f);


                createdPrefabStem.isStatic = true;
                createdPrefabStem.SetActive(true);

                createdPrefabStem.transform.SetParent(this.transform);
                createdPrefabStems.Add(createdPrefabStem);
                plantComponents.Add(createdPrefabStem);
            }
        }


        createdPrefabStemLeaves.Clear();
        createdPrefabStemLeavesType.Clear();
        createdPrefabHeadLeaves.Clear();
        createdPrefabHeadLeavesType.Clear();


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
                    createdPrefabStemLeavesType.Add(typeOfLeaf);
                    createdPrefabLeaf = Instantiate(stemLeaves[typeOfLeaf], s.transform.GetChild(l).position, newRotation);
                    createdPrefabStemLeaves.Add(createdPrefabLeaf);
                    createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.5f, 1f);
                    createdPrefabLeaf.transform.SetParent(this.transform);
                    plantComponents.Add(createdPrefabLeaf);


                    randomOffset = UnityEngine.Random.Range(0.0f, 45.0f);
                    flipRotation = new Vector3(0, 0 + randomOffset, 0);
                    newRotation = Quaternion.Euler(leafRotation + flipRotation + s.transform.GetChild(l).rotation.eulerAngles);


                    typeOfLeaf = Random.Range(0, stemLeaves.Length);
                    createdPrefabStemLeavesType.Add(typeOfLeaf);
                    createdPrefabLeaf = Instantiate(stemLeaves[typeOfLeaf], s.transform.GetChild(l).position, newRotation);
                    createdPrefabStemLeaves.Add(createdPrefabLeaf);
                    createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.5f, 1f);
                    createdPrefabLeaf.transform.localScale = new Vector3(createdPrefabLeaf.transform.localScale.x, createdPrefabLeaf.transform.localScale.y * UnityEngine.Random.Range(0.75f, 1f), createdPrefabLeaf.transform.localScale.z);
                    createdPrefabLeaf.transform.SetParent(this.transform);
                    plantComponents.Add(createdPrefabLeaf);

                }
            }

            // HEAD part
            flipRotation = new Vector3(0, -45, 0);
            newRotation = Quaternion.Euler(headRotation + flipRotation + s.transform.GetChild(s.transform.childCount - 2).rotation.eulerAngles);

            typeOfLeaf = Random.Range(0, headLeaves.Length);
            createdPrefabHeadLeavesType.Add(typeOfLeaf);
            createdPrefabLeaf = Instantiate(headLeaves[typeOfLeaf], s.transform.GetChild(s.transform.childCount - 2).position, newRotation);
            createdPrefabHeadLeaves.Add(createdPrefabLeaf);
            createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.75f, 1.25f);
            createdPrefabLeaf.transform.SetParent(this.transform);

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
        if (createdPrefabStems.Length > 0)
        {
            for (int x = 0; x < maxStems; x++)
            {
                GameObject createdPrefabStem = Instantiate(beetLeaf[createdPrefabStemsType[x]], createdPrefabStems[x].transform.position, createdPrefabStems[x].transform.rotation);
                //(beetLeaf_NIR[createdPrefabStemsType[x]], createdPrefabStems[x]);
                createdPrefabStem.transform.SetParent(this.transform);
                createdPrefabStem.transform.localScale = createdPrefabStems[x].transform.localScale;
                Destroy(createdPrefabStems[x]);
                createdPrefabStems[x] = createdPrefabStem;
                //Debug.Log(createdPrefabStemsType[x]);
            }
        }
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
        /*
        if (createdPrefabStems.Count > 0)
        {
            for (int x = 0; x < createdPrefabStems.Count; x++)
            {
                GameObject createdPrefabStem = Instantiate(stems_TAG[createdPrefabStemsType[x]], createdPrefabStems[x].transform.position, createdPrefabStems[x].transform.rotation);
                //(beetLeaf_NIR[createdPrefabStemsType[x]], createdPrefabStems[x]);
                createdPrefabStem.transform.SetParent(this.transform);
                createdPrefabStem.transform.localScale = createdPrefabStems[x].transform.localScale;
                Destroy(createdPrefabStems[x]);
                createdPrefabStems[x] = createdPrefabStem;
                //Debug.Log(createdPrefabStemsType[x]);
            }
        }
        */
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
