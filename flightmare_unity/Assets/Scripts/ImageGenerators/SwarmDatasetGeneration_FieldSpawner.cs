using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;

public class SwarmDatasetGeneration_FieldSpawner
 : MonoBehaviour
{

    //field premade variables
     



    //procedural field variables
    private Vector3 cameraInitialPosition;

    private bool firstSpawn = true;

    public bool Include_NIR = false;


    public GameObject goodPlant;
    public Vector3 goodPlant_Offset = new Vector3(0f, 0.1f, 0f);



    /*
    public SwarmDatasetGeneration_Spawner ()
    {
        beetLeaf = Instantiate(Resources.Load("Assets/Plants/2", typeof(GameObject))) as GameObject;
    }
    */



    public Vector3 goodPlantScale = new Vector3(1, 1, 1);

    //public static int WeedInit = 14;//5;//20;
    public int WeedNumber = 14;

    public GameObject[] weedPlants;
    public Vector3[] weedPlants_Offset;
    public Vector3[] weedPlants_Rotation_Min;
    public Vector3[] weedPlants_Rotation_Max;
    public Vector3[] weedPlants_Scale;


    public Vector3 capsellaLeafScale = new Vector3(1, 1, 1);

    // materials for ground truths

    public Material PlainBlack;
    public Material green;
    public Material white;

    public GameObject terrain;

    private Material red;
    private Material black;
    // to select the kind of annotation to be generated
    public bool TakeScreenshots = true;
    public bool SaveBoxes = false;

    protected Quaternion newRotation;
    protected Vector3 randomRotationValue;

    protected static int plantNumber = 84;//9;//65;
    protected static int cropRows = 6;//3;//5;

    //private static int CapsellaNumber = 8;
    //private static int GalliumNumber = 8;


    string[] boxes; //= new string[plantNumber + WeedNumber];

    // object instances 
    protected GameObject[] newPlant = new GameObject[plantNumber];
    protected GameObject[] newWeed; //= new GameObject[WeedInit];
    //private GameObject[] newCapsella = new GameObject[CapsellaNumber];
    //private GameObject[] newGallium = new GameObject[GalliumNumber];
    protected GameObject newTerrain;

    protected int beetLeafAmount = 7;
    protected int galliumLeafAmount = 5;
    protected int capsellaLeafAmount = 8;

    // control the spawing ratio, remember to check also Invoke() functions
    protected float clearDelay = 2f;
    protected float spawnDelay = 3f;
    protected float NIRswitchDelay = 1f;
    protected float TAGswitchDelay = 1f;
    protected float nextSpawnTime = 0f;
    protected float nextNIRswitch = 1f;


    // range for spawing objects
    protected float minScaleValue = 0.2f;
    protected float maxScaleValue = 1f;

    // control missing beet ratio
    protected float missBeet = 10f;
    // control gallium/capsella ratio
    protected float weedType = 7.5f;

    // counter to save pictures incrementally
    protected static int imgPerWeedNumber = 5;
    protected int counter;// = WeedNumber * imgPerWeedNumber;


    // camera resolution
    protected int width = 1024;
    protected int height = 1024;

    Vector3 spawnPoint;
    Vector3 scaleFactor;

    protected Vector3 zeroPos = new Vector3(0f, 0.4f, 0f);
    protected Vector3 cameraPos = new Vector3(-1f, 7f, -0.7f);
    protected Vector3 cameraRot = new Vector3(90, 0, 0);
    protected Vector3 defaultScale = new Vector3(25f, 1f, 25f);
    protected Vector3 defaultPos = new Vector3(0f, 0.5f, 0f); //default terrain dimensions
    protected Vector3 beetLeafRotation = new Vector3(200f, 0f, -90f);
    protected Vector3 galliumLeafRotation = new Vector3(-90, 0f, 0f);
    protected Vector3 capsellaLeafRotation = new Vector3(0f, 0f, 0f);

    Quaternion zeroRot = Quaternion.Euler(0, 0, 0);
    Quaternion rotation;

    // control where to save pictures
    public enum type { Image, Mask, Box };
    public enum cls { Crop, Weed };
    public enum field { A, B, C, D, E, F, G, H, I, L };
    public enum species { Beet, Gall, Caps };

    protected species[] specs; // = new species[plantNumber + WeedInit];
    public ArrayList positions = new ArrayList();
    public ArrayList rotations = new ArrayList();

    // Start is called before the first frame update
    public void Start()
    {
        //readField();
        /*
        cameraInitialPosition = this.transform.position;
        newWeed = new GameObject[WeedNumber];
        boxes = new string[plantNumber + WeedNumber];
        //counter = WeedNumber * imgPerWeedNumber;
        counter = 0;
        specs = new species[plantNumber + WeedNumber];

        SpawnTerrain();
        Spawn();

        if (Include_NIR)
        {        
            spawnDelay = 8;
            clearDelay = 6;
            TAGswitchDelay = 4;
            NIRswitchDelay = 2;
        }
        else
        {
            spawnDelay = 6;
            clearDelay = 4;
            TAGswitchDelay = 2;
        }

        updateScreen();
        */

    }

    // Update is called once per frame
    public void Update()
    {
        //Debug.Log(nextSpawnTime);


    }

    /*
    private void readField()
    {
        string path = "Assets/Resources/Texts/Fields/fields.yaml";
        Debug.Log("file read start");
        StreamReader reader = new StreamReader(path);
        string stream = reader.ReadToEnd();
        string[] lines = stream.Split('\n');
        foreach (string line in lines)
        {
            string[] numbers = line.Split(' ');
            foreach(string i in numbers)
            {
                //int inNumber = int.Parse(i);
                int inNumber;
                int.TryParse(i, out inNumber);
                Debug.Log(inNumber);
            }
            //Debug.Log(line);
        }

        //Debug.Log(reader.ReadToEnd());
        Debug.Log("file read finished");
        reader.Close();

    }
    */
    private void updateScreen()
    {
        Debug.Log("spawning");

        float randInitX = Random.Range(-3, 3);
        float randInitY = Random.Range(-0.5f, 0.5f);
        float randInitZ = Random.Range(-3, 3);
        Vector3 RandomPosition = new Vector3(randInitX, randInitY, randInitZ);
        this.transform.position = cameraInitialPosition + RandomPosition;

        if (TakeScreenshots)
        {
            //Invoke("SaveRGB", 0.5f);
            Invoke("SaveRGB", 1.0f);
        }

        if (Include_NIR)
        {
            Invoke("SwitchToNIR", NIRswitchDelay);
            if (TakeScreenshots)
            {
                //Invoke("SaveNIR", 1.5f);
                //Invoke("SaveTAG", 2.5f);
                Invoke("SaveNIR", 3.0f);
                Invoke("SaveTAG", 5.0f);
            }
        }
        else
        {
            if (TakeScreenshots)
            {
                //Invoke("saveTAG", 1.5f);
                Invoke("saveTAG", 3.0f);

            }
        }

        Invoke("SwitchToTAG", TAGswitchDelay);
        Invoke("clearScene", clearDelay);
    }

    private bool ShouldSpawn()
    {
        //firstSpawn = false

        return Time.time >= nextSpawnTime;

    }
    private bool ShouldSwitch()
    {
        return Time.time >= nextNIRswitch;
    }

    private void CounterUpdate()
    {
        counter++;
        if(counter >= 1001)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void clearScene()
    {
        Destroy(newTerrain);
        for (int i = 0; i < plantNumber; i++)
        {
            if (newPlant[i] != null)
            {
                Destroy(newPlant[i]);
            }
        }
        for (int i = 0; i < WeedNumber; i++)
        {
            Destroy(newWeed[i]);
        }
        CounterUpdate();
    }



    private void Spawn()
    {
        int cnt = 0;
        float x_offset = 1.25f;
        float z_offset = 0.53f;

        Vector3 pos = RandomPosition();
        Vector3 start_pos = pos;

        for (int j = 0; j < cropRows; j++)
        {
            for (int k = 0; k < plantNumber / cropRows; k++)
            {
                if (UnityEngine.Random.Range(0f, 10f) >= missBeet)
                {
                    newPlant[cnt] = null;
                    specs[cnt] = species.Beet;
                }
                else
                {
                    newPlant[cnt] = SpawnGoodPlant(pos + goodPlant_Offset);//SpawnBeet(pos + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0.1f, UnityEngine.Random.Range(-0.2f, 0.2f)));
                    changeColor(newPlant[cnt], 0.5f, 0.8f);
                    specs[cnt] = species.Beet;
                }
                cnt++;
                pos[2] = pos[2] + z_offset;
            }

            start_pos[0] = start_pos[0] + x_offset;
            pos = start_pos;
        }
       
        for (int i = 0; i < WeedNumber; i++)
        {

            Vector3 tempPosition = new Vector3(UnityEngine.Random.Range(-4.1f, 2f), 0.8f, UnityEngine.Random.Range(-4.1f, 2f));
            newWeed[i] = SpawnWeedPlant(tempPosition);
            //changeColor(newWeed[i], 0.5f, 0.6f);
        }

        if (TakeScreenshots)
        {
            print("shot");
        }

        RandomLightAndPosition();

        nextSpawnTime = Time.time + spawnDelay;
        nextNIRswitch = Time.time + NIRswitchDelay;
    }

    private void SwitchToNIR()
    {
        for (int i = 0; i < plantNumber; i++)
        {
            newPlant[i].GetComponent<SpawnerAndSwitch>().SwitchToNIR();
        }
        for (int i = 0; i < WeedNumber; i++)
        {
            newWeed[i].GetComponent<SpawnerAndSwitch>().SwitchToNIR();
        }
        newTerrain.GetComponent<SpawnerAndSwitch>().SwitchToNIR();
    }

    private void SwitchToTAG()
    {
        for (int i = 0; i < plantNumber; i++)
        {
            newPlant[i].GetComponent<SpawnerAndSwitch>().SwitchToTAG();
        }
        for (int i = 0; i < WeedNumber; i++)
        {
            newWeed[i].GetComponent<SpawnerAndSwitch>().SwitchToTAG();
        }
        newTerrain.GetComponent<SpawnerAndSwitch>().SwitchToTAG();
    }



    private void SaveRGB() //mode can be type.Image or type.Mask
    {

        RenderTexture rt = new RenderTexture(width, height, 24);
        GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        //string filename = ScreenshotName(mode, field);
        string filename = string.Format("{0}/Dataset/rgb/{1}.png", Application.persistentDataPath, counter);
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    private void SaveNIR() //mode can be type.Image or type.Mask
    {

        RenderTexture rt = new RenderTexture(width, height, 24);
        GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        //string filename = ScreenshotName(mode, field);
        string filename = string.Format("{0}/Dataset/nir/{1}.png", Application.persistentDataPath, counter);
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    private void SaveTAG() //mode can be type.Image or type.Mask
    {

        RenderTexture rt = new RenderTexture(width, height, 24);
        GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        //string filename = ScreenshotName(mode, field);
        string filename = string.Format("{0}/Dataset/tag/{1}.png", Application.persistentDataPath, counter);
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    private void RandomLightAndPosition()
    {
        field f = new field();
        DefinePositions();

        for (int i = 0; i < 10; i++)
        {
            //transform.position = (Vector3)positions[i];
            //transform.rotation = (Quaternion)rotations[i];
            if (i == 0)
            {
                f = field.A;
            }
            else if (i == 1)
            {
                f = field.B;
            }
            else if (i == 2)
            {
                f = field.C;
            }
            else if (i == 3)
            {
                f = field.D;
            }
            else if (i == 4)
            {
                f = field.E;
            }
            else if (i == 5)
            {
                f = field.F;
            }
            else if (i == 6)
            {
                f = field.G;
            }
            else if (i == 7)
            {
                f = field.H;
            }
            else if (i == 8)
            {
                f = field.I;
            }
            else if (i == 9)
            {
                f = field.L;
            }

            //change light
            GameObject myLight = GameObject.Find("Directional Light");
            if (myLight)
            {
                print("change light");
                myLight.GetComponent<RandomLight>().changeLight_intensity();
            }
            /*
            if (TakeScreenshots)
            { TakeShot(type.Image, f); }
            */
        }
        for (int i = 0; i < 10; i++)
        {
            //transform.position = (Vector3)positions[i];
            //transform.rotation = (Quaternion)rotations[i];
            if (i == 0)
            {
                f = field.A;
            }
            else if (i == 1)
            {
                f = field.B;
            }
            else if (i == 2)
            {
                f = field.C;
            }
            else if (i == 3)
            {
                f = field.D;
            }
            else if (i == 4)
            {
                f = field.E;
            }
            else if (i == 5)
            {
                f = field.F;
            }
            else if (i == 6)
            {
                f = field.G;
            }
            else if (i == 7)
            {
                f = field.H;
            }
            else if (i == 8)
            {
                f = field.I;
            }
            else if (i == 9)
            {
                f = field.L;
            }
            if (SaveBoxes)
            {
                saveMasks(type.Mask, f);
            }
        }
    }

    private void DefinePositions()
    {
        positions.Clear();
        positions.Add(transform.position);
        for (int i = 0; i < 9; i++)
        {
            positions.Add(transform.position + new Vector3(UnityEngine.Random.Range(-0.15f, +0.15f), 0f, UnityEngine.Random.Range(-0.15f, +0.15f)));
        }

        rotations.Clear();
        rotations.Add(Quaternion.Euler(cameraRot));
        for (int i = 0; i < 9; i++)
        {
            rotations.Add(Quaternion.Euler(cameraRot + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, UnityEngine.Random.Range(-5f, 5f))));
        }
    }

    private void SpawnTerrain()
    {
        spawnPoint = defaultPos;
        rotation = zeroRot;
        newTerrain = Instantiate(terrain, zeroPos, zeroRot);
        newTerrain.transform.localScale = defaultScale;
    }

    private Vector3 RandomPosition()
    {
        Vector3 ret = new Vector3(UnityEngine.Random.Range(-4.1f, -3.9f), 0f, UnityEngine.Random.Range(-4.1f, -3.9f));
        return ret;
    }

    private Vector3 RandomScale()
    {
        Vector3 ret = new Vector3(UnityEngine.Random.Range(minScaleValue, maxScaleValue), UnityEngine.Random.Range(minScaleValue, maxScaleValue), UnityEngine.Random.Range(minScaleValue, maxScaleValue));
        return ret;
    }

    private Quaternion RandomRotation()
    {
        Quaternion ret = Quaternion.Euler(UnityEngine.Random.Range(-30f, 30f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(-30f, 30f));
        return ret;
    }

    /////////////////////////////////////////////////////////////
    //////////////// Annotations generators /////////////////////
    /////////////////////////////////////////////////////////////

    private void changeColor(GameObject go, float min, float max)
    {
        //Color newColor = new Color(0f, UnityEngine.Random.Range(min, max), 0f, 1f);

        Renderer[] children;
        children = go.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in children)
        {
            Color newColor = new Color(UnityEngine.Random.Range(0.4f, 0.8f), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(0f, 0.4f), 1f);
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = rend.materials[j];
                mats[j].SetColor("_Color", newColor);
            }
            rend.materials = mats;
        }
    }

    private void changeMaterial(Material newMat, GameObject go)
    {
        Renderer[] children;
        children = go.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = newMat;
            }
            rend.materials = mats;
        }
    }

    private void saveMasks(type type, field field)
    {
        if (TakeScreenshots)
        { 
            /*
            GameObject[] objects = new GameObject[plantNumber + WeedNumber];
            newPlant.CopyTo(objects, 0);
            newWeed.CopyTo(objects, newPlant.Length);
           
            changeMaterial(PlainBlack, newTerrain);

            for (int i = 0; i < plantNumber + WeedNumber; i++)
            {
                if (objects[i] != null)
                {
                    if (i < plantNumber)
                    {
                        changeMaterial(green, objects[i]);
                    }
                    else
                    {
                        red = GalliumRed;
                        foreach (Transform child in objects[i].transform)
                        {
                            if (child.tag == "Capsella")
                                red = CapsellaRed;
                            break;
                        }

                        changeMaterial(red, objects[i]);
                    }
                }
            }
            */
            TakeShot(type, field);
        }
    }

    private string ScreenshotName(type name, field field)
    {
        string ret;
        if (name == type.Image)
        {
            ret = string.Format("{0}/Dataset/Field{2}/rgb/{1}.png", Application.persistentDataPath, counter, field);
        }
        else if (name == type.Mask)
        {
            ret = string.Format("{0}/Dataset/Field{2}/nir/{1}.png", Application.persistentDataPath, counter, field);
        }
        else
        {
            ret = string.Format("{0}/Dataset/Field{2}/lbl/{1}.txt", Application.persistentDataPath, counter, field);
        }
        return ret;
    }

    private void TakeShot(type mode, field field) //mode can be type.Image or type.Mask
    {

        RenderTexture rt = new RenderTexture(width, height, 24);
        GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenshotName(mode, field);
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    private string GUIRectWithObject(GameObject go, cls cls) //compute bounding box from camera view
    {
        Renderer[] rr = go.GetComponentsInChildren<Renderer>();
        Bounds b = rr[0].bounds;
        foreach (Renderer r in rr) { b.Encapsulate(r.bounds); }
        Vector3 cen = b.center;
        Vector3 ext = b.extents;

        //Vector3 cen = go.GetComponent<Renderer>().bounds.center;
        //Vector3 ext = go.GetComponent<Renderer>().bounds.extents;
        Vector2[] extentPoints = new Vector2[8]
        {
         HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
         HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
         HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
         HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
         HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
         HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
         HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
         HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
        };
        Vector2 min = extentPoints[0];
        Vector2 max = extentPoints[0];
        foreach (Vector2 v in extentPoints)
        {
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }

        // transform min and manx in strings read to be save
        float x, y, w, h;
        x = Mathf.Clamp(min.x, 0f, width - 1f);
        y = Mathf.Clamp(min.y, 0f, height - 1f);
        w = Mathf.Clamp(max.x, 0f, width - 1f);
        h = Mathf.Clamp(max.y, 0f, height - 1f);

        string species = null;
        if (cls == cls.Crop)
        {
            species = "1";
        }
        else
        {
            species = "0";
        }
        return species + " " + x.ToString() + " " + y.ToString() + " " + w.ToString() + " " + h.ToString();
    }

    private void SaveBoundingBox(string[] content)
    {
        string filename = ScreenshotName(type.Box, field.A);
        File.WriteAllLines(filename, content);
    }

    private void GenerateBoundingBoxes()
    {
        if (SaveBoxes)
        {
            GameObject[] objects = new GameObject[plantNumber + WeedNumber];
            newPlant.CopyTo(objects, 0);
            newWeed.CopyTo(objects, newPlant.Length);

            for (int i = 0; i < objects.Length; i++)
            {
                if (i < plantNumber)
                {
                    if (objects[i] != null)
                    {
                        boxes[i] = GUIRectWithObject(objects[i], cls.Crop);
                    }
                }
                else
                {
                    boxes[i] = GUIRectWithObject(objects[i], cls.Weed);
                }
            }

            SaveBoundingBox(boxes);
        }
    }

    /////////////////////////////////////////////////////////////
    //////////////// Plants generators //////////////////////////
    /////////////////////////////////////////////////////////////

    
    


    public GameObject SpawnGoodPlant(Vector3 position)
    {

        GameObject createdPrefabPlant = Instantiate(goodPlant, position, Quaternion.Euler(0, 0, 0));
        //createdPrefabPlant.AddComponent<MeshRenderer>();
        //Debug.Log(position.ToString());

        return createdPrefabPlant;
    }


    public GameObject SpawnWeedPlant(Vector3 position)
    {
        int plantChosen=Random.Range(0, weedPlants.Length);
        float rot_x = Random.Range(weedPlants_Rotation_Min[plantChosen].x, weedPlants_Rotation_Max[plantChosen].x);
        float rot_y = Random.Range(weedPlants_Rotation_Min[plantChosen].y, weedPlants_Rotation_Max[plantChosen].y);
        float rot_z = Random.Range(weedPlants_Rotation_Min[plantChosen].z, weedPlants_Rotation_Max[plantChosen].z);
        Vector3 weedRotation= new Vector3(rot_x, rot_y, rot_z);
        //GameObject createdPrefabPlant = Instantiate(weedPlants[plantChosen], position+weedPlants_Offset[plantChosen], Quaternion.Euler(weedRotation));
        GameObject createdPrefabPlant = Instantiate(weedPlants[plantChosen], position + weedPlants_Offset[plantChosen], Quaternion.Euler(weedRotation) * weedPlants[plantChosen].transform.rotation);

        createdPrefabPlant.transform.localScale = weedPlants_Scale[plantChosen];
        //Debug.Log(position.ToString());

        return createdPrefabPlant;
    }



    /*
    GameObject SpawnCapsella()
    {
        GameObject createdPrefabStem = new GameObject();
        GameObject leaf;

        // Leaf Spawn
        Vector3 tempPosition = new Vector3(UnityEngine.Random.Range(-4.1f, 2f), 0.9f, UnityEngine.Random.Range(-4.1f, 2f));
        //Vector3 noise = new Vector3(UnityEngine.Random.Range(-0.5f, 2f), 0.9f, UnityEngine.Random.Range(0.5f, 2f));
        //Vector3 tempPosition = RandomPosition()+noise;
        for (int l = 0; l < 2; l++)
        {
            for (int x = 0; x < capsellaLeafAmount; x++)
            {
                leaf = SelectCapsellaLeaf();

                //TODO modify leaf geometry and displacement, maybe done?

                randomRotationValue = new Vector3(-50f, x * 45f, 60f);
                capsellaLeafRotation[1] += UnityEngine.Random.Range(-10.0f, 10.0f);
                newRotation = Quaternion.Euler(capsellaLeafRotation + randomRotationValue);

                GameObject createdPrefabLeaf = Instantiate(leaf, tempPosition, newRotation);
                if (UnityEngine.Random.Range(0f, 10f) < 9f)
                {
                    createdPrefabLeaf.transform.localScale = capsellaLeafScale / (l + 1) * UnityEngine.Random.Range(0.5f, 1f);
                }
                else
                {
                    createdPrefabLeaf.transform.localScale = capsellaLeafScale * 0;
                }

                createdPrefabLeaf.AddComponent<MeshRenderer>();
                createdPrefabLeaf.transform.SetParent(createdPrefabStem.transform);
            }
        }
        return createdPrefabStem;
    }
    */
    /*
    GameObject SelectCapsellaLeaf()
    {
        GameObject ret;
        int n = UnityEngine.Random.Range(1, 10);
        switch (n)
        {
            case 1:
                ret = capsellaLeaf1;
                break;
            case 2:
                ret = capsellaLeaf2;
                break;
            case 3:
                ret = capsellaLeaf3;
                break;
            case 4:
                ret = capsellaLeaf4;
                break;
            case 5:
                ret = capsellaLeaf5;
                break;
            case 6:
                ret = capsellaLeaf6;
                break;
            case 7:
                ret = capsellaLeaf6;
                break;
            case 8:
                ret = capsellaLeaf8;
                break;
            case 9:
                ret = capsellaLeaf9;
                break;
            default:
                ret = capsellaLeaf2;
                break;
        }

        return ret;
    }
    */
    ///////////////////////////////////////////////////////////////////////////////
    /// SINGLE MASK GENERATION TO EXTRACT BOXES ///////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////

    private void SingleMaskScreenshot(int idx, species cls, field field)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = SingleMaskScreenshotName(idx, cls, field);
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    private string SingleMaskScreenshotName(int idx, species cls, field field)
    {
        return string.Format("{0}/Dataset/Field{4}/Boxes/{1}_{2}_{3}.png", Application.persistentDataPath, counter, idx, cls, field);
    }


}