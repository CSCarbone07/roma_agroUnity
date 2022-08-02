using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qrTextureHandler : SpawnerAndSwitch
{
    private bool DEBUG_ALL = false;
    
    public Material RGB_Mat;
    public string pathToTextures_RGB = "qr/numbers/";
    private Material RGB_Mat_internal;
    
    private int currentTextureID = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void Awake()
    {
        materialsSetup();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void materialsSetup()
    {
	if(DEBUG_ALL)
	{
	  print("setting materials");
	}

        RGB_Mat_internal = new Material(RGB_Mat);
        //NIR_Mat_internal = new Material(NIR_Mat);
        //TAG_Mat_internal = new Material(TAG_Mat);

        Texture2D myRGBTexture = Resources.Load<Texture2D>(pathToTextures_RGB + currentTextureID) as Texture2D;
        //Texture2D myNIRTexture = Resources.Load<Texture2D>(pathToTextures_NIR + currentTextureID) as Texture2D;


	if(DEBUG_ALL)
	{
	  print(pathToTextures_RGB + currentTextureID);
	}

        RGB_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);
        //NIR_Mat_internal.mainTexture = myNIRTexture; //("_MainTex", myTexture);
        //TAG_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);

        GetComponent<Renderer>().material = RGB_Mat_internal;
    }
    

    public override void Spawn()
    {

      currentTextureID = appearanceId;
      materialsSetup();


    }
}
