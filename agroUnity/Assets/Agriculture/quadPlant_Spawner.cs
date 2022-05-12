using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class quadPlant_Spawner : SpawnerAndSwitch
{
    public Material RGB_Mat;
    public Material NIR_Mat;
    public Material TAG_Mat;
    private Material RGB_Mat_internal;
    private Material NIR_Mat_internal;
    private Material TAG_Mat_internal;

    public bool randomizeInitialTexture = false;
    public bool randomizeSimTexture = false;
    public string pathToTextures_RGB = "bonirob/weeds/rgb/";
    public string pathToTextures_NIR = "bonirob/weeds/nir/";
    public int maxIndexOfTextures = 51;
    public int currentTextureID = 0;
    public Vector3 weedScale = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    public override void Start()
    {

        transform.localScale = weedScale;

        /*     
        if (RGB_Mat_internal == null)
        {
            materialsSetup();
        }
        */




    }

    void Awake()
    {
        materialsSetup();

    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public void materialsSetup()
    {
        RGB_Mat_internal = new Material(RGB_Mat);
        NIR_Mat_internal = new Material(NIR_Mat);
        TAG_Mat_internal = new Material(TAG_Mat);

        Texture2D myRGBTexture = Resources.Load<Texture2D>(pathToTextures_RGB + currentTextureID) as Texture2D;
        Texture2D myNIRTexture = Resources.Load<Texture2D>(pathToTextures_NIR + currentTextureID) as Texture2D;

        RGB_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);
        NIR_Mat_internal.mainTexture = myNIRTexture; //("_MainTex", myTexture);
        TAG_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);

        GetComponent<Renderer>().material = RGB_Mat_internal;
    }

    public override void Spawn()
    {
        base.Spawn();
        //print("weed spawn");

        materialsSetup();

        if (randomizeInitialTexture)
        {
            changeTexture();
        }

    }


    public void changeTexture()
    {
        //Texture2D myTexture = Resources.Load<Texture2D>(Application.dataPath + "Assets/Plants/Weeds/WeedBonirob/bonirob_2016-05-23-10-42-16_1_frame29.png");
        //Texture2D[] texturePool = new Texture2D[3];
        //texturePool = Resources.LoadAll<Texture2D>("bonirob/weds/rgb");
        //Texture2D myTexture = texturePool[Random.Range(0, texturePool.Length-1)];
        int randomName = Random.Range(0, maxIndexOfTextures);
        currentTextureID = randomName;
        //Texture2D myRGBTexture = Resources.Load<Texture2D>("bonirob/weeds/rgb/" + randomName) as Texture2D;
        //Texture2D myNIRTexture = Resources.Load<Texture2D>("bonirob/weeds/nir/" + randomName) as Texture2D;
        Texture2D myRGBTexture = Resources.Load<Texture2D>(pathToTextures_RGB + currentTextureID) as Texture2D;
        Texture2D myNIRTexture = Resources.Load<Texture2D>(pathToTextures_NIR + currentTextureID) as Texture2D;
        //print("spawning weed: " + pathToTextures_RGB + randomName);
        /*
        var sr = new StreamReader(Application.dataPath + "/" + fileName);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        var lines = fileContents.Split("\n"[0]);
        for (line in lines)
        {
            print(line);
        }
        */
        //GetComponent<Renderer>().material.mainTexture = myRGBTexture; //("_MainTex", myTexture);
        //print("spawning weed " + myRGBTexture);
        RGB_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);
        NIR_Mat_internal.mainTexture = myNIRTexture; //("_MainTex", myTexture);
        TAG_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);
    }


    public override void SwitchToRGB()
    {
        base.SwitchToRGB();
        /*
        if (RGB_Mat_internal == null)
        {
            materialsSetup();
        }
        */
        //materialsSetup();

        if (randomizeSimTexture)
        {
            changeTexture();
        }



        GetComponent<Renderer>().material = RGB_Mat_internal;
        //Debug.Log("weeeed");       
    }
    public override void SwitchToNIR()
    {
        base.SwitchToNIR();
        /*
        if (NIR_Mat_internal == null)
        {
            materialsSetup();
        }
        */
        GetComponent<Renderer>().material = NIR_Mat_internal;
        print(this.name + " | selecting nir texture number " + currentTextureID);
	//Debug.Log("weeeed");       
    }
    public override void SwitchToTAG()
    {
        base.SwitchToTAG();
        GetComponent<Renderer>().material = TAG_Mat_internal;
        //Debug.Log("weeeed");       
    }

}
