using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;

public class sunflower_spawner : SpawnerAndSwitch
{

    public Material TestMaterial;

    public GameObject[] rootLeaf;// = new GameObject[] { beetLeaf1 };
    public GameObject[] rootLeaf_NIR;
    public GameObject[] rootLeaf_TAG;

    public GameObject[] headLeaf;// = new GameObject[] { beetLeaf1 };
    public GameObject[] headLeaf_NIR;
    public GameObject[] headLeaf_TAG;

    public GameObject[] centerLeaf;// = new GameObject[] { beetLeaf1 };
    public GameObject[] centerLeaf_NIR;
    public GameObject[] centerLeaf_TAG;

    private int maxLeafAmount = 2;
    public float plantDeltaHeight = 0.1f;  //height between root leaves and head leaves
    public Vector3 leafScale = new Vector3(1, 1, 1);

    private GameObject[] createdPrefab_rootLeaves;
    private int[] createdPrefab_rootLeaves_Type;

    private GameObject[] createdPrefab_headLeaves;
    private int[] createdPrefab_headLeaves_Type;

    private GameObject createdPrefab_centerLeaves;
    private int createdPrefab_centerLeaves_Type;

    protected Vector3 randomRotationValue;
    protected Quaternion newRotation;
    public Vector3 basePlantRotation = new Vector3(200f, 0f, -90f);
    public Vector3 baseLeafRotation = new Vector3(200f, 0f, -90f);
    public Renderer[] rend;
    public Renderer thisRend;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        thisRend = GetComponent<Renderer>();
        //OnDrawGizmosSelected();
        /*
        rend = new Renderer[createdPrefabLeaves.Length];
        int n = 0;
        foreach(GameObject leaf in createdPrefabLeaves)
        {
            rend[n]=leaf.GetComponentInChildren<SkinnedMeshRenderer>();
            n++;
        }
        */
        //OnDrawGizmosSelected();
        //Debug.Log("gizmosss");

    }



    /*
    // Draws a wireframe sphere in the Scene view, fully enclosing
    // the object.
    void OnDrawGizmos()
    {
        // A sphere that fully encloses the bounding box.


        
        foreach(Renderer r in rend)
        {
            Vector3 center = r.bounds.center;
            float radius = r.bounds.extents.magnitude;

            Debug.Log(center.ToString("F4"));
            Debug.Log(radius.ToString("F4"));

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(center, radius);
        }


        Vector3 center = createdPrefabLeaves[0].GetComponentInChildren<SkinnedMeshRenderer>().bounds.center;
        float radius = createdPrefabLeaves[0].GetComponentInChildren<SkinnedMeshRenderer>().bounds.extents.magnitude;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(center, radius);
    }
    */

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void Spawn()
    {
        base.Spawn();
        //Debug.Log("I am a Good Plant");
        //GameObject createdPrefabStem = new GameObject();
        createdPrefab_rootLeaves = new GameObject[maxLeafAmount];
        createdPrefab_rootLeaves_Type = new int[maxLeafAmount];

        createdPrefab_headLeaves = new GameObject[maxLeafAmount];
        createdPrefab_headLeaves_Type = new int[maxLeafAmount];

        // Leaf Spawn
        //Vector3 tempPosition = position;
        Vector3 tempPosition = this.transform.position;
        tempPosition[1] += 0.8f;

        Vector3 randomPlantRotation = new Vector3(0f, UnityEngine.Random.Range(-180.0f, 180.0f), 0f);
        Vector3 plantRotation = basePlantRotation + randomPlantRotation;

        GameObject createdPrefabLeaf;
        Vector3 leafRotationOffset;
        int typeOfLeaf;

        // first spawn root leaves
        for (int x = 0; x < maxLeafAmount; x++)
        {
            Debug.Log("spawning leaf: " + x);



            leafRotationOffset = new Vector3(UnityEngine.Random.Range(-16.0f, 16.0f), 
                                                    UnityEngine.Random.Range(-16.0f, 16.0f)+(x*180f), 0.0f);

            //Root spawn
            //plantRotation[0] += UnityEngine.Random.Range(-5.0f, 5.0f);
            newRotation = Quaternion.Euler(baseLeafRotation + plantRotation + leafRotationOffset);

            typeOfLeaf = Random.Range(0, rootLeaf.Length - 1);

            //print("Type of root leaf: " + typeOfLeaf + " in round " + x);

            createdPrefab_rootLeaves_Type[x] = typeOfLeaf;
            createdPrefabLeaf = Instantiate(rootLeaf[typeOfLeaf], tempPosition, newRotation);
            createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.8f, 1f);
                
            createdPrefabLeaf.SetActive(true);
            createdPrefabLeaf.AddComponent<MeshRenderer>();
            createdPrefabLeaf.transform.SetParent(this.transform);
            createdPrefab_rootLeaves[x] = createdPrefabLeaf;


            //Head spawn
            leafRotationOffset = new Vector3(UnityEngine.Random.Range(-16.0f, 16.0f),
                                                    UnityEngine.Random.Range(-16.0f, 16.0f) + (x * 180f)+90, 0.0f);

            newRotation = Quaternion.Euler(baseLeafRotation + plantRotation + leafRotationOffset);

            typeOfLeaf = Random.Range(0, headLeaf.Length - 1);

            //print("Type of head leaf: " + typeOfLeaf + " in round " + x);

            createdPrefab_headLeaves_Type[x] = typeOfLeaf;
            Vector3 headPosition = tempPosition + new Vector3(0, plantDeltaHeight, 0);
            createdPrefabLeaf = Instantiate(headLeaf[typeOfLeaf], headPosition, newRotation);

            createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.8f, 1f);

            createdPrefabLeaf.SetActive(true);
            createdPrefabLeaf.AddComponent<MeshRenderer>();
            createdPrefabLeaf.transform.SetParent(this.transform);
            createdPrefab_headLeaves[x] = createdPrefabLeaf;


        }

        //center spawn
        leafRotationOffset = new Vector3(UnityEngine.Random.Range(-16.0f, 16.0f),
                                    UnityEngine.Random.Range(-16.0f, 16.0f) + 90, 0.0f);

        newRotation = Quaternion.Euler(baseLeafRotation + plantRotation + leafRotationOffset);

        typeOfLeaf = Random.Range(0, centerLeaf.Length - 1);
        createdPrefab_centerLeaves_Type = typeOfLeaf;
        Vector3 centerPosition = tempPosition + new Vector3(0, plantDeltaHeight+0.01f, 0);
        createdPrefabLeaf = Instantiate(centerLeaf[typeOfLeaf], centerPosition, newRotation);

        createdPrefabLeaf.transform.localScale = leafScale * UnityEngine.Random.Range(0.8f, 1f);

        createdPrefabLeaf.SetActive(true);
        createdPrefabLeaf.AddComponent<MeshRenderer>();
        createdPrefabLeaf.transform.SetParent(this.transform);
        createdPrefab_centerLeaves = createdPrefabLeaf;


    }

    // calculate signed triangle area using a kind of "2D cross product":
    float Area(Vector2 p1, Vector2 p2, Vector2 p3) 
    {
    Vector2 v1 = p1 - p3;
    Vector2 v2 = p2 - p3;
        //return (v1.x* v2.y - v1.y* v2.x)/2;
        return ((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2);
    }

    public override void SwitchToNIR()
    {
        base.SwitchToNIR();
        GameObject createdPrefabLeaf;
        for (int x = 0; x < maxLeafAmount; x++)
        {
            createdPrefabLeaf = Instantiate(rootLeaf_NIR[createdPrefab_rootLeaves_Type[x]], createdPrefab_rootLeaves[x].transform.position, createdPrefab_rootLeaves[x].transform.rotation);
            createdPrefabLeaf.transform.localScale = createdPrefab_rootLeaves[x].transform.localScale;
            createdPrefabLeaf.transform.SetParent(this.transform);
            Destroy(createdPrefab_rootLeaves[x]);
            createdPrefab_rootLeaves[x] = createdPrefabLeaf;

            createdPrefabLeaf = Instantiate(headLeaf_NIR[createdPrefab_headLeaves_Type[x]], createdPrefab_headLeaves[x].transform.position, createdPrefab_headLeaves[x].transform.rotation);
            createdPrefabLeaf.transform.localScale = createdPrefab_headLeaves[x].transform.localScale;
            createdPrefabLeaf.transform.SetParent(this.transform);
            Destroy(createdPrefab_headLeaves[x]);
            createdPrefab_headLeaves[x] = createdPrefabLeaf;

        }

        createdPrefabLeaf = Instantiate(centerLeaf_NIR[createdPrefab_centerLeaves_Type], createdPrefab_centerLeaves.transform.position, createdPrefab_centerLeaves.transform.rotation);
        createdPrefabLeaf.transform.localScale = createdPrefab_centerLeaves.transform.localScale;
        createdPrefabLeaf.transform.SetParent(this.transform);
        Destroy(createdPrefab_centerLeaves);
        createdPrefab_centerLeaves = createdPrefabLeaf;

    }

    public override void SwitchToTAG()
    {
        base.SwitchToTAG();
        GameObject createdPrefabLeaf;
        for (int x = 0; x < maxLeafAmount; x++)
        {
            createdPrefabLeaf = Instantiate(rootLeaf_TAG[createdPrefab_rootLeaves_Type[x]], createdPrefab_rootLeaves[x].transform.position, createdPrefab_rootLeaves[x].transform.rotation);
            createdPrefabLeaf.transform.localScale = createdPrefab_rootLeaves[x].transform.localScale;
            createdPrefabLeaf.transform.SetParent(this.transform);
            Destroy(createdPrefab_rootLeaves[x]);
            createdPrefab_rootLeaves[x] = createdPrefabLeaf;

            createdPrefabLeaf = Instantiate(headLeaf_TAG[createdPrefab_headLeaves_Type[x]], createdPrefab_headLeaves[x].transform.position, createdPrefab_headLeaves[x].transform.rotation);
            createdPrefabLeaf.transform.localScale = createdPrefab_headLeaves[x].transform.localScale;
            createdPrefabLeaf.transform.SetParent(this.transform);
            Destroy(createdPrefab_headLeaves[x]);
            createdPrefab_headLeaves[x] = createdPrefabLeaf;

        }

        createdPrefabLeaf = Instantiate(centerLeaf_TAG[createdPrefab_centerLeaves_Type], createdPrefab_centerLeaves.transform.position, createdPrefab_centerLeaves.transform.rotation);
        createdPrefabLeaf.transform.localScale = createdPrefab_centerLeaves.transform.localScale;
        createdPrefabLeaf.transform.SetParent(this.transform);
        Destroy(createdPrefab_centerLeaves);
        createdPrefab_centerLeaves = createdPrefabLeaf;



    }
}
