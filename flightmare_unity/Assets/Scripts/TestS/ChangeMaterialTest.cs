using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialTest : MonoBehaviour
{
    public Material groundTruthMaterial;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = groundTruthMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
