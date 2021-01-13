//using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;

public class PlantGroundSpawn_2 : MonoBehaviour
{
    [SerializeField] GameObject beetLeaf;
    [SerializeField] Vector3 beetLeafScale = new Vector3(1, 1, 1);

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
    [SerializeField] Material green;
    [SerializeField] Material red;
    [SerializeField] Material black;

    [SerializeField] GameObject terrain;

    // to select the kind of annotation to be generated
    [SerializeField] bool TakeScreenshots = false;
    [SerializeField] bool SaveBoxes = false;

    private Quaternion newRotation;
    private Vector3 randomRotationValue;

    private GameObject Light;

    private static int BeetNumber = 4;
    private static int CapsellaNumber = 12;

    // object instances 
    GameObject[] newBeet = new GameObject[BeetNumber];
    GameObject[] newCapsella = new GameObject[CapsellaNumber];
    GameObject newTerrain;

    private int beetLeafAmount = 5;
    private int capsellaLeafAmount = 8;

    // control the spawing ratio, remeber to check also Invoke() functions
    private float spawnDelay = 4f;
    private float nextSpawnTime = 0f;

    // range for spawing objects
    private float minScaleValue = 0.2f;
    private float maxScaleValue = 1f;

    // counter to save pictures incrementally
    private int counter = 0;

    // camera resolution
    private int width = 416;
    private int height = 416;

    string[] boxes = new string[BeetNumber + CapsellaNumber];

    Vector3 spawnPoint;
    Vector3 scaleFactor;

    Vector3 zeroPos = new Vector3(0f, 0.85f, 0f);
    Vector3 defaultScale = new Vector3(10f, 0.8f, 10f); //default terrain dimensions
    Vector3 leafRotation = new Vector3(270f, 0f, 0f);

    Quaternion zeroRot = Quaternion.Euler(0, 0, 0);
    Quaternion rotation;

    // control where to save pictures
    public enum type { Image, Mask, Box };
    public enum cls { Crop, Weed };

