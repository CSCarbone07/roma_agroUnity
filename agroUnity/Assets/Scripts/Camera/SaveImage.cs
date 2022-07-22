using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveImage : MonoBehaviour
{

    // camera resolution
    public string savePath = "/dataset/";
    public string firstShotName = "test";
    public int width = 1024;
    public int height = 1024;
    public bool takeScreenshotOnStart = false;
    public bool saveBoundingBoxes = false;

    public List<GameObject> plantSpawners = null;

    // globally average value according to wikipedia
    // https://en.wikipedia.org/wiki/Earth_radius
    private float earthRadiusKm = 6371;

    private int counter = 0;
    private List<GameObject> allPlants = null;

   
    // termini coordinates 41.90121857710724, 12.499919190501346
    //[Tooltip("Latitude, Longitude, Altitude")]
    //public Vector3 zero_coordinate = new Vector3(41.90121857710724, 12.499919190501346 , 0);
    public double zero_latitude = 41.90121857710724;
    public double zero_longitude = 12.499919190501346;


    // Start is called before the first frame update
    void Start()
    {   
        if(takeScreenshotOnStart)
        {
            TakeScreenshot(firstShotName, 1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public float degreesToRadians(float degrees) {
      return degrees * Mathf.PI / 180;
    }

    public float distanceInKmBetweenEarthCoordinates(float lat1, float lon1, float lat2, float lon2) {

      float dLat = degreesToRadians(lat2-lat1);
      float dLon = degreesToRadians(lon2-lon1);

      lat1 = degreesToRadians(lat1);
      lat2 = degreesToRadians(lat2);

      var a = Mathf.Sin(dLat/2) * Mathf.Sin(dLat/2) +
	      Mathf.Sin(dLon/2) * Mathf.Sin(dLon/2) * Mathf.Cos(lat1) * Mathf.Cos(lat2); 
      var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1-a)); 
      return earthRadiusKm * c;
    }

    public double convertToGNSS_longitude_difference(float x) // in meters
    {
      return x * (1/(earthRadiusKm * 1000)) * (180 / Mathf.PI);
    }
    public double convertToGNSS_latitude_difference(float z) // in meters
    {
      return z * (1/(earthRadiusKm * 1000)) * (180 / Mathf.PI);
    }
    
    public void TakeScreenshot(string sub, int c)
    {
        counter = c;
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
        
	//string filename = string.Format("{0}" + savePath + "images/" + sub + "{1}.png", Application.persistentDataPath, counter);
	string filename = string.Format("{0}" + savePath + sub + "{1}.png", Application.persistentDataPath, counter);
	print("savePath");
	print(filename);
        System.IO.File.WriteAllBytes(filename, bytes);


        //filename = string.Format("{0}/Dataset/LocationAndRotations.png", Application.persistentDataPath);
        //filename = string.Format("{0}" + savePath + "transforms/" + sub + "{1}.txt", Application.persistentDataPath, counter);
        filename = string.Format("{0}" + savePath + "transforms/" + "{1}.txt", Application.persistentDataPath, counter);

        string[] content = new string[2];
        content[0] = "Label,Latitude,Longitude,Altitude,Roll,Pitch,Yaw";
        //content[0] = "Position: " + this.transform.position.ToString();
        //content[1] = "Rotation: " + this.transform.rotation.ToString();
        //content[1] = "Position " + this.transform.position.x.ToString() + " " + this.transform.position.y.ToString() + " " + this.transform.position.z.ToString();
        //content[2] = "Rotation " + this.transform.eulerAngles.x.ToString() + " " + this.transform.eulerAngles.y.ToString() + " " + this.transform.eulerAngles.z.ToString();
        content[1] = counter.ToString() + "," + (zero_latitude + convertToGNSS_latitude_difference(this.transform.position.z)).ToString() + "," + (zero_longitude + convertToGNSS_longitude_difference(this.transform.position.x)).ToString() + "," + this.transform.position.y.ToString() + "," + this.transform.eulerAngles.z.ToString() + "," + this.transform.eulerAngles.x.ToString() + "," + this.transform.eulerAngles.y.ToString();
        File.WriteAllLines(filename, content);

	if(saveBoundingBoxes)
	{
	  getPlants();
	  if(allPlants != null)
	  {	
	    string boxFileName;
	    boxFileName = string.Format("{0}" + savePath + "boxes/" + "{1}.txt"
	    , Application.persistentDataPath, counter);
	    
	    if (GetComponent<BoundingBox_Plants>())
	    {
	      GetComponent<BoundingBox_Plants>().setPlantSpawner(allPlants);
	      GetComponent<BoundingBox_Plants>().saveBoxes(allPlants, filename, boxFileName, 
	    -1, -1, -1, -1); 
	    } 
	  }
	}
        //counter++;
    }

    public void setPlantSpawners(List<GameObject> inPlantSpawners=null)
    {
      if (inPlantSpawners != null)
      {
	print("SaveImage setting plantSpawners");
	plantSpawners = inPlantSpawners;
      }
    }
    

    public void getPlants()
    { 
      if (plantSpawners != null)
      {
	print("SaveImage | getting plants from " + plantSpawners.Count + " plantSpawners");
        
	allPlants = new List<GameObject>();
        for (var i = 0; i < plantSpawners.Count; i++)
	{
	  List<GameObject> newPlants = new List<GameObject>();
	  newPlants = plantSpawners[i].GetComponent<PrefabInstatiation>().get_createdPrefabs();
	  if (newPlants != null)
	  {
	    print("SaveImage | adding " + newPlants.Count + " plants from plantSpawner with id " + i );
	    allPlants.AddRange(newPlants);
	  }
	}
      }
      else
      {
	print("SaveImage trying to get plants but plant spawners is empty");
      }
    }

    public void setPlants(List<GameObject> inPlants=null)
    {
      if (inPlants != null)
      {
	print("SaveImage setting allPlants");
	allPlants = inPlants;
      }

    }
}
