using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quadPlant_Spawner_RandomTexture : quadPlant_Spawner
{
    public Material RGB_Mat;
    public Material NIR_Mat;
    public Material TAG_Mat;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();


        //Texture2D myTexture = Resources.Load<Texture2D>(Application.dataPath + "Assets/Plants/Weeds/WeedBonirob/bonirob_2016-05-23-10-42-16_1_frame29.png");
        //Texture2D[] texturePool = new Texture2D[3];
        //texturePool = Resources.LoadAll<Texture2D>("bonirob/weds/rgb");
        //Texture2D myTexture = texturePool[Random.Range(0, texturePool.Length-1)];
        int randomName = Random.Range(0, maxIndexOfTextures);
        //Texture2D myRGBTexture = Resources.Load<Texture2D>("bonirob/weeds/rgb/" + randomName) as Texture2D;
        //Texture2D myNIRTexture = Resources.Load<Texture2D>("bonirob/weeds/nir/" + randomName) as Texture2D;
        Texture2D myRGBTexture = Resources.Load<Texture2D>(pathToTextures_RGB + randomName) as Texture2D;
        Texture2D myNIRTexture = Resources.Load<Texture2D>(pathToTextures_NIR + randomName) as Texture2D;

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
        RGB_Mat.mainTexture = myRGBTexture; //("_MainTex", myTexture);
        NIR_Mat.mainTexture = myNIRTexture; //("_MainTex", myTexture);
        TAG_Mat.mainTexture = myRGBTexture; //("_MainTex", myTexture);

    }

    // Update is called once per frame
    public override void Update()
    {

    }

}
