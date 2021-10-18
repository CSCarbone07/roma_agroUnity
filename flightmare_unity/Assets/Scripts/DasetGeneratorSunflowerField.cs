using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;

public class DasetGeneratorSunflowerField : MonoBehaviour
{
    //[SerializeField] GameObject beetLeaf;
    [SerializeField] GameObject Sunflower;
    [SerializeField] Vector3 sunflowerScale = new Vector3(1, 1, 1);
    [SerializeField] Vector3 randomSunflowerScaleVariation = new Vector3(0, 0, 0);
    [SerializeField] Vector3 sunflowerRotation = new Vector3(0, 0, 0);
    [SerializeField] Vector3 randomSunflowerRotationVariation = new Vector3(0, 0, 0);
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
    [SerializeField] Material red;
    [SerializeField] Material black;
    [SerializeField] Material green;

    [SerializeField] GameObject terrain;

    // to select the kind of annotation to be generated
    public bool TakeScreenshots = false;
    public bool SaveBoxes = false;

    private Quaternion newRotation;
    private Vector3 randomRotationValue;

    private static int BeetNumber = 35;

    public int cropRows = 5;
    public int cropColumns = 5;
    public float spacingRow = 1;
    public float spacingColumn = 1;
    public Vector3 cropStartingPosition = new Vector3(0, 0, 0);

    public float minAltitude = 7;
    public float maxAltitude = 20;

    //private static int CapsellaNumber = 8;
    //private static int GalliumNumber = 8;
    private static int WeedInit = 10;
    int WeedNumber = WeedInit;

    string[] boxes; //= new string[BeetNumber + WeedInit];

    // object instances 
    private GameObject[] newBeet = new GameObject[BeetNumber];

    private GameObject[] newSunflower;// = new GameObject[cropRows * cropColumns];

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
    Vector3 defaultPos = new Vector3(0f, 0.5f, 0f); //default terrain dimensions
    Vector3 beetLeafRotation = new Vector3(200f, 0f, -90f);
    Vector3 galliumLeafRotation = new Vector3(-90, 0f, 0f);
    Vector3 capsellaLeafRotation = new Vector3(-90, 0f, 0f);

    Quaternion zeroRot = Quaternion.Euler(0, 0, 0);
    Quaternion rotation;

    // control where to save pictures
    public enum type { Image, Mask, Box };
    public enum cls { Crop, Weed };

    private bool CanStart = false;

