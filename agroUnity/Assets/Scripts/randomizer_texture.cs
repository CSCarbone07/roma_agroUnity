using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizer_texture : MonoBehaviour
{

    public Material baseMaterial;
    public string pathToTextures_RGB = "grounds/wageningen/sugarBeets/";
    //public string pathToTextures_NIR = "bonirob/weeds/nir/";
    public int minIndexOfTextures = 1;
    public int maxIndexOfTextures = 100;
    private int currentTextureID = 0;
    
    private Material RGB_Mat_internal;

    // Start is called before the first frame update
    void Start()
    {
      setupMaterials();
      changeTexture();
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

    }

    void changeTexture()
    {
        int randomName = Random.Range(minIndexOfTextures, maxIndexOfTextures);
        currentTextureID = randomName;
        

        Texture2D myRGBTexture = Resources.Load<Texture2D>(pathToTextures_RGB + currentTextureID) as Texture2D;
        //Texture2D myNIRTexture = Resources.Load<Texture2D>(pathToTextures_NIR + currentTextureID) as Texture2D;

        RGB_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);
        //NIR_Mat_internal.mainTexture = myNIRTexture; //("_MainTex", myTexture);
        //TAG_Mat_internal.mainTexture = myRGBTexture; //("_MainTex", myTexture);

        GetComponent<Renderer>().material = RGB_Mat_internal;
    }

}
