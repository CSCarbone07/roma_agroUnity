using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Potato_Spawner : SpawnerAndSwitch
{
    public bool SpawnOnStart = false;

    public Material TestMaterial;

    public GameObject[] topLeaf;
    public GameObject[] topLeaf_NIR;
    public GameObject[] topLeaf_TAG;

    public GameObject[] stemLeaf;
    public GameObject[] stemLeaf_NIR;
    public GameObject[] stemLeaf_TAG;

    public int leafLevels = 3;
    public Vector3 beetLeafScale = new Vector3(1, 1, 1);

    [HideInInspector]
    public GameObject[] createdPrefabLeaves;
    [HideInInspector]
    public int[] createdPrefabLeavesType;


    protected Vector3 randomRotationValue;
    protected Quaternion newRotation;
    public Vector3 beetLeafRotation = new Vector3(200f, 0f, -90f);
    public Renderer[] rend;
    public Renderer thisRend;

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

    public override void Spawn()
    {

        foreach (Transform child in transform) //this.gameObject.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }

        //Debug.Log("I am a Good Plant");
        //GameObject createdPrefabStem = new GameObject();
        createdPrefabLeaves = new GameObject[maxBeetLeafAmount];
        createdPrefabLeavesType = new int[maxBeetLeafAmount];

        // Leaf Spawn
        Vector3 tempPosition = this.transform.position;
        tempPosition[1] += 0.8f;

        for (int x = 0; x < maxBeetLeafAmount; x++)
        {
            //Debug.Log("spawning leaf: " + x);


            randomRotationValue = new Vector3(0f, x * 50f, 0f);
            beetLeafRotation[1] += UnityEngine.Random.Range(-16.0f, 16.0f);
            //beetLeafRotation[0] += UnityEngine.Random.Range(-5.0f, 5.0f);
            newRotation = Quaternion.Euler(beetLeafRotation + randomRotationValue);
            GameObject createdPrefabLeaf;

            int typeOfLeaf = Random.Range(0, beetLeaf.Length - 1);
            createdPrefabLeavesType[x] = typeOfLeaf;
            createdPrefabLeaf = Instantiate(beetLeaf[typeOfLeaf], tempPosition, newRotation);

            if (UnityEngine.Random.Range(0f, 10f) < 9.5f)
            {
                createdPrefabLeaf.transform.localScale = beetLeafScale * UnityEngine.Random.Range(0.5f, 1f);
            }
            else
            {
                createdPrefabLeaf.transform.localScale = beetLeafScale * 0;
            }
            createdPrefabLeaf.isStatic = true;
            createdPrefabLeaf.SetActive(true);
            //createdPrefabLeaf.AddComponent<MeshRenderer>();
            createdPrefabLeaf.transform.SetParent(this.transform);
            createdPrefabLeaves[x] = createdPrefabLeaf;

            Vector2 upperLimit = new Vector2(0, 0);
            Vector2 lowerLimit = new Vector2(0, 0);

        }
        if (didSpawnedOnce == false)
        { didSpawnedOnce = true; }

        base.Spawn();


    }

    public override void SwitchToRGB()
    {
        base.SwitchToRGB();

        if (createdPrefabLeaves.Length > 0)
        {
            for (int x = 0; x < maxBeetLeafAmount; x++)
            {
                GameObject createdPrefabLeaf = Instantiate(beetLeaf[createdPrefabLeavesType[x]], createdPrefabLeaves[x].transform.position, createdPrefabLeaves[x].transform.rotation);
                //(beetLeaf_NIR[createdPrefabLeavesType[x]], createdPrefabLeaves[x]);
                createdPrefabLeaf.transform.SetParent(this.transform);
                createdPrefabLeaf.transform.localScale = createdPrefabLeaves[x].transform.localScale;
                Destroy(createdPrefabLeaves[x]);
                createdPrefabLeaves[x] = createdPrefabLeaf;
                //Debug.Log(createdPrefabLeavesType[x]);
            }
        }

    }

    public override void SwitchToNIR()
    {
        base.SwitchToNIR();

        if (createdPrefabLeaves.Length > 0)
        {
            for (int x = 0; x < maxBeetLeafAmount; x++)
            {
                GameObject createdPrefabLeaf = Instantiate(beetLeaf_NIR[createdPrefabLeavesType[x]], createdPrefabLeaves[x].transform.position, createdPrefabLeaves[x].transform.rotation);
                //(beetLeaf_NIR[createdPrefabLeavesType[x]], createdPrefabLeaves[x]);
                createdPrefabLeaf.transform.SetParent(this.transform);
                createdPrefabLeaf.transform.localScale = createdPrefabLeaves[x].transform.localScale;
                Destroy(createdPrefabLeaves[x]);
                createdPrefabLeaves[x] = createdPrefabLeaf;
                //Debug.Log(createdPrefabLeavesType[x]);
                //print(x);
            }
        }

    }

    public override void SwitchToTAG()
    {
        base.SwitchToTAG();
        if (createdPrefabLeaves.Length > 0)
        {
            for (int x = 0; x < maxBeetLeafAmount; x++)
            {
                GameObject createdPrefabLeaf = Instantiate(beetLeaf_TAG[createdPrefabLeavesType[x]], createdPrefabLeaves[x].transform.position, createdPrefabLeaves[x].transform.rotation);
                //(beetLeaf_NIR[createdPrefabLeavesType[x]], createdPrefabLeaves[x]);
                createdPrefabLeaf.transform.SetParent(this.transform);
                createdPrefabLeaf.transform.localScale = createdPrefabLeaves[x].transform.localScale;
                Destroy(createdPrefabLeaves[x]);
                createdPrefabLeaves[x] = createdPrefabLeaf;
                //Debug.Log(createdPrefabLeavesType[x]);
            }
        }
    }

    public void getLeaves()
    {
        print("types of leaves");
        //print(this.transform.GetChilds);
        //print(createdPrefabLeavesType[0]);
        foreach (Transform child in transform)
        {
            print(beetLeaf[1].name);
            print(child.name);
            print(child.GetType());
            print(createdPrefabLeaves[1]);

            //if (child == beetLeaf[1])

            /*
            if (beetLeaf.Length > 0)
            {
                var childCasted = new GameObject();
                if (childCasted as beetLeaf[1])
                {
                    print("match found");
                }
            }
            */
        }
        didSpawnedOnce = true; // used to have the right reference
    }
}
