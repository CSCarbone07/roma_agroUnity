using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class SugarBeet_Spawner : SpawnerAndSwitch
{

    public Material TestMaterial;

    public GameObject[] beetLeaf;// = new GameObject[] { beetLeaf1 };

    public GameObject[] beetLeaf_NIR;
    public GameObject[] beetLeaf_TAG;


    public int maxBeetLeafAmount = 7;
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
        thisRend = GetComponent<Renderer>();
        //Spawn();
        //print(createdPrefabLeaves.Length);
        //print(createdPrefabLeavesType.Length);


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
        //OnDrawGizmos();
        //Debug.Log("gizmosss");



    }



    /*
    // Draws a wireframe sphere in the Scene view, fully enclosing
    // the object.
    void OnDrawGizmos()
    {

        //Vector3[] pts = new Vector3[8];
        //Bounds b = createdPrefabLeaves[0].GetComponent().bounds;






        // A sphere that fully encloses the bounding box.
        Vector3 center;
        float radius;

        foreach (Renderer r in rend)
        {
            center = r.bounds.center;
            radius = r.bounds.extents.magnitude;

            Debug.Log(center.ToString("F4"));
            Debug.Log(radius.ToString("F4"));

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(center, radius);
        }


        center = createdPrefabLeaves[0].GetComponentInChildren<SkinnedMeshRenderer>().bounds.center;
        radius = createdPrefabLeaves[0].GetComponentInChildren<SkinnedMeshRenderer>().bounds.extents.magnitude;
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

        foreach (Transform child in transform) //this.gameObject.transform)
        {
            //DestroyImmediate(child.gameObject);
            GameObject.DestroyImmediate(child.gameObject);
        }

        //Debug.Log("I am a Good Plant");
        //GameObject createdPrefabStem = new GameObject();
        createdPrefabLeaves = new GameObject[maxBeetLeafAmount];
        createdPrefabLeavesType = new int[maxBeetLeafAmount];

        // Leaf Spawn
        //Vector3 tempPosition = position;
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

            // TODO this is going to create extra boxes for the labeling, need to change to not creating more plants 
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

            /*
            //Material leafMaterial = FindObjectOfType(typeof(Renderer));

            Mesh leafMesh = createdPrefabLeaf.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh;
            int[] leafTris = leafMesh.triangles;
            Vector3[] leafVerts = leafMesh.vertices;

            //Mesh leafMesh = createdPrefabLeaf.GetComponent<MeshFilter>().mesh;
            //leafUVs = leafMesh.uv;


            Material leafMaterial = createdPrefabLeaf.transform.GetChild(0).GetComponent<Renderer>().material;
            Texture2D leafTexture = (Texture2D)leafMaterial.mainTexture;
            List<Color> leafVisiblePixels = new List<Color>();
            List<Vector2> leafVisibleUVs = new List<Vector2>();
            Color[] leafPixels = leafTexture.GetPixels();
            Vector2[] leafUVs = leafMesh.uv;// = new Vector2[leafMesh.vertices.Length];
            int leafTextureWidth = leafTexture.width;
            int leafTextureHeight = leafTexture.height;

            print("printing verts");
            print(leafVerts.Length);
            print("printing uvs");
            print(leafUVs.Length);
            print(leafTris.Length);
            print(leafTextureWidth);
            print(leafTextureHeight);


            //Debug.Log("w: " + leafTextureWidth);
            //Debug.Log("h: " + leafTextureHeight);
            int counter = 0;
           
            for (int w = 0; w < leafTextureWidth; w += 1)
            {
                for (int h = 0; h < leafTextureHeight; h += 1)
                {
                    if (leafTexture.GetPixel(w,h).a > 0)
                    {
                        //Debug.Log("w: " + w);
                        //Debug.Log("h: " + h);

                        print("Getting pixel");

                        int u = w / leafTextureWidth;
                        int v = h / leafTextureHeight;
                        Vector2 uv = new Vector2(u,v);
                        leafVisibleUVs.Add(new Vector2(u,v));
                        Vector3 p3D = new Vector3(0,0,0);

                        if (counter == 0)
                        {
                            for (int i = 0; i < leafTris.Length; i += 3)
                            {

                                Vector2 u1 = leafUVs[leafTris[i]]; // get the triangle UVs
                                Vector2 u2 = leafUVs[leafTris[i + 1]];
                                Vector2 u3 = leafUVs[leafTris[i + 2]];
                                //Vector2 u1 = leafUVs[i]; // get the triangle UVs
                                //Vector2 u2 = leafUVs[i + 1];
                                //Vector2 u3 = leafUVs[i + 2];
                                // calculate triangle area - if zero, skip it
                                float a = Area(u1, u2, u3);
                                if (a > 0)
                                { Debug.Log("a: " + a); }



                                if (a == 0)
                                { continue; }
                                // calculate barycentric coordinates of u1, u2 and u3
                                // if anyone is negative, point is outside the triangle: skip it
                                float a1 = Area(u2, u3, uv) / a; if (a1 < 0) continue;
                                float a2 = Area(u3, u1, uv) / a; if (a2 < 0) continue;
                                float a3 = Area(u1, u2, uv) / a; if (a3 < 0) continue;
                                //if (Area(u2, u3, uv) > 0)
                                //{ Debug.Log("a: " + Area(u2, u3, uv)); }
                                // point inside the triangle - find mesh position by interpolation...
                                p3D = a1 * leafVerts[leafTris[i]] + a2 * leafVerts[leafTris[i + 1]] + a3 * leafVerts[leafTris[i + 2]];
                                // and return it in world coordinates:
                                Debug.Log("p3D: " + p3D);


                            }
                            //Debug.Log("pixel loc: " + p3D);
                            float uOffset = p3D.x - this.transform.position.x;
                            float vOffset = p3D.z - this.transform.position.z;
                            if (uOffset >= upperLimit.x)
                            { upperLimit.x = uOffset; }
                            if (uOffset <= lowerLimit.x)
                            { lowerLimit.x = uOffset; }
                            if (vOffset >= upperLimit.y)
                            { upperLimit.y = vOffset; }
                            if (vOffset <= lowerLimit.y)
                            { lowerLimit.y = vOffset; }

                            counter = counter + 1;

                        }


                    }
                }
            }
            //Color currentPixel = leafTexture.GetPixelBilinear(0.8f, 0.5f);
            //Color currentPixel = ((Texture2D)leafMaterial.mainTexture).GetPixelBilinear(0.8f, 0.5f);
            */



            //Debug.Log("lower limit: " + lowerLimit);
            //Debug.Log("upper limit: " + upperLimit);

            //createdPrefabLeaf.transform.SetParent(createdPrefabStem.transform);
        }
        if (didSpawnedOnce == false)
        { didSpawnedOnce = true; }

        base.Spawn();


    }



    // calculate signed triangle area using a kind of "2D cross product":
    float Area(Vector2 p1, Vector2 p2, Vector2 p3) 
    {
    Vector2 v1 = p1 - p3;
    Vector2 v2 = p2 - p3;
        //return (v1.x* v2.y - v1.y* v2.x)/2;
        return ((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2);
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
