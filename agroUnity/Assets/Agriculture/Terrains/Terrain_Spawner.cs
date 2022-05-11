using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Terrain_Spawner : SpawnerAndSwitch
{
    public Material terrain_RGB;
    public Material terrain_NIR;
    public Material terrain_TAG;

    public Vector3 positionRandomness = new Vector3(0, 0, 0);


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        terrain_RGB = new Material(terrain_RGB);
        terrain_NIR = new Material(terrain_NIR);
        terrain_TAG = new Material(terrain_TAG);


        Vector3 newPositionRandomness = new Vector3(Random.Range(-positionRandomness.x, positionRandomness.x),
        Random.Range(-positionRandomness.y, positionRandomness.y), Random.Range(-positionRandomness.z, positionRandomness.z));

        this.transform.position += newPositionRandomness;


    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void Spawn()
    {

    }

    public override void SwitchToRGB()
    {
        base.SwitchToRGB();
        GetComponent<Renderer>().material = terrain_RGB;
        //GetComponent<Renderer>().material = NIR_Mat;


    }
    public override void SwitchToNIR()
    {
        base.SwitchToNIR();
        GetComponent<Renderer>().material = terrain_NIR;
        //GetComponent<Renderer>().material = NIR_Mat;


    }
    public override void SwitchToTAG()
    {
        base.SwitchToTAG();
        GetComponent<Renderer>().material = terrain_TAG;
        //Debug.Log("weeeed");       
    }
}
