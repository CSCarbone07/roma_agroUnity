using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{

    public GameObject[] prefab;
    //public int numberOfObjects = 20;
    //public float radius = 5f;
    private Vector3 myPosition;// = transform.position;
    //private GameObject createdPrefab;
    public float gridX = 5f;
    public float gridY = 5f;
    public float spacingX = 2f;
    public float spacingY = 2f;
    public Vector3 Rotation;
    public Vector3 Scale = new Vector3(1, 1, 1);
    private Quaternion newRotation;
    //private float test = 5;
    private GameObject[] spawnedPlants;


    // Start is called before the first frame update
    void Start()
    {
        myPosition = transform.position;
        newRotation = Quaternion.Euler(Rotation);

        Vector3 pos = new Vector3(1, 1, 1);
        //prefab[0] = (GameObject)lants.Load("enemy");
        Instantiate(prefab[0], myPosition, newRotation);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
