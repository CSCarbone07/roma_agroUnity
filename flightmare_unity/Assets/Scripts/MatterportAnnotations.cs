using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class MatterportAnnotations : MonoBehaviour
{
    [SerializeField] GameObject beetLeaf;
    [SerializeField] Vector3 beetLeafScale = new Vector3(1, 1, 1);

    [SerializeField] GameObject galliumLeaf;
    [SerializeField] Vector3 galliumLeafScale = new Vector3(1, 1, 1);
    [SerializeField] GameObject galliumSteam;
    [SerializeField] Vector3 galliumStemScale = new Vector3(1, 1, 1);

    [SerializeField] GameObject capsellaLeaf1;
    [SerializeField] GameObject capsellaLeaf2;
    [SerializeField] GameObject capsellaLeaf3;
    [SerializeField] GameObject capsellaLeaf4;
    [SerializeField] GameObject capsellaLeaf5;
    [SerializeField] GameObject capsellaLeaf6;
    [SerializeField] GameObject capsellaLeaf7;
    [SerializeField] GameObject capsellaLeaf8;
    [SerializeField] GameObject capsellaLeaf9;
    [SerializeField] Vector3 capsellaLeafScale = new Vector3(1, 1, 1);

    // materials for ground truths
    [SerializeField] Material white;
    [SerializeField] Material black;
    [SerializeField] Material bendedBlack;

    [SerializeField] GameObject terrain;

    // to select the kind of annotation to be generated
    public bool TakeScreenshots = false;
    public bool SaveBoxes = false;

    private Quaternion newRotation;
    private Vector3 randomRotationValue;

    private static int BeetNumber = 35; //35
    private static int cropRows = 5; //5

    //private static int CapsellaNumber = 8;
    //private static int GalliumNumber = 8;
    private static int WeedInit = 20;
    int WeedNumber = WeedInit;

    string[] boxes = new string[BeetNumber + WeedInit];

    // object instances 
    private GameObject[] newBeet = new GameObject[BeetNumber];
    private GameObject[] newWeed = new GameObject[WeedInit];
    private spec[] specs = new spec[BeetNumber + WeedInit];
    //private GameObject[] newCapsella = new GameObject[CapsellaNumber];
    //private GameObject[] newGallium = new GameObject[GalliumNumber];
    private List<GameObject> newTerrain = new List<GameObject>();

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
    private float missBeet = 9f;
    // control gallium/capsella ratio
    private float weedType = 7.5f;

    // counter to save pictures incrementally
    private static int imgPerWeedNumber = 50;
    private int counter = WeedInit * imgPerWeedNumber;


    // camera resolution
    private int width = 1024;
    private int height = 1024;

    Vector3 spawnPoint;
    Vector3 scaleFactor;

    Vector3 zeroPos = new Vector3(0f, 0.3f, 0f);
    Vector3 defaultScale = new Vector3(25f, 1f, 25f);
    Vector3 defaultPos = new Vector3(-5f, 0.15f, -5f); //default terrain dimensions
    Vector3 beetLeafRotation = new Vector3(200f, 0f, -90f);
    Vector3 galliumLeafRotation = new Vector3(-90, 0f, 0f);
    Vector3 capsellaLeafRotation = new Vector3(-90, 0f, 0f);

    Quaternion zeroRot = Quaternion.Euler(0, 0, 0);
    Quaternion rotation;

    // control where to save pictures
    public enum type { Image, Mask, Box };
    public enum cls { Crop, Weed };
    public enum spec { Crop, Gall, Caps };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldSpawn())
        {
            if (counter == (WeedInit * imgPerWeedNumber) + imgPerWeedNumber)
            {
                TakeScreenshots = false;
            }
            if (counter == (WeedNumber+1)*imgPerWeedNumber)
            {
                WeedNumber++;
                Array.Resize(ref boxes, BeetNumber + WeedNumber);
                Array.Resize(ref newWeed, WeedNumber);
                Array.Resize(ref specs, BeetNumber + WeedNumber);
                print(WeedNumber);
                print(counter);
            }
            Terrain();
            Spawn();
            Invoke("GenerateBoundingBoxes", 0.5f);
            Invoke("saveMasks", 1f);
            Invoke("clearScene", 2f);
        }
    }

    private void CounterUpdate()
    {
        counter++;
    }

    private void clearScene()
    {
        for (int i = 0; i < newTerrain.Count; i++)
        {
            Destroy(newTerrain[i]);
        }

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
        int counter = 0;
        float x_offset = 1.5f;
        float z_offset = 1f;

        Vector3 pos = RandomPosition();
        Vector3 start_pos = pos;

        for (int j = 0; j < cropRows; j++)
        {
            for (int k = 0; k < BeetNumber / cropRows; k++) 
            {
                if (UnityEngine.Random.Range(0f, 10f) >= missBeet)
                {
                    newBeet[counter] = null;
                    specs[counter] = spec.Crop;
                }
                else
                {
                    //newBeet[counter] = SpawnBeet(pos + new Vector3( UnityEngine.Random.Range(-0.2f, 0.2f), 0.1f, UnityEngine.Random.Range(-0.2f, 0.2f)));
                    newBeet[counter] = SpawnBeet(pos);
                    specs[counter] = spec.Crop;
                    changeColor(newBeet[counter], 0.4f, 0.9f);
                }
                counter++;
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
                specs[BeetNumber + i] = spec.Gall;
                changeColor(newWeed[i], 0.2f, 0.6f);
            }
            else
            {
                newWeed[i] = SpawnCapsella();
                specs[BeetNumber + i] = spec.Caps;
                changeColor(newWeed[i], 0.4f, 0.9f);
            }
        }

        if (TakeScreenshots)
        {
            TakeShot(type.Image);
        }

        nextSpawnTime = Time.time + spawnDelay;
    }

    /*private void SpawnTerrain()
    {
        spawnPoint = defaultPos;
        rotation = zeroRot;
        newTerrain = Instantiate(terrain, zeroPos, zeroRot);
        newTerrain.transform.localScale = defaultScale;
    }*/

    private void Terrain()
    {
    
        int gridY = 15;
        int gridX = 15;

        Vector3 Scale = new Vector3(1f,1f,1f);
        float spacingX = 0.75f;
        float spacingY = 1f;

        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                //randomRotationValue = new Vector3(Random.Range(-90.0f, 90.0f)* addRandomRotationX, Random.Range(-90.0f, 90.0f) * addRandomRotationY, Random.Range(-90.0f, 90.0f) * addRandomRotationZ));
                //addRandomRotation = new Vector3(addRandomRotationX, addRandomRotationY, addRandomRotationZ);
                //randomRotationValue = addRandomRotation * (Random.Range(-90.0f, 90.0f));
                //newRotation = Quaternion.Euler(Rotation + randomRotationValue);

                Vector3 pos = new Vector3(x * spacingX, 0, y * spacingY) + defaultPos;

                GameObject createdPrefab = Instantiate(terrain, pos, zeroRot);
                //createdPrefab.transform.SetParent(newTerrain.gameObject.transform); // = this.transform;
                                                                              //prefab.transform.parent = transform;
                createdPrefab.transform.localScale = Scale + new Vector3(UnityEngine.Random.Range(0.0f, .5f), UnityEngine.Random.Range(0.0f, .2f), UnityEngine.Random.Range(0.0f, .5f));
                newTerrain.Add(createdPrefab);
            }
        }
    }

    private Vector3 RandomPosition()
    { 
        //Vector3 ret = new Vector3(UnityEngine.Random.Range(-4.1f, -3.9f), 0f, UnityEngine.Random.Range(-4.1f,-3.9f));
        Vector3 ret = new Vector3(UnityEngine.Random.Range(-4.1f, -3.9f), 0f, -3.9f);
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

    private void changeTerrainMaterial(Material newMat, List<GameObject> gos)
    {
        foreach (GameObject go in gos)
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
    }

    private void changeColor(GameObject go, float min, float max)
    {
        //Color newColor = new Color(UnityEngine.Random.Range(0f, 0.33f), UnityEngine.Random.Range(0.6f, 0.8f), UnityEngine.Random.Range(0f, 0.33f), 1f);
        Color newColor = new Color(0f, UnityEngine.Random.Range(min, max), 0f, 1f);

        Renderer[] children;
        children = go.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = rend.materials[j];
                mats[j].SetColor("_Color",newColor);
            }
            rend.materials = mats;
        }
    }

    private void saveMasks()
    {
        if(TakeScreenshots)
        {
            GameObject[] objects = new GameObject[BeetNumber + WeedNumber];
            newBeet.CopyTo(objects, 0);
            newWeed.CopyTo(objects, newBeet.Length);

            changeTerrainMaterial(white, newTerrain);

            for (int i = 0; i < BeetNumber + WeedNumber; i++)
            {
                if (objects[i] != null)
                {
                    if ( i < BeetNumber)
                    {
                    changeMaterial(bendedBlack, objects[i]);
                    }
                    else
                    {
                        changeMaterial(black, objects[i]);
                    }

                    for (int j = 0; j < BeetNumber + WeedNumber; j++)
                    {
                        if (i != j && objects[j] != null)
                        {
                            changeMaterial(white, objects[j]);
                        }
                    }

                    MatterPortScreenshot(i,specs[i]);
                }
            }
        }
    }

    private void MatterPortScreenshot(int idx, spec spec)
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
        string filename = MatterPortScreenshotName(idx, spec);
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    private string MatterPortScreenshotName(int idx, spec spec)
    {
        return string.Format("{0}/Dataset/Matterport/{1}_{2}_{3}.png", Application.persistentDataPath, counter, idx, spec);
    }


    private string ScreenshotName(type name)
    {
        string ret;
        if (name == type.Image)
        {
            ret = string.Format("{0}/Dataset/Images/{1}.png", Application.persistentDataPath, counter);
        }
        else if (name == type.Mask)
        {
            ret = string.Format("{0}/Dataset/Masks/{1}.png", Application.persistentDataPath, counter);
        }
        else
        {
            ret = string.Format("{0}/Dataset/Boxes/{1}.txt", Application.persistentDataPath, counter);
        }
        return ret;
    }

    private void TakeShot(type mode) //mode can be type.Image or type.Mask
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
        string filename = ScreenshotName(mode);
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
        int x, y, w, h;
        x = (int)Mathf.Clamp(min.x, 0f, width-1f);
        y = (int)Mathf.Clamp(min.y, 0f, height - 1f);
        w = (int)Mathf.Clamp(max.x, 0f, width - 1f);
        h = (int)Mathf.Clamp(max.y, 0f, height - 1f);

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
        string filename = ScreenshotName(type.Box);
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
        /*GameObject createdPrefabStem = new GameObject();
        createdPrefabStem = Instantiate(beetLeaf, tempPosition, newRotation);*/

        // Leaf Spawn
        Vector3 tempPosition = position;
        tempPosition[1] += 0.5f;

        GameObject createdPrefabStem = new GameObject();
        createdPrefabStem = Instantiate(beetLeaf, tempPosition, newRotation);

        for (int x = 0; x < beetLeafAmount; x++)
        {
            //TODO modify leaf geometry and displacement, maybe done?
            randomRotationValue = new Vector3(0f, x * 50f, 0f);
            beetLeafRotation[1] += UnityEngine.Random.Range(-16.0f, 16.0f);
            newRotation = Quaternion.Euler(beetLeafRotation + randomRotationValue);

            GameObject createdPrefabLeaf = Instantiate(beetLeaf, tempPosition, newRotation);
            if (UnityEngine.Random.Range(0f, 10f) < 9f)
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

        createdPrefabStem.transform.localScale *= UnityEngine.Random.Range(0.9f, 1.1f);
        return createdPrefabStem;


    }

    GameObject SpawnGallium()
    {
        GameObject createdPrefabStem = new GameObject();

        // Leaf Spawn
        Vector3 tempPosition = new Vector3(UnityEngine.Random.Range(-4.1f, 1.8f), 0.5f, UnityEngine.Random.Range(-4.1f, 1.8f));
        GameObject stem = Instantiate(galliumSteam, tempPosition, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        stem.transform.localScale = galliumStemScale;
        stem.transform.SetParent(createdPrefabStem.transform);

        for (int l = 0; l < 2; l++)
        {
            Vector3 layerPos = tempPosition;
            layerPos[1] += 0.2f*(l + 1);

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

        createdPrefabStem.transform.localScale *= UnityEngine.Random.Range(0.6f, 1f);
        return createdPrefabStem;
    }

    GameObject SpawnCapsella()
    {
        GameObject createdPrefabStem = new GameObject();
        GameObject leaf;

        // Leaf Spawn
        Vector3 tempPosition = new Vector3(UnityEngine.Random.Range(-4.1f, 1.8f), 0.7f, UnityEngine.Random.Range(-4.1f, 1.8f));

        for (int l = 0; l < 2; l++)
        {
            for (int x = 0; x < capsellaLeafAmount; x++)
            {
                leaf = SelectCapsellaLeaf();

                //TODO modify leaf geometry and displacement, maybe done?

                randomRotationValue = new Vector3(50f, x * 45f, 60f);
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
        createdPrefabStem.transform.localScale *= UnityEngine.Random.Range(0.8f, 1.2f);
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
}
