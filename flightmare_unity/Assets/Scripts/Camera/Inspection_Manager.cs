using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspection_Manager : MonoBehaviour
{

    public bool takeScreenshot = false;

    public float delayBetweenMoves = 1;
    private bool firstShoot = true;
    private string subfolder = "";
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Inspection_Move>().Initialize();
        all_switch_rgb();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeScreenshot()
    {
        if (takeScreenshot)
        {
            this.GetComponent<SaveImage>().TakeScreenshot(subfolder, counter);
        }    
    }

    void all_switch_rgb()
    {
        if (firstShoot == true)
        {
            firstShoot = false;
        }
        else
        {
            rellocate();
            print("relocating");
        }


        //foreach (object g in FindObjectsOfType<SpawnerAndSwitch>())
        //foreach (GameObject g in GameObject.FindObjectsOfType(typeof(GameObject)))
        foreach (GameObject g in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {

            if (g.GetComponent<SpawnerAndSwitch>())
            {
                g.GetComponent<SpawnerAndSwitch>().SwitchToRGB();
            }

        }
        subfolder = "rgb/";
        Invoke("TakeScreenshot", delayBetweenMoves/2);
        Invoke("all_switch_nir", delayBetweenMoves);
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
        Invoke("TakeScreenshot", delayBetweenMoves / 2);
        //TakeScreenshot();
        Invoke("all_switch_tag", delayBetweenMoves);

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
        Invoke("TakeScreenshot", delayBetweenMoves / 2);
        TakeScreenshot();
        Invoke("all_switch_rgb", delayBetweenMoves);

        counter++;
    }

    void rellocate()
    {
        this.GetComponent<Inspection_Move>().Rellocate();
    }


    /*
    void Manage()
    {

        Loop();
    }
    */
}