    // Start is called before the first frame update
    void Start()
    {
        newSunflower = new GameObject[cropRows * cropColumns];
        boxes = new string[(BeetNumber + WeedInit + (cropRows * cropColumns))];
        CanStart = true;
        //print("ready");

    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldSpawn() && CanStart)
        {
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
                print(counter);
            }
            //print(Application.persistentDataPath);
            SpawnTerrain();
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
        Destroy(newTerrain);
        for (int i = 0; i < BeetNumber; i++)
        {
            if (newBeet[i] != null)
            {
                Destroy(newBeet[i]);
            }
        }
        for (int i = 0; i < (cropRows * cropColumns); i++)
        {
            if (newSunflower[i] != null)
            {
                Destroy(newSunflower[i]);
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


        GameObject myLight = GameObject.Find("Directional Light");
        if(myLight)
        {
            myLight.GetComponent<LightChanging>().ChangeLightParameters();
        }

        transform.position = new Vector3(0, UnityEngine.Random.Range(minAltitude, maxAltitude), 0);

        int counter = 0;
        float x_offset = 1.5f;
        float z_offset = 1f;

        Vector3 pos = RandomPosition();
        Vector3 start_pos = pos;
        /*
        for (int j = 0; j < cropRows; j++)
        {
            for (int k = 0; k < BeetNumber / cropRows; k++)
            {
                if (UnityEngine.Random.Range(0f, 10f) >= missBeet)
                {
                    newBeet[counter] = null;
                }
                else
                {
                    newBeet[counter] = SpawnBeet(pos + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0.1f, UnityEngine.Random.Range(-0.2f, 0.2f)));
                }
                counter++;
                pos[2] = pos[2] + z_offset;
            }

            start_pos[0] = start_pos[0] + x_offset;
            pos = start_pos;
        }
        */

        float rowPos = 0;
        float columnPos = 0;

        for (int i = 0; i < cropRows; i++)
        {
            for (int j = 0; j < cropColumns; j++)
            {
                newSunflower[counter] = spawnSunflower(cropStartingPosition + new Vector3((spacingRow*i), 0, (spacingColumn*j)));
                //rowPos = rowPos + spacingRow;
                counter++;
            }
            //columnPos = columnPos + spacingColumn;
        }


        for (int i = 0; i < WeedNumber; i++)
        {
            if (UnityEngine.Random.Range(0f, 10f) >= weedType)
            {
                newWeed[i] = SpawnGallium();
            }
            else
            {
                newWeed[i] = SpawnCapsella();
            }
        }

        if (TakeScreenshots)
        {
            TakeShot(type.Image);
        }

        nextSpawnTime = Time.time + spawnDelay;
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

    private void saveMasks()
    {
        if (TakeScreenshots)
        {
            GameObject[] objects = new GameObject[BeetNumber + WeedNumber];
            newBeet.CopyTo(objects, 0);
            newWeed.CopyTo(objects, newBeet.Length);

            changeMaterial(black, newTerrain);

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
                        changeMaterial(red, objects[i]);
                    }
                }
            }
            for (int i = 0; i < (cropRows*cropColumns); i++)
            {
                if (newSunflower[i] != null)
                {
                    changeMaterial(green, newSunflower[i]);
                }
            }

            TakeShot(type.Mask);
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
        int x, y, w, h;
        x = (int)Mathf.Clamp(min.x, 0f, width - 1f);
        y = (int)Mathf.Clamp(min.y, 0f, height - 1f);
        w = (int)Mathf.Clamp(max.x, 0f, width - 1f);
        h = (int)Mathf.Clamp(max.y, 0f, height - 1f);

        string species = null;
        if (cls == cls.Crop)
        {
            species = "1";
            //print(species + " " + x.ToString() + " " + y.ToString() + " " + w.ToString() + " " + h.ToString());
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
        string filename = ScreenshotName(type.Box);
        File.WriteAllLines(filename, content);
    }

    private void GenerateBoundingBoxes()
    {
        if (SaveBoxes)
        {
            //print("holaaaa-1");

            GameObject[] objects = new GameObject[BeetNumber + WeedNumber];
            newBeet.CopyTo(objects, 0);
            newWeed.CopyTo(objects, newBeet.Length);

            //print("holaaaa0");

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

            print(BeetNumber + WeedNumber);

            for (int i = BeetNumber + WeedNumber; i < (BeetNumber + WeedNumber + (cropRows * cropColumns)); i++)
            {
                if (newSunflower[i-(BeetNumber + WeedNumber)] != null)
                {
                    boxes[i] = GUIRectWithObject(newSunflower[i - (BeetNumber + WeedNumber)], cls.Crop);
                    //print(i - (BeetNumber + WeedNumber));
                    //print(boxes[0]);
                   //print("holaaaa1");
                }
                //print("holaaaa2");
            }

            SaveBoundingBox(boxes);
        }
    }

    /////////////////////////////////////////////////////////////
    //////////////// Plants generators //////////////////////////
    /////////////////////////////////////////////////////////////

    GameObject spawnSunflower(Vector3 position)
    {
        Vector3 tempPosition = position;
        //Vector3 tempPosition = new Vector3(UnityEngine.Random.Range(-4.1f, 1.8f), 0.8f, UnityEngine.Random.Range(-4.1f, 1.8f));
        randomRotationValue = new Vector3(
        UnityEngine.Random.Range(-randomSunflowerRotationVariation.x, randomSunflowerRotationVariation.x),
        UnityEngine.Random.Range(-randomSunflowerRotationVariation.y, randomSunflowerRotationVariation.y),
        UnityEngine.Random.Range(-randomSunflowerRotationVariation.z, randomSunflowerRotationVariation.z));



        newRotation = Quaternion.Euler(sunflowerRotation + randomRotationValue);

        GameObject createdPrefabSunflower = Instantiate(Sunflower, tempPosition, newRotation);

        createdPrefabSunflower.transform.localScale = sunflowerScale + 
        new Vector3(
            UnityEngine.Random.Range(-randomSunflowerScaleVariation.x, randomSunflowerScaleVariation.x), 
            UnityEngine.Random.Range(-randomSunflowerScaleVariation.y, randomSunflowerScaleVariation.y), 
            UnityEngine.Random.Range(-randomSunflowerScaleVariation.z, randomSunflowerScaleVariation.z));
        return createdPrefabSunflower;

    }

    /*
GameObject SpawnBeet(Vector3 position)
{
    GameObject createdPrefabStem = new GameObject();

    // Leaf Spawn
    Vector3 tempPosition = position;
    tempPosition[1] += 0.5f;

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

    return createdPrefabStem;
}
*/

    GameObject SpawnGallium()
    {
        GameObject createdPrefabStem = new GameObject();

        // Leaf Spawn
        Vector3 tempPosition = new Vector3(UnityEngine.Random.Range(-4.1f, 1.8f), 0.8f, UnityEngine.Random.Range(-4.1f, 1.8f));
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
                randomRotationValue = new Vector3(0f, 0f, x * 72.0f);
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

    GameObject SpawnCapsella()
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

