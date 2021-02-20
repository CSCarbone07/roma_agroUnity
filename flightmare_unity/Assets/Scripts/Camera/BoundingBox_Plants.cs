using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class BoundingBox_Plants : MonoBehaviour
{
    public bool displayBoxes = true;
    private GameObject plantSpawner;
    private List<GameObject> plants;



    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPlantSpawner(List<GameObject> inPlants)
    {
        plants = inPlants;
        print("plants reference set");
    }

    void OnGUI()
    {
        if (displayBoxes && plants.Count > 1 && plants[0] != null)
        {
            //GameObject[] allGOs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            //GameObject[] allGOs = plantSpawner;

            Camera camera = GetComponent<Camera>();
            Vector3[] vertices = new Vector3[8];
            Vector3 total_min = new Vector3(0, 0, 0);
            Vector3 total_max = new Vector3(0, 0, 0);
            bool firstRound = true;
            Rect r;

            foreach (GameObject p in plants)
            {
                firstRound = true;


                for (int c = 0; c < p.transform.GetChildCount(); c++)
                {
                    Transform a = p.transform.GetChild(c);
                    Transform o;
                    if (a.Find("BoundingBox"))
                    {
                        o = a.transform.Find("BoundingBox");

                        if (o.GetComponent<MeshFilter>())
                        {
                            vertices = o.GetComponent<MeshFilter>().mesh.vertices;

                            for (int i = 0; i < vertices.Length; i++)
                            {
                                // total space 
                                vertices[i] = o.transform.TransformPoint(vertices[i]);
                                // GUI space 
                                vertices[i] = camera.WorldToScreenPoint(vertices[i]);
                                vertices[i].y = Screen.height - vertices[i].y;
                            }

                            // Calculate the min and max positions 
                            if (firstRound)
                            {
                                firstRound = false;
                                total_min = vertices[0];
                                total_max = vertices[0];
                            }
                            for (int i = 1; i < vertices.Length; i++)
                            {
                                total_min = Vector3.Min(total_min, vertices[i]);
                                total_max = Vector3.Max(total_max, vertices[i]);
                            }
                        }
                    }
                }

                print(Screen.width + "x" + Screen.height);
                print("min:" + total_min);
                print("max:" + total_max);
                // Construct a rect of the min and max positions 
                if ((total_max.x >= 0 && total_max.y >= 0 && total_max.x <= Screen.width && total_max.y <= Screen.height) 
                || (total_min.x >= 0 && total_min.y >= 0 && total_min.x <= Screen.width && total_min.y <= Screen.height)
                || (total_min.x >= 0 && total_max.y >= 0 && total_min.x <= Screen.width && total_max.y <= Screen.height)
                || (total_max.x >= 0 && total_min.y >= 0 && total_max.x <= Screen.width && total_min.y <= Screen.height))
                {

                    if (total_min.x < 0)
                    { total_min.x = 0; }
                    if (total_min.y < 0)
                    { total_min.y = 0; }

                    if (total_min.x > Screen.width)
                    { total_min.x = Screen.width; }
                    if (total_min.y > Screen.height)
                    { total_min.y = Screen.height; }

                    if (total_max.x < 0)
                    { total_max.x = 0; }
                    if (total_max.y < 0)
                    { total_max.y = 0; }

                    if (total_max.x > Screen.width)
                    { total_max.x = Screen.width; }
                    if (total_max.y > Screen.height)
                    { total_max.y = Screen.height; }

                    print("final min:" + total_min);
                    print("final max:" + total_max);


                    r = Rect.MinMaxRect(total_min.x, total_min.y, total_max.x, total_max.y);
                    GUI.Box(r, "");
                }
            }
        }
    }

    public void saveBoxes(List<GameObject> inObjects,string inImgPath, string inSavePath, string inClass)
    {
        string boxText = " ";

        Camera camera = GetComponent<Camera>();
        Vector3[] vertices = new Vector3[8];
        Vector3 total_min = new Vector3(0, 0, 0);
        Vector3 total_max = new Vector3(0, 0, 0);
        bool firstRound = true;
        Rect r;

        foreach (GameObject p in inObjects)
        {
            firstRound = true;


            for (int c = 0; c < p.transform.GetChildCount(); c++)
            {
                Transform a = p.transform.GetChild(c);
                Transform o;
                if (a.Find("BoundingBox"))
                {
                    o = a.transform.Find("BoundingBox");

                    if (o.GetComponent<MeshFilter>())
                    {
                        vertices = o.GetComponent<MeshFilter>().mesh.vertices;

                        for (int i = 0; i < vertices.Length; i++)
                        {
                            // total space 
                            vertices[i] = o.transform.TransformPoint(vertices[i]);
                            // GUI space 
                            vertices[i] = camera.WorldToScreenPoint(vertices[i]);
                            vertices[i].y = Screen.height - vertices[i].y;
                        }

                        // Calculate the min and max positions 
                        if (firstRound)
                        {
                            firstRound = false;
                            total_min = vertices[0];
                            total_max = vertices[0];
                        }
                        for (int i = 1; i < vertices.Length; i++)
                        {
                            total_min = Vector3.Min(total_min, vertices[i]);
                            total_max = Vector3.Max(total_max, vertices[i]);
                        }
                    }
                }
            }

            // Construct a rect of the min and max positions 
            if ((total_max.x >= 0 && total_max.y >= 0 && total_max.x <= Screen.width && total_max.y <= Screen.height)
            || (total_min.x >= 0 && total_min.y >= 0 && total_min.x <= Screen.width && total_min.y <= Screen.height)
            || (total_min.x >= 0 && total_max.y >= 0 && total_min.x <= Screen.width && total_max.y <= Screen.height)
            || (total_max.x >= 0 && total_min.y >= 0 && total_max.x <= Screen.width && total_min.y <= Screen.height))
            {

                if (total_min.x < 0)
                { total_min.x = 0; }
                if (total_min.y < 0)
                { total_min.y = 0; }

                if (total_min.x > Screen.width)
                { total_min.x = Screen.width; }
                if (total_min.y > Screen.height)
                { total_min.y = Screen.height; }

                if (total_max.x < 0)
                { total_max.x = 0; }
                if (total_max.y < 0)
                { total_max.y = 0; }

                if (total_max.x > Screen.width)
                { total_max.x = Screen.width; }
                if (total_max.y > Screen.height)
                { total_max.y = Screen.height; }

                int outMinX = Mathf.RoundToInt(total_min.x);
                int outMinY = Mathf.RoundToInt(total_min.y);
                int outMaxX = Mathf.RoundToInt(total_max.x);
                int outMaxY = Mathf.RoundToInt(total_max.y);



                //filepath,x1,y1,x2,y2,class_name
                string newContent = inImgPath + "," + outMinX + "," + outMinY + "," + outMaxX + "," + outMaxY + "," + inClass + "\n";

                if(boxText == " ")
                {
                    boxText = newContent;
                }
                else
                {
                    boxText = boxText + newContent;
                }

                //boxTexts.Add()

                //r = Rect.MinMaxRect(total_min.x, total_min.y, total_max.x, total_max.y);
                //GUI.Box(r, "");
            }
        }
        File.WriteAllText(inSavePath, boxText);


    }

}
