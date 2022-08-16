using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class surface_lerp_colorRandomization : MonoBehaviour
{
    private bool DEBUG_ALL = false;

    public Material baseMaterial;
   

    public bool usePerlinNoise = true;

    public int pixWidth = 1024;
    public int pixHeight = 1024;
    // The origin of the sampled area in the plane.
    public float xOrg = 0;
    public float yOrg = 0;
    public float xOrg_random = 100;
    public float yOrg_random = 100;
    
    public float scale = 1.0F;
    public float scale_random = 0.2F;
    
    [Tooltip("range -360 to 720, hue ranges are 0 to 360")]
    public int hue_min = 0;
    [Tooltip("range -360 to 720, hue ranges are 0 to 360")]
    public int hue_max = 360;
    [Tooltip("range -100 to 200, sat ranges are 0 to 100")]
    public int sat_min = 0;
    [Tooltip("range -100 to 200, sat ranges are 0 to 100")]
    public int sat_max = 100;
    [Tooltip("range -100 to 200, val ranges are 0 to 100")]
    public int val_min = 0;
    [Tooltip("range -100 to 200, val ranges are 0 to 100")]
    public int val_max = 100;

    private Material RGB_Mat_internal;
    
    private Texture2D noiseTex;
    private Color[] pix;

    // Start is called before the first frame update
    void Start()
    {
      setupMaterials();
      setPerlinNoise();
      randomizeColors();

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
     
    void setupMaterials()
    {
	RGB_Mat_internal = new Material(baseMaterial);
        //NIR_Mat_internal = new Material(NIR_Mat);
        //TAG_Mat_internal = new Material(TAG_Mat);

        //RGB_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);
        //NIR_Mat_internal.mainTexture = myNIRTexture; //("_MainTex", myTexture);
        //TAG_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);
    }

    void setPerlinNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

	noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        RGB_Mat_internal.mainTexture = noiseTex;

	xOrg = Random.Range(xOrg, xOrg_random);
	yOrg = Random.Range(yOrg, yOrg_random);

	scale = Random.Range(scale - scale_random, scale + scale_random);

	while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    void randomizeColors()
    {

	int hue_current1 = Random.Range(hue_min, hue_max);
	int sat_current1 = Random.Range(sat_min, sat_max);
	int val_current1 = Random.Range(val_min, val_max);
	int hue_current2 = Random.Range(hue_min, hue_max);
	int sat_current2 = Random.Range(sat_min, sat_max);
	int val_current2 = Random.Range(val_min, val_max);
	
	if(DEBUG_ALL)
	{
	  print("current hsv values pre process");
	  print(hue_current1);
	  print(sat_current1);
	  print(val_current1);
	  print(hue_current2);
	  print(sat_current2);
	  print(val_current2);
	}

	if (hue_current1 < 0) 
	{hue_current1 = 360 + hue_current1;}
	if (hue_current1 > 360) 
	{hue_current1 = hue_current1 - 360;}
	if (hue_current2 < 0) 
	{hue_current2 = 360 + hue_current2;}
	if (hue_current2 > 360) 
	{hue_current2 = hue_current2 - 360;}

	if (sat_current1 < 0) 
	{sat_current1 = 100 + sat_current1;}
	if (sat_current1 > 100) 
	{sat_current1 = sat_current1 - 100;}
	if (sat_current2 < 0) 
	{sat_current2 = 100 + sat_current2;}
	if (sat_current2 > 100) 
	{sat_current2 = sat_current2 - 100;}
	
	if (val_current1 < 0) 
	{val_current1 = 100 + val_current1;}
	if (val_current1 > 100) 
	{val_current1 = val_current1 - 100;}
	if (val_current2 < 0) 
	{val_current2 = 100 + val_current2;}
	if (val_current2 > 100) 
	{val_current2 = val_current2 - 100;}
	
	if(DEBUG_ALL)
	{
	  print("current hsv values post process");
	  print(hue_current1);
	  print(sat_current1);
	  print(val_current1);
	  print(hue_current2);
	  print(sat_current2);
	  print(val_current2);
	}

	Color color_current1 = Color.HSVToRGB(hue_current1/360.0f, sat_current1/100.0f, val_current1/100.0f);
	Color color_current2 = Color.HSVToRGB(hue_current2/360.0f, sat_current2/100.0f, val_current2/100.0f);


	if(DEBUG_ALL)
	{
	  print("selected colors");
	  print(color_current1);
	  print(color_current2);
	}
	
	RGB_Mat_internal.SetColor("_Color1", color_current1);	

	RGB_Mat_internal.SetColor("_Color1", color_current1);	
	RGB_Mat_internal.SetColor("_Color2", color_current2);	

	//RGB_Mat_internal.Color1 = Color.HSVToRGB(hue_current, sat_current, val_current);
	//RGB_Mat_internal.Color2 = Color.HSVToRGB(hue_current, sat_current, val_current);

        GetComponent<Renderer>().material = RGB_Mat_internal;
    }
}
