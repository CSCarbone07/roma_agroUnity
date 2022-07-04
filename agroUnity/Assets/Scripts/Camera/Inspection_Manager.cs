using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspection_Manager : MonoBehaviour
{


    public float delayBetweenMoves = 1;
    public bool respawnOnStart = false;
    private bool firstShoot = true;
    private string subfolder = "";
    private int counter = 0;

    public bool recordScreenshot = false;
    public bool includeRGB = false;
    public bool includeNIR = false;
    public bool includeTAG = false;
    private float delay_current = 0;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Inspection_Move>().Initialize();
	dataLoop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeScreenshot()
    {
        print("trying to take screenshot");
        if (recordScreenshot)
        {
	    print("taking screenshot");
	    this.GetComponent<SaveImage>().TakeScreenshot(subfolder, counter);
        }    
    }


    void dataLoop()
    {
	delay_current = 0;
        
	if (firstShoot == true)
        {
            firstShoot = false;
	    if(respawnOnStart)
	    {
	      delay_current += delayBetweenMoves;
	      Invoke("all_respawn", delay_current); 
	    }
        }
        else
        {
            rellocate();
            print("relocating");
        }




	if(includeRGB)
	{
	  delay_current += delayBetweenMoves;
	  Invoke("all_switch_rgb", delay_current);
	  delay_current += delayBetweenMoves;
	  Invoke("TakeScreenshot", delay_current); 
	}
	if(includeNIR)
	{
	  delay_current += delayBetweenMoves;
	  Invoke("all_switch_nir", delay_current);
	  delay_current += delayBetweenMoves;
	  Invoke("TakeScreenshot", delay_current);
	}
	if(includeTAG)
	{
	  delay_current += delayBetweenMoves;
	  Invoke("all_switch_tag", delay_current);
	  delay_current += delayBetweenMoves;
	  Invoke("TakeScreenshot", delay_current);
	}

	delay_current += delayBetweenMoves;
	Invoke("dataLoop", delay_current);

    }

    void all_respawn()
    {

	spawnedObjects.Clear();	
      
	//foreach (GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        foreach (GameObject g in GameObject.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {

	    print("found on respwan");
	    print(g);
	    if (g.GetComponent<PrefabInstatiation>())
            {
                List<GameObject> newSpawnedObjects = g.GetComponent<PrefabInstatiation>().procedural_Instantiate(null);
		
		spawnedObjects.AddRange(newSpawnedObjects); 
            }

        }

    }

    void all_switch_rgb()
    {


        //foreach (object g in FindObjectsOfType<SpawnerAndSwitch>())
        //foreach (GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        foreach (GameObject g in GameObject.FindObjectsOfType(typeof(GameObject)))
        {

            if (g.GetComponent<SpawnerAndSwitch>())
            {
                g.GetComponent<SpawnerAndSwitch>().SwitchToRGB();
            }

        }
        subfolder = "rgb/";

    }

    void all_switch_nir()
    {
        //foreach (GameObject g in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
        foreach (GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (g.GetComponent<SpawnerAndSwitch>())
            {
                g.GetComponent<SpawnerAndSwitch>().SwitchToNIR();
            }
        }
        subfolder = "nir/";

    }

    void all_switch_tag()
    {

        //foreach (GameObject g in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
        foreach (GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (g.GetComponent<SpawnerAndSwitch>())
            {
                g.GetComponent<SpawnerAndSwitch>().SwitchToTAG();
            }
        }
        subfolder = "tag/";

    }

    void rellocate()
    {
        this.GetComponent<Inspection_Move>().Rellocate();
        counter++;
    }


    /*
    void Manage()
    {

        Loop();
    }
    */

    void OnApplicationQuit()
    {
      //Application.LoadLevel("CurrentLevel");
      foreach (GameObject g in spawnedObjects)
      {
	DestroyImmediate(g);
      }
    }
}