    int imgCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Light = GameObject.Find("Directional Light");
        spawnEverything();
        
    }

    IEnumerator spawnWithDelay(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnTerrain();
        Spawn();
        Invoke("GenerateGroundTruth", 1f);
        Invoke("GenerateBoundingBoxes", 2f);
        Invoke("clearScene", 3f);
        imgCount++;
        //Invoke("spawnTerrain", 3f);

    }

    private void spawnEverything()
    {
                     
        print("SpawningEverything");
        ChangeLight();
        SpawnTerrain();
        Spawn();
        Invoke("GenerateGroundTruth", 1f);
        Invoke("GenerateBoundingBoxes", 2f);
        Invoke("clearScene", 3f);
        imgCount++;
        Invoke("spawnEverything", 4f);

    }


    // Update is called once per frame
    void Update()
    {
  
    }

    void ChangeLight()
    {
        //print(Light);
        if (Light)
        {
            Light.GetComponent<LightChanging>().ChangeLightParameters();
        }
    }

    private void CounterUpdate()
    {
        print(counter);
        counter++;
    }

    private void clearScene()
    {
        Destroy(newTerrain);
        for (int i = 0; i < BeetNumber; i++)
        {
            Destroy(newBeet[i]);
        }
        for (int i = 0; i < CapsellaNumber; i++)
        {
            Destroy(newCapsella[i]);
        }
        CounterUpdate();
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }

    private void SpawnTerrain()
    {
        spawnPoint = zeroPos;
        rotation = zeroRot;
        newTerrain = Instantiate(terrain, zeroPos, zeroRot);
        newTerrain.transform.localScale = defaultScale;
    }

    private void Spawn()
    {
        int counter = 0;
        float x_offset = 4f;
        float z_offset = 1.5f;

        Vector3 pos = RandomPosition();

        for (int  j = 0; j < BeetNumber; j++)
        {
            newBeet[counter] = SpawnBeet(pos);
            counter++;
            pos[2] = pos[2] + z_offset;
        }

        
        for (int i = 0; i < CapsellaNumber; i++)
        {
           newCapsella[i] = SpawnCapsella();
        }
        
        if (TakeScreenshots)
        {
            TakeShot(type.Image);
        }

        nextSpawnTime = Time.time + spawnDelay;
    }

    GameObject SpawnBeet(Vector3 position)
    {
        GameObject createdPrefabStem = new GameObject();

        // Leaf Spawn
        Vector3 tempPosition = position;
        tempPosition[1] += 0.5f;

        for (int x = 0; x < beetLeafAmount; x++)
        {
            //TODO modify leaf geometry and displacement, mayne done?
            randomRotationValue = new Vector3(0f, 0f, x * 72f);
            leafRotation[1] += Random.Range(-16.0f, 16.0f);
            newRotation = Quaternion.Euler(leafRotation + randomRotationValue);

            GameObject createdPrefabLeaf = Instantiate(beetLeaf, tempPosition, newRotation);
            createdPrefabLeaf.transform.localScale = beetLeafScale * Random.Range(0.5f, 1f);

            createdPrefabLeaf.AddComponent<MeshRenderer>();
            createdPrefabLeaf.transform.SetParent(createdPrefabStem.transform);
            createdPrefabLeaf.transform.localRotation = Quaternion.Euler(createdPrefabLeaf.transform.localRotation.eulerAngles + new Vector3(0f,0f,0f));
        }

        return createdPrefabStem;
    }


    private Vector3 RandomPosition()
    {
        Vector3 ret = new Vector3(UnityEngine.Random.Range(-2.5f, 2.5f), 1f, UnityEngine.Random.Range(-3f, -1.5f));
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

    private void GenerateGroundTruth()
    {
        newTerrain.GetComponent<MeshRenderer>().material = black;

        for (int i = 0; i < CapsellaNumber; i++)
        {
            changeMaterial(red, newCapsella[i]);
        }
        for (int i = 0; i < BeetNumber; i++)
        {
            changeMaterial(green, newBeet[i]);
        }

        if (TakeScreenshots)
        {
            TakeShot(type.Mask);
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


    private string ScreenshotName(type name)
    {
        string ret;
        if (name == type.Image)
        {
            ret = string.Format("{0}/Dataset/Images/{1}.png", Application.dataPath, counter);
        }
        else if (name == type.Mask)
        {
            ret = string.Format("{0}/Dataset/Masks/{1}.png", Application.dataPath, counter);
        }
        else
        {
            ret = string.Format("{0}/Dataset/Boxes/{1}.txt", Application.dataPath, counter);
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
        //TODO consider also the children object
        // maybe something like:

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
        x = (int) Mathf.Clamp(min.x, 0f, 415f);
        y = (int) Mathf.Clamp(min.y, 0f, 415f);
        w = (int) Mathf.Clamp(max.x, 0f, 415f);
        h = (int) Mathf.Clamp(max.y, 0f, 415f);

        string species = null;
        if(cls == cls.Crop)
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
        //TODO
        string filename = ScreenshotName(type.Box);
        File.WriteAllLines(filename, content);
    }

    private void GenerateBoundingBoxes()
    {
        if (SaveBoxes)
        {
            //TODO save on file
            //values depend on game resolution
            //SaveBoundingBox() how to save .txt files?
            for (int i = 0; i < BeetNumber ; i++)
            {
                boxes[i] = GUIRectWithObject(newBeet[i], cls.Crop);
            }

            for (int i = 0; i < CapsellaNumber; i++)
            {
                boxes[i+BeetNumber] = GUIRectWithObject(newCapsella[i], cls.Weed);
            }

            SaveBoundingBox(boxes);
        }
    }



    GameObject SpawnCapsella()
    {
        GameObject createdPrefabStem = new GameObject();
        GameObject leaf;

        // Leaf Spawn
        Vector3 tempPosition = new Vector3(Random.Range(-3f, 3f), 1.4f, Random.Range(-3f, 3f));

        for (int x = 0; x < capsellaLeafAmount; x++)
        {
            leaf = SelectCapsellaLeaf();

            //TODO modify leaf geometry and displacement, mayne done?
            randomRotationValue = new Vector3(0f, 0f, x * 45f);
            leafRotation[1] += Random.Range(-10.0f, 10.0f);
            newRotation = Quaternion.Euler(leafRotation + randomRotationValue);

            GameObject createdPrefabLeaf = Instantiate(leaf, tempPosition, newRotation);
            createdPrefabLeaf.transform.localScale = capsellaLeafScale * Random.Range(0.5f, 1f);

            createdPrefabLeaf.AddComponent<MeshRenderer>();
            createdPrefabLeaf.transform.SetParent(createdPrefabStem.transform);
        }

        return createdPrefabStem;
    }

    GameObject SelectCapsellaLeaf()
    {
        GameObject ret;
        int n = Random.Range(2, 10);
        switch (n)
        {
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
                ret = capsellaLeaf7;
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