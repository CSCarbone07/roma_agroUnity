using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
public class readerSpawner : MonoBehaviour
{
    private string m_Path;
    public string r_Path = "Assets/Resources/Texts/Fields/";

    public bool spawnOnStart = false;

    public string sourceFile = "fields";
    public string sourceFormat = ".yaml";

    public float scale = 1;
    public float maxDensity = 12;
    public bool useSeed = true;
    public bool readSeed = true;
    public int seed = 0;
    public float altitudeOffset = 0;
    public Vector3 worldOffset = new Vector3(0,0,0);
    public int borderExclusion = 0;   // This is to exclude spawning in the outer layer of the world. Used for missions were agent sees multiple cells
    public bool includeNonUtility = false;   // This is to include boxes where there is no utility markers 
    public float nonUtilityDensity = 100;   // This is to include boxes where there is no utility markers 
    public int nonUtilityMaxCount = 9;   // This is to include boxes where there is no utility markers 
    private List<Vector3> cells_coordinates = new List<Vector3>();
    private List<float> cells_density = new List<float>();
    private Vector3 world_maxCoordinates = new Vector3(0, 0, 0);

    public GameObject spawner;
    private List<GameObject> spawnedInstances = new List<GameObject>();
    private List<GameObject> spawnedUtilities = new List<GameObject>();

    // Variables to specify the amount of boxes (for box experiment) spawned in every cell (low is determined by
    // the targets there need to be present in the cell)
    public bool forceHighAmount = true;
    private int ForcedAmountLow = -1;
    public int ForcedAmountHigh = -1;

    // Variable to specify the amount targets, this is setup later based on the generated utility
    // for the field
    private int sub_ForcedAmountLow = -1;
    private int sub_ForcedAmountHigh = -1;


    public bool updateInstantiation = false;    // Click to update instances





    public void ReadString(string inString)
    {
        sourceFile = inString;


        /*
        string path = "Assets/Resources/test.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
        */
    }
    /*
    public void setFieldNumber(string inString)
    {
        sourceFile = "Assets/Resources/Texts/Fields/field_" + inString + ".yaml";
    }
    */



    // TODO menuitem is causing an error on build
    //[MenuItem("Tools/Read file")]
    public void readFile(string inString)
    {
        string fileString;
        if (inString == " ")
        {
            fileString = sourceFile;
        }
        else
        {
            fileString = inString;
        }

        //Get the path of the Game data folder
        m_Path = Application.dataPath + "/";

        //Output the Game data path to the console
        Debug.Log("dataPath : " + m_Path);

        string path = sourceFile;

        if (!Application.isEditor)
        {
            path = m_Path + fileString + sourceFormat;
        }
        else
        {
            path = r_Path + fileString + sourceFormat;
        }



        Debug.Log("file read start: " + path);
        StreamReader reader = new StreamReader(path);
        string stream = reader.ReadToEnd();
        string[] lines = stream.Split('\n');
        Debug.Log(lines.Length + " lines found");

        cells_coordinates.Clear();
        cells_density.Clear();
	for (int i = 0; i < lines.Length; i++)
	{
	  cells_coordinates.Add(new Vector3(0,0,0));
	  cells_density.Add(0); 
	}

        int cell_id = 0;

        if((useSeed && readSeed) || !Application.isEditor)
        {
            Debug.Log("Seed read " + lines[0][0]);
            int inSeed;// = (int)lines[0][0];
            int.TryParse(lines[0][0].ToString(), out inSeed);
            Debug.Log("Seed found " + inSeed);
            seed = inSeed;
        }


        foreach (string line in lines)
        {
            string[] numbers = line.Split(' ');
            List<float> temp_numbers = new List<float>();
            int i = 0;
            foreach (string s in numbers)
            {
                //int inNumber = int.Parse(i);
                int inNumber;
                int.TryParse(s, out inNumber);
                //Debug.Log(inNumber);
                temp_numbers.Add((float)inNumber);
                i++;
            }
            if(temp_numbers.Count>1)
            {
                //Debug.Log(line);
                //Vector3 testVector = new Vector3(temp_numbers[2], temp_numbers[3], 0.0f);
                //Debug.Log(testVector.ToString());

                cell_id = (int)temp_numbers[1];
                //Debug.Log("id assigned " + cell_id.ToString());

		//cells_coordinates.Insert(cell_id, new Vector3(temp_numbers[2] * scale, altitudeOffset, temp_numbers[3] * scale));
		cells_coordinates[cell_id] = new Vector3(temp_numbers[2] * scale, altitudeOffset, temp_numbers[3] * scale);
	
		// Update max world coordinates
		if(world_maxCoordinates[0] < temp_numbers[2])
		{
		  world_maxCoordinates[0] = temp_numbers[2];
		}
		if(world_maxCoordinates[2] < temp_numbers[3])
		{
		  world_maxCoordinates[2] = temp_numbers[3];
		}

		//cells_density.Insert(cell_id, temp_numbers[4]);
		cells_density[cell_id] = temp_numbers[4];
                //Debug.Log(cells_density[cell_id].ToString());
                Debug.Log("reading cell " + cell_id + " with coordinates " + cells_coordinates[cell_id].x.ToString() + "x + " + cells_coordinates[cell_id].y.ToString() + "y + " + cells_coordinates[cell_id].z.ToString() + "z and density " + cells_density[cell_id]);
            }

        }

        //Debug.Log(reader.ReadToEnd());
        Debug.Log("file read finished");
        Debug.Log("file read max coordinates " + world_maxCoordinates[0] + "x + " + world_maxCoordinates[1] + "y + " + world_maxCoordinates[2] + "z");

        reader.Close();

        spawnObjects();
    }

