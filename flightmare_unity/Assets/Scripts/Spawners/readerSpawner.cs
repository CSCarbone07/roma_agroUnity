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

    public string sourceFile = "Assets/Resources/Texts/Fields/fields.yaml";

    public float scale = 1;
    public float maxDensity = 12;
    public bool useSeed = true;
    public bool readSeed = true;
    public int seed = 0;
    private List<Vector3> cells_coordinates = new List<Vector3>();
    private List<float> cells_density = new List<float>();

    public GameObject spawner;
    private List<GameObject> spawnedInstances = new List<GameObject>();


    public bool updateInstantiation = false;    // Click to update instances


    // Start is called before the first frame update
    void Start()
    {
        if(useSeed)
        {Random.seed = seed;}

        readFile();
    }

    // Update is called once per frame
    void Update()
    {

        if (updateInstantiation)
        {
            if(useSeed)
            {
                Random.seed = seed;
            }

            readFile();
            updateInstantiation = false;
        }

    }


    static void ReadString()
    {
        string path = "Assets/Resources/test.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    private void readFile()
    {

        foreach (Transform child in transform) //this.gameObject.transform)
        {
            //DestroyImmediate(child.gameObject);
            GameObject.DestroyImmediate(child.gameObject);
        }

        //Get the path of the Game data folder
        m_Path = Application.dataPath;

        //Output the Game data path to the console
        Debug.Log("dataPath : " + m_Path);

        string path = sourceFile;
        if (!Application.isEditor)
        {
            path = m_Path + "/field.yaml";
        }



        Debug.Log("file read start");
        StreamReader reader = new StreamReader(path);
        string stream = reader.ReadToEnd();
        string[] lines = stream.Split('\n');
        Debug.Log(lines.Length + " lines found");

        cells_coordinates.Clear();
        cells_density.Clear();
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
                Vector3 testVector = new Vector3(temp_numbers[2], temp_numbers[3], 0.0f);
                //Debug.Log(testVector.ToString());

                cell_id = (int)temp_numbers[1];
                //Debug.Log("id assigned " + cell_id.ToString());

                cells_coordinates.Insert(cell_id, new Vector3(temp_numbers[2] * scale + scale/2, 0.0f, temp_numbers[3] * scale + scale / 2));
                //Debug.Log("coordinates assigned");

                cells_density.Insert(cell_id, temp_numbers[4]);
                //Debug.Log(cells_density[cell_id].ToString());
            }

        }

        //Debug.Log(reader.ReadToEnd());
        Debug.Log("file read finished");
        reader.Close();

        spawnObjects();
    }

    private void spawnObjects()
    {
        foreach (Transform child in transform) //this.gameObject.transform)
        {
            //DestroyImmediate(child.gameObject);
            GameObject.DestroyImmediate(child.gameObject);
        }


        spawnedInstances.Clear();
        Quaternion zeroRot = Quaternion.Euler(0, 0, 0);

        //foreach (Vector3 o in cells_coordinates)
        print(cells_density.Count.ToString());
        for (int i = 0; i < cells_density.Count; i++)
        {
            if (cells_density[i]>0)
            {
                GameObject spawnedObject = Instantiate(spawner, cells_coordinates[i], zeroRot);
                spawnedInstances.Add(spawnedObject);
                spawnedObject.transform.SetParent(this.gameObject.transform);
                spawnedObject.GetComponent<CellSpawner>().cell_id = i;
                spawnedObject.GetComponent<CellSpawner>().Density = cells_density[i]*100.0f/ maxDensity;
                spawnedObject.GetComponent<CellSpawner>().procedural_Instantiate(null);
                print("spawning cell " + i.ToString() + " in position " + cells_coordinates[i]);
            }
        }

        //this.GetComponent<>().SwitchToTAG();

    }

}
