using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Beet : MonoBehaviour
{


    public GameObject stem;
    public Vector3 stemScale = new Vector3(1, 1, 1);
    public Vector3 stemRotation; //for rotating the mesh in case the mesh is upside down or something else

    public GameObject leaf;
    public Vector3 leafScale = new Vector3(1, 1, 1);
    public Vector3 leafRotation; //for rotating the mesh in case the mesh is upside down or something else

    private int leafAmount = 3;

    private Vector3 myPosition;
    private Vector3 myRotation;
    private Quaternion newRotation;

    private Vector3 randomRotationValue;




    





    // Start is called before the first frame update
    void Start()
    {


        myPosition = transform.position;
        //myRotation = transform.rotation.;
        newRotation = Quaternion.Euler(stemRotation);

        /*------Stem Spawn---------*/
        GameObject createdPrefabStem = Instantiate(stem, myPosition, newRotation);
        createdPrefabStem.transform.SetParent(this.gameObject.transform); // = this.transform;
                                                                          //prefab.transform.parent = transform;
        createdPrefabStem.transform.localScale = stemScale;

        /*------Leaf Spawn---------*/
        for (int x = 0; x < leafAmount; x++)
        {
            randomRotationValue = new Vector3(0f, 0f, x * 90f * Random.Range(-60.0f, 60.0f));

            newRotation = Quaternion.Euler(leafRotation + randomRotationValue);

            GameObject createdPrefabLeaf = Instantiate(leaf, myPosition, newRotation);
            createdPrefabLeaf.transform.SetParent(this.gameObject.transform); // = this.transform;
                                                                              //prefab.transform.parent = transform;
            createdPrefabLeaf.transform.localScale = leafScale;

        }

    }

    
        



    // Update is called once per frame
    void Update()
    {
        
    }
}