    private void spawnObjects()
    {
   
        if (useSeed)
        { Random.seed = seed; }



        /*
        foreach (GameObject o in spawnedInstances)
        {
            Destroy(o);
            print("Destroying cells");
        }
        */
        /*
        foreach (Transform child in transform) //this.gameObject.transform)
        {
            //DestroyImmediate(child.gameObject);
            GameObject.DestroyImmediate(child.gameObject);
        }
        */

        spawnedInstances.Clear();
        spawnedUtilities.Clear();
        Quaternion zeroRot = Quaternion.Euler(0, 0, 0);

	float exclusion = borderExclusion * scale;
        //foreach (Vector3 o in cells_coordinates)
        //print(cells_density.Count.ToString());
        for (int i = 0; i < cells_density.Count; i++)
        {
	  // Skip spawning if outer layer is excluded 
	  print("Spawning cell id " + i + " at coordinates " + cells_coordinates[i][0] + "x + " + cells_coordinates[i][1] + "x + " + cells_coordinates[i][2] + "z with density " + cells_density[i]);
	  if(borderExclusion == 0 || (cells_coordinates[i][0] >= exclusion && cells_coordinates[i][2] >= exclusion 
		&& cells_coordinates[i][0] <= (world_maxCoordinates[0]*scale - exclusion) && cells_coordinates[i][2] <= (world_maxCoordinates[2]*scale - exclusion)))
	  {
	    // Spawn spawner in case boxes without targets will be included
	    GameObject spawnedObject = Instantiate(spawner, cells_coordinates[i] + worldOffset, zeroRot);
	    spawnedObject.transform.SetParent(this.gameObject.transform);
          
	    if (cells_density[i]>0)
            {               
		// spawn utilities with normal cell spawner 
                if(spawnedObject.GetComponent<CellSpawner>() != null)
		{
		  spawnedObject.GetComponent<CellSpawner>().cell_id = i;
		  spawnedObject.GetComponent<CellSpawner>().Density = cells_density[i]*100.0f/ maxDensity;
		  List<GameObject> newUtilities = spawnedObject.GetComponent<CellSpawner>().procedural_Instantiate(null);
		  
		  // Save spawned utilities in a variable
		  spawnedUtilities.AddRange(newUtilities);
		} 
            }
	    
	    // Spawn utilities using the prefab instantiation, this is used mostly for the box cases
	    // where there can be boxes but no utility target
	    if(spawnedObject.GetComponent<PrefabInstatiation>() != null && (cells_density[i] > 0 || (includeNonUtility && (nonUtilityDensity / 100) >= Random.Range(0.0f, 1.0f))))
	    {
	      sub_ForcedAmountLow = (int)cells_density[i];
	      sub_ForcedAmountHigh = (int)cells_density[i];
	      ForcedAmountLow = (int)cells_density[i];
	      if(!forceHighAmount)
	      {ForcedAmountHigh = (int)cells_density[i];}

	      if(includeNonUtility)
	      {
		ForcedAmountHigh = nonUtilityMaxCount;
	      }

	      print("Spawning with cell density " + (int)cells_density[i]);

	      spawnedObject.GetComponent<PrefabInstatiation>().setForcedAmounts(ForcedAmountLow, ForcedAmountHigh, sub_ForcedAmountLow, sub_ForcedAmountHigh);
	      List<GameObject> newUtilities = spawnedObject.GetComponent<PrefabInstatiation>().procedural_Instantiate(null);
	      
	      // Save spawned utilities in a variable
	      spawnedUtilities.AddRange(newUtilities);
	    }

	    spawnedInstances.Add(spawnedObject);
	    //print("spawning cell " + i.ToString() + " in position " + cells_coordinates[i]);
	  }

        //this.GetComponent<>().SwitchToTAG();
	}
    }

    public List<GameObject> getCellObjects()
    {
        return spawnedInstances;
    }

    public List<GameObject> getSpawnedObjects()
    {
        return spawnedUtilities;
    }




    // Start is called before the first frame update
    void Start()
    {
        if (useSeed)
        { Random.seed = seed; }

        if (spawnOnStart)
        { readFile(sourceFile); }
    }

    // Update is called once per frame
    void Update()
    {

        if (updateInstantiation)
        {
            List<GameObject> objectsToDestroy = new List<GameObject>();

            foreach (Transform child in transform) //this.gameObject.transform)
            {
                objectsToDestroy.Add(child.gameObject);
                //DestroyImmediate(child.gameObject);
            }
            foreach (GameObject o in objectsToDestroy)
            {
                GameObject.DestroyImmediate(o);
                print("destroying object");
                //GameObject.Destroy(child.gameObject);
            }



            /*
            foreach (GameObject o in spawnedUtilities)
            {
                Destroy(o);
                //print("Destroying cells");
            }
            foreach (GameObject o in spawnedInstances)
            {
                Destroy(o);
                //print("Destroying cells");
            }
            */


            if (useSeed)
            {
                Random.seed = seed;
            }

            readFile(sourceFile);

            updateInstantiation = false;
                      
        }

    }


}
