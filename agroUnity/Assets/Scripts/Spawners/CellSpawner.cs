using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CellSpawner : MonoBehaviour
{

    public GameObject prefab;
    //public int numberOfObjects = 20;
    //public float radius = 5f;
    public int cell_id = 0;
    private Vector3 myPosition;// = transform.position;
    private List<GameObject> createdPrefabs = new List<GameObject>();
    public float cellSize = 1.0f;
    //public float spacingX = 2f;
    //public float spacingY = 2f;
    public int max_prefabs= 12;

    // Positioning variables
    public Vector3 Rotation; //for rotating the mesh in case the mesh is upside down or something else
    public Vector3 Scale = new Vector3(1.0f, 1.0f, 1.0f); 
    private Quaternion newRotation;
    public Vector3 positionRandomness = new Vector3(0, 0, 0);
    private Vector3 addRandomRotation;

    [Range(0.0f, 1.0f)]
    public float addRandomRotationX = 0;
    [Range(0.0f, 1.0f)]
    public float addRandomRotationY = 0;
    [Range(0.0f, 1.0f)]
    public float addRandomRotationZ = 0;


    private Vector3 randomRotationValue;


    public float scaleRandomness = 0;// = new Vector3(0, 0, 0);
    /*
    [Range(0.0f, 1.0f)]
    public float scaleRandomnessX = 0;
    [Range(0.0f, 1.0f)]
    public float scaleRandomnessY = 0;
    [Range(0.0f, 1.0f)]
    public float scaleRandomnessZ = 0;
    [Range(0.0f, 100.0f)]
    */

    public float Density = 100.0f;

    public bool updateInstantiation = false;    // Click to update instances
    //private bool canUpdate = true;
    private bool regenerate = true;



    public List<GameObject> procedural_Instantiate(GameObject inGameObject)
    {
        gameObject.isStatic = true;


        foreach (Transform child in transform) //this.gameObject.transform)
        {
            //DestroyImmediate(child.gameObject);
            GameObject.DestroyImmediate(child.gameObject);
        }

        //print("Instatiating prefabs");
        //print(inGameObject);
        if (inGameObject != null)
        {
            prefab = inGameObject;
        }
        if (prefab != null)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }

            myPosition = transform.position;
            newRotation = Quaternion.Euler(Rotation);
            //print("Debugging1");
            if(createdPrefabs != null)
            {
                createdPrefabs.Clear();
            }
            //print("Debugging2");

            if (regenerate)
            {
                for (int i = 0; i < max_prefabs; i++)
                {
                    addRandomRotation = new Vector3(addRandomRotationX, addRandomRotationY, addRandomRotationZ);
                    randomRotationValue = addRandomRotation * (Random.Range(-180.0f, 180.0f));
                    newRotation = Quaternion.Euler(Rotation + randomRotationValue);

                    //print(Density);
                    if ((Density / 100) >= Random.Range(0.0f, 1.0f))
                    {

                        Vector3 newPositionRandomness = new Vector3(Random.Range(-positionRandomness.x, positionRandomness.x),
                        Random.Range(-positionRandomness.y, positionRandomness.y), Random.Range(-positionRandomness.z, positionRandomness.z));

                        Vector3 cellPositionRandomness = new Vector3(Random.Range(-cellSize/2, cellSize/2),
                        0, Random.Range(-cellSize/2, cellSize/2));

                        Vector3 pos = myPosition + cellPositionRandomness + newPositionRandomness;
                        GameObject createdPrefab = Instantiate(prefab, pos, Quaternion.Euler(Vector3.zero));
                        //print("Debugging3");
                        createdPrefabs.Add(createdPrefab);
                        //print("Debugging4");

                        createdPrefab.transform.SetParent(this.gameObject.transform); // = this.transform;
                                                                                      //prefab.transform.parent = transform;

                        float newFloatScaleRandomness = Random.Range(-scaleRandomness, scaleRandomness);
                        Vector3 newScaleRandomness = new Vector3(newFloatScaleRandomness, newFloatScaleRandomness, newFloatScaleRandomness);

                        //Vector3 newRandomness = new Vector3(Random.Range(0.0f, scaleRandomness.X), Random.Range(0.0f, scaleRandomness.Y), Random.Range(0.0f, scaleRandomness.Z));
                        //Vector3 newScaleRandomness = new Vector3(Random.Range(-scaleRandomnessX, scaleRandomnessX), Random.Range(-scaleRandomnessY, scaleRandomnessY), Random.Range(-scaleRandomnessZ, scaleRandomnessZ));

                        if (createdPrefab.GetComponent<SpawnerAndSwitch>())
                        {
                            createdPrefab.GetComponent<SpawnerAndSwitch>().setPlantScale(Random.Range(-scaleRandomness, scaleRandomness));
                            createdPrefab.transform.rotation = newRotation;
                            createdPrefab.GetComponent<SpawnerAndSwitch>().Spawn();
                        }
                        //print("spawning cell objects");
                    }



                    //createdPrefab.transform.localScale = Scale + newScaleRandomness;
                }

            }
        }

        foreach (Transform child in transform)
        {
            float newFloatScaleRandomness = Random.Range(-scaleRandomness, scaleRandomness);
            Vector3 newScaleRandomness = new Vector3(newFloatScaleRandomness, newFloatScaleRandomness, newFloatScaleRandomness);

            //Vector3 newScaleRandomness = new Vector3(newFloatScaleRandomness, newFloatScaleRandomness, newFloatScaleRandomness);
            //print(child);
            child.transform.localScale = Scale + newScaleRandomness;
        }
        //this.transform.localScale = Scale;
        //print("rescaling " + Scale);

        return createdPrefabs;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    //void OnRenderObject()
    {


        if (updateInstantiation)
        {
            procedural_Instantiate(prefab);
            updateInstantiation = false;
        }


        /*
        if (updateInstantiation)
        {
            if (canUpdate)
            {
                canUpdate = false;
                foreach (Transform child in this.gameObject.transform)
                {
                    DestroyImmediate(child.gameObject);
                }
                Instantiate();

                updateInstantiation = false;
                canUpdate = true;
            }
        }
        */


        /*
        for (int i = 0; i < numberOfObjects; i++)
        {

            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 pos = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius) + myPosition;
            Instantiate(prefab, pos, Quaternion.identity);
         }
        */


        //print(this.gameObject.name);
    }
}
