using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;

public class SwarmDatasetGeneration
 : MonoBehaviour
{
    public GameObject beetLeaf;
    public Vector3 beetLeafScale = new Vector3(1, 1, 1);

    public GameObject galliumLeaf;
    public Vector3 galliumLeafScale = new Vector3(1, 1, 1);
    public GameObject galliumSteam;
    public Vector3 galliumStemScale = new Vector3(1, 1, 1);

    public GameObject capsellaLeaf1;
    public GameObject capsellaLeaf2;
    public GameObject capsellaLeaf3;
    public GameObject capsellaLeaf4;
    public GameObject capsellaLeaf5;
    public GameObject capsellaLeaf6;
    public GameObject capsellaLeaf7;
    public GameObject capsellaLeaf8;
    public GameObject capsellaLeaf9;
    public Vector3 capsellaLeafScale = new Vector3(1, 1, 1);

    // materials for ground truths
    public Material CapsellaRed;
    public Material GalliumRed;
    public Material BeetBlack;
    public Material CapsellaBlack;
    public Material GalliumBlack;
    public Material PlainBlack;
    public Material green;
    public Material white;

    public GameObject terrain;

    private Material red;
    private Material black;
    // to select the kind of annotation to be generated
    public bool TakeScreenshots = true;
    public bool SaveBoxes = false;

    private Quaternion newRotation;
    private Vector3 randomRotationValue;

    private static int BeetNumber = 84;//9;//65;
    private static int cropRows = 6;//3;//5;

    //private static int CapsellaNumber = 8;
    //private static int GalliumNumber = 8;
    private static int WeedInit = 14;//5;//20;
    int WeedNumber = WeedInit;

    string[] boxes = new string[BeetNumber + WeedInit];

    // object instances 
    private GameObject[] newBeet = new GameObject[BeetNumber];
    private GameObject[] newWeed = new GameObject[WeedInit];
    //private GameObject[] newCapsella = new GameObject[CapsellaNumber];
    //private GameObject[] newGallium = new GameObject[GalliumNumber];
    private GameObject newTerrain;

    private int beetLeafAmount = 7;
    private int galliumLeafAmount = 5;
    private int capsellaLeafAmount = 8;

    // control the spawing ratio, remember to check also Invoke() functions
    private float spawnDelay = 3f;
    private float nextSpawnTime = 0f;

    // range for spawing objects
    private float minScaleValue = 0.2f;
    private float maxScaleValue = 1f;

    // control missing beet ratio
    private float missBeet = 10f;
    // control gallium/capsella ratio
    private float weedType = 7.5f;

    // counter to save pictures incrementally
    private static int imgPerWeedNumber = 5;
    private int counter = WeedInit * imgPerWeedNumber;


    // camera resolution
    private int width = 1024;
    private int height = 1024;

    Vector3 spawnPoint;
    Vector3 scaleFactor;

    Vector3 zeroPos = new Vector3(0f, 0.4f, 0f);
    Vector3 cameraPos = new Vector3(-1f, 7f, -0.7f);
    Vector3 cameraRot = new Vector3(90, 0, 0);
    Vector3 defaultScale = new Vector3(25f, 1f, 25f);
    Vector3 defaultPos = new Vector3(0f, 0.5f, 0f); //default terrain dimensions
    Vector3 beetLeafRotation = new Vector3(200f, 0f, -90f);
    Vector3 galliumLeafRotation = new Vector3(-90, 0f, 0f);
    Vector3 capsellaLeafRotation = new Vector3(0f, 0f, 0f);

    Quaternion zeroRot = Quaternion.Euler(0, 0, 0);
    Quaternion rotation;

    // control where to save pictures
    public enum type { Image, Mask, Box };
    public enum cls { Crop, Weed };
    public enum field { A, B, C, D, E, F, G, H, I, L };
    public enum species { Beet, Gall, Caps };

    private species[] specs = new species[BeetNumber + WeedInit];
    public ArrayList positions = new ArrayList();
    public ArrayList rotations = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldSpawn())
        {
            print(counter);
            if (counter == (WeedInit * imgPerWeedNumber) + imgPerWeedNumber)
            {
                TakeScreenshots = false;
            }
            if (counter == (WeedNumber + 1) * imgPerWeedNumber)
            {
                WeedNumber++;
                Array.Resize(ref boxes, BeetNumber + WeedNumber);
                Array.Resize(ref newWeed, WeedNumber);
                print(WeedNumber);
            }
            //print(Application.persistentDataPath);
            SpawnTerrain();
            Spawn();
            //Invoke("saveSingleMasks", 1f);
            //Invoke("saveMasks", 0.5f);
            Invoke("clearScene", 2f);
        }
    }

    private void CounterUpdate()
    {
        counter++;
    }

    private void clearScene()
    {
        Destroy(newTerrain);
        for (int i = 0; i < BeetNumber; i++)
        {
            if (newBeet[i] != null)
            {
                Destroy(newBeet[i]);
            }
        }
        for (int i = 0; i < WeedNumber; i++)
        {
            Destroy(newWeed[i]);
        }
        CounterUpdate();
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
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
            for (int k = 0; k < BeetNumber / cropRows; k++)
            {
                if (UnityEngine.Random.Range(0f, 10f) >= missBeet)
                {
                    newBeet[cnt] = null;
                    specs[cnt] = species.Beet;
                }
                else
                {
                    newBeet[cnt] = SpawnBeet(pos + new Vector3(0f, 0.1f, 0f));//SpawnBeet(pos + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0.1f, UnityEngine.Random.Range(-0.2f, 0.2f)));
                    changeColor(newBeet[cnt], 0.5f, 0.8f);
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
            if (UnityEngine.Random.Range(0f, 10f) >= weedType)
            {
                newWeed[i] = SpawnGallium();
                specs[BeetNumber + i] = species.Gall;
                changeColor(newWeed[i], 0.5f, 0.6f);
            }
            else
            {
                newWeed[i] = SpawnCapsella();
                specs[BeetNumber + i] = species.Caps;
                changeColor(newWeed[i], 0.5f, 0.8f);
            }
        }

        if (TakeScreenshots)
        {
            print("shot");
            RandomLightAndPosition();
        }

        nextSpawnTime = Time.time + spawnDelay;
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
            TakeShot(type.Image, f);
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
            saveMasks(type.Mask, f);
            saveSingleMasks(f);

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
            GameObject[] objects = new GameObject[BeetNumber + WeedNumber];
            newBeet.CopyTo(objects, 0);
            newWeed.CopyTo(objects, newBeet.Length);

            changeMaterial(PlainBlack, newTerrain);

            for (int i = 0; i < BeetNumber + WeedNumber; i++)
            {
                if (objects[i] != null)
                {
                    if (i < BeetNumber)
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

            TakeShot(type, field);
        }
    }

    private string ScreenshotName(type name, field field)
    {
        string ret;
        if (name == type.Image)
        {
            ret = string.Format("{0}/Dataset/Field{2}/Images/{1}.png", Application.persistentDataPath, counter, field);
        }
        else if (name == type.Mask)
        {
            ret = string.Format("{0}/Dataset/Field{2}/Masks/{1}.png", Application.persistentDataPath, counter, field);
        }
        else
        {
            ret = string.Format("{0}/Dataset/Field{2}/Boxes/{1}.txt", Application.persistentDataPath, counter, field);
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
	#if UNITY_EDITOR	
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
	#else
	return "none";
	#endif

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
            GameObject[] objects = new GameObject[BeetNumber + WeedNumber];
            newBeet.CopyTo(objects, 0);
            newWeed.CopyTo(objects, newBeet.Length);

            for (int i = 0; i < objects.Length; i++)
            {
                if (i < BeetNumber)
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

    GameObject SpawnBeet(Vector3 position)
    {
        GameObject createdPrefabStem = new GameObject();

        // Leaf Spawn
        Vector3 tempPosition = position;
        tempPosition[1] += 0.8f;

        for (int x = 0; x < beetLeafAmount; x++)
        {
            //TODO modify leaf geometry and displacement, maybe done?
            randomRotationValue = new Vector3(0f, x * 50f, 0f);
            beetLeafRotation[1] += UnityEngine.Random.Range(-16.0f, 16.0f);
            newRotation = Quaternion.Euler(beetLeafRotation + randomRotationValue);

            GameObject createdPrefabLeaf = Instantiate(beetLeaf, tempPosition, newRotation);
            if (UnityEngine.Random.Range(0f, 10f) < 9.5f)
            {
                createdPrefabLeaf.transform.localScale = beetLeafScale * UnityEngine.Random.Range(0.5f, 1f);
            }
            else
            {
                createdPrefabLeaf.transform.localScale = beetLeafScale * 0;
            }
            createdPrefabLeaf.AddComponent<MeshRenderer>();
            createdPrefabLeaf.transform.SetParent(createdPrefabStem.transform);
        }

        return createdPrefabStem;
    }

    GameObject SpawnGallium()
    {
        GameObject createdPrefabStem = new GameObject();

        // Leaf Spawn
        Vector3 tempPosition = new Vector3(UnityEngine.Random.Range(-4.1f, 2f), 0.8f, UnityEngine.Random.Range(-4.1f, 2f));
        //Vector3 noise = new Vector3(UnityEngine.Random.Range(-0.5f, 1f), 0.8f, UnityEngine.Random.Range(0.5f, 1f));
        //Vector3 tempPosition = RandomPosition()+noise;
        GameObject stem = Instantiate(galliumSteam, tempPosition, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        stem.transform.localScale = galliumStemScale;
        stem.transform.SetParent(createdPrefabStem.transform);

        for (int l = 0; l < 2; l++)
        {
            Vector3 layerPos = tempPosition;
            layerPos[1] += 0.2f * (l + 1);

            for (int x = 0; x < galliumLeafAmount; x++)
            {
                //TODO modify leaf geometry and displacement, maybe done?
                randomRotationValue = new Vector3(0f, 0f, x * 72f);
                galliumLeafRotation[1] += UnityEngine.Random.Range(-16.0f, 16.0f);
                newRotation = Quaternion.Euler(galliumLeafRotation + randomRotationValue);

                GameObject createdPrefabLeaf = Instantiate(galliumLeaf, layerPos, newRotation);
                if (UnityEngine.Random.Range(0f, 10f) < 9f)
                {
                    createdPrefabLeaf.transform.localScale = galliumLeafScale / (l + 1) * UnityEngine.Random.Range(0.5f, 1f);
                }
                else
                {
                    createdPrefabLeaf.transform.localScale = galliumLeafScale * 0;
                }
                createdPrefabLeaf.AddComponent<MeshRenderer>();
                createdPrefabLeaf.transform.SetParent(createdPrefabStem.transform);
            }
        }


        return createdPrefabStem;
    }

    /*GameObject SpawnCapsella()
    {
        GameObject createdPrefabStem = new GameObject();
        GameObject leaf;

        // Leaf Spawn
        Vector3 tempPosition = new Vector3(UnityEngine.Random.Range(-4.1f, 1.8f), 0.9f, UnityEngine.Random.Range(-4.1f, 1.8f));

        for (int l = 0; l < 2; l++)
        {
            for (int x = 0; x < capsellaLeafAmount; x++)
            {
                leaf = SelectCapsellaLeaf();

                //TODO modify leaf geometry and displacement, maybe done?

                randomRotationValue = new Vector3(0f, x * 45f, 90f);
                capsellaLeafRotation[1] += UnityEngine.Random.Range(-10.0f, 10.0f);
                newRotation = Quaternion.Euler(capsellaLeafRotation+ randomRotationValue);

                GameObject createdPrefabLeaf = Instantiate(leaf, tempPosition, newRotation);
                if (UnityEngine.Random.Range(0f, 10f) < 9f)
                {
                    createdPrefabLeaf.transform.localScale = capsellaLeafScale / (l + 1) * UnityEngine.Random.Range(0.5f, 0.8f);
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
    }*/

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

    private void saveSingleMasks(field field)
    {
        if (SaveBoxes)
        {
            GameObject[] objects = new GameObject[BeetNumber + WeedNumber];
            newBeet.CopyTo(objects, 0);
            newWeed.CopyTo(objects, newBeet.Length);

            changeMaterial(white, newTerrain);

            for (int i = 0; i < BeetNumber + WeedNumber; i++)
            {
                if (objects[i] != null)
                {
                    if (i < BeetNumber)
                    {
                        changeMaterial(BeetBlack, objects[i]);
                    }
                    else
                    {
                        black = GalliumBlack;
                        foreach (Transform child in objects[i].transform)
                        {
                            if (child.tag == "Capsella")
                                black = CapsellaBlack;
                            break;
                        }

                        changeMaterial(black, objects[i]);
                    }

                    for (int j = 0; j < BeetNumber + WeedNumber; j++)
                    {
                        if (i != j && objects[j] != null)
                        {
                            changeMaterial(white, objects[j]);
                        }
                    }

                    SingleMaskScreenshot(i, specs[i], field);
                }
            }
        }
    }
}
