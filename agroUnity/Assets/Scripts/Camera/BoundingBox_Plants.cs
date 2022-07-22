using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class BoundingBox_Plants : MonoBehaviour
{

    private bool DEBUG_ALL = false;

    public bool displayBoxes = true;
    private GameObject plantSpawner;
    private List<GameObject> plants;

    private int w_trimmed_0 = -1;
    private int h_trimmed_0 = -1;
    private int w_trimmed_1 = -1;
    private int h_trimmed_1 = -1;


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
        //plants = null;
        //plants.Clear();
        plants = inPlants;
	if(DEBUG_ALL) 
	{
	  print("plants reference set " + inPlants.Count);
	}
    }

    void OnGUI()
    {
	if(DEBUG_ALL) 
	{
	  print("trying to display boxes");
	}
        if (displayBoxes && plants != null && plants.Count > 0)// && plants[0] != null)
        {
	    
	    if(DEBUG_ALL) 
	    {
	      print("trying to display boxes for " + plants.Count + " plants");
	    }
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

		if(DEBUG_ALL) 
		{
		  print("getting each plant");
		  print(p);
		}

                if(p.GetComponent<SpawnerAndSwitch>().hasBeenSpawned)
                {

                    firstRound = true;
		
		    if(DEBUG_ALL) 
		    {
		      print("getting bounding boxes");
		    }


                    for (int c = 0; c < p.transform.childCount; c++)
                    {
                        bool boundingBoxFound = false;

                        Transform a = p.transform.GetChild(c);
                        Transform o;
                        if (a.Find("BoundingBox"))
                        {
                            boundingBoxFound = true;
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

                    /*
                    print(Screen.width + "x" + Screen.height);
                    print("min:" + total_min);
                    print("max:" + total_max);
                    */

                    int save_w_0; 
                    int save_h_0;
                    int save_w_1; 
                    int save_h_1;

                    //rescaling for new trimmed image
                    
                    if(w_trimmed_1 >= 0 && h_trimmed_1 >= 0 && w_trimmed_0 >= 0 && h_trimmed_0 >= 0)
                    {
                        /*
                        print("plant found at " + total_min.x + "x_0, " + total_min.y + "y_0, " 
                        + total_max.x + "x_1, " + total_max.y + "y_1. Within " 
                        + w_trimmed_0 + "w_0, " + h_trimmed_0 + "h_0, "
                        + w_trimmed_1 + "w_1, " + h_trimmed_1 + "h_1. With screen " 
                        + Screen.width + "w x " + Screen.height + "h");
                        */

                        save_w_0 = w_trimmed_0;
                        save_h_0 = (Screen.height-h_trimmed_1);
                        save_w_1 = w_trimmed_1;
                        save_h_1 = (Screen.height-h_trimmed_0);

                    }
                    else
                    {
                        save_w_0 = 0;
                        save_h_0 = 0;
                        save_w_1 = Screen.width;
                        save_h_1 = Screen.height;
                    }

		    if(DEBUG_ALL) 
		    {
		      print("save boundaries " + save_w_0 + "w_0, " + save_h_0 + "h_0, " + save_w_1 + "w_1, " + save_h_1 + "h_1. With screen " 
		      + Screen.width + "w x " + Screen.height + "h");
		    }
                    
                    // Construct a rect of the min and max positions 
                    if ((total_max.x >= save_w_0 && total_max.y >= save_h_0 && total_max.x <= save_w_1 && total_max.y <= save_h_1) 
                    || (total_min.x >= save_w_0 && total_min.y >= save_h_0 && total_min.x <= save_w_1 && total_min.y <= save_h_1)
                    || (total_min.x >= save_w_0 && total_max.y >= save_h_0 && total_min.x <= save_w_1 && total_max.y <= save_h_1)
                    || (total_max.x >= save_w_0 && total_min.y >= save_h_0 && total_max.x <= save_w_1 && total_min.y <= save_h_1))
                    {

                        if (total_min.x < save_w_0)
                        { total_min.x = save_w_0; }
                        if (total_min.y < save_h_0)
                        { total_min.y = save_h_0; }

                        if (total_min.x > save_w_1)
                        { total_min.x = save_w_1; }
                        if (total_min.y > save_h_1)
                        { total_min.y = save_h_1; }

                        if (total_max.x < save_w_0)
                        { total_max.x = save_w_0; }
                        if (total_max.y < save_h_0)
                        { total_max.y = save_h_0; }

                        if (total_max.x > save_w_1)
                        { total_max.x = save_w_1; }
                        if (total_max.y > save_h_1)
                        { total_max.y = save_h_1; }

                        /*
                        print("final min:" + total_min);
                        print("final max:" + total_max);
                        */

                        r = Rect.MinMaxRect(total_min.x, total_min.y, total_max.x, total_max.y);
                        GUI.Box(r, "");
                    }
                }
            }
        }
    }

    //public void saveBoxes(List<GameObject> inObjects,string inImgPath, string inSavePath, string inClass, int w_0, int h_0, int w_1, int h_1)
    public void saveBoxes(List<GameObject> inObjects,string inImgPath, string inSavePath, int w_0, int h_0, int w_1, int h_1)
    {

	if(DEBUG_ALL) 
	{
	  print("BoundingBox_Plants | saving boxes from " + inObjects.Count + " plants at path " + inSavePath);
	}
        string boxText = " ";

        Camera camera = GetComponent<Camera>();
        Vector3[] vertices = new Vector3[8];
        Vector3 total_min = new Vector3(0, 0, 0);
        Vector3 total_max = new Vector3(0, 0, 0);
        bool firstRound = true;
        Rect r;

        foreach (GameObject p in inObjects)
        {
            if(p.GetComponent<SpawnerAndSwitch>().hasBeenSpawned)
            {
                firstRound = true;

                string objectClass = "0";

                if(p.GetComponent<SpawnerAndSwitch>()!=null)
                {
                    objectClass = p.GetComponent<SpawnerAndSwitch>().objectClass;
                }

                for (int c = 0; c < p.transform.childCount; c++)
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

                int save_w_0;
                int save_h_0;
                int save_w_1; 
                int save_h_1;
                //rescaling for new trimmed image
                
                if(w_1 >= 0 || h_1 >= 0 || w_0 >= 0 || h_0 >= 0)
                {
                    /*
                    print("plant found at " + total_min.x + "x_0 + " + total_min.y + "y_0 + " 
                    + total_max.x + "x_1 + " + total_max.y + "y_1. Within " 
                    + w_0 + "w_0 + " + h_0 + "h_0 + "
                    + w_1 + "w_1 + " + h_1 + "h_1.");
                    */
                    
                    w_trimmed_0 = w_0;
                    h_trimmed_0 = h_0;
                    w_trimmed_1 = w_1;
                    h_trimmed_1 = h_1;


                    save_w_0 = w_trimmed_0;
                    save_h_0 = (Screen.height-h_trimmed_1);
                    save_w_1 = w_trimmed_1;
                    save_h_1 = (Screen.height-h_trimmed_0);
                    
                }
                else
                {
                    save_w_0 = 0;
                    save_h_0 = 0;
                    save_w_1 = Screen.width;
                    save_h_1 = Screen.height;
                }
                
                //save_w_1 = Screen.width;
                //save_h_1 = Screen.height;

                // Construct a rect of the min and max positions 
                if ((total_max.x >= save_w_0 && total_max.y >= save_h_0 && total_max.x <= save_w_1 && total_max.y <= save_h_1) 
                || (total_min.x >= save_w_0 && total_min.y >= save_h_0 && total_min.x <= save_w_1 && total_min.y <= save_h_1)
                || (total_min.x >= save_w_0 && total_max.y >= save_h_0 && total_min.x <= save_w_1 && total_max.y <= save_h_1)
                || (total_max.x >= save_w_0 && total_min.y >= save_h_0 && total_max.x <= save_w_1 && total_min.y <= save_h_1))
                {

                    if (total_min.x < save_w_0)
                    { total_min.x = save_w_0; }
                    if (total_min.y < save_h_0)
                    { total_min.y = save_h_0; }

                    if (total_min.x > save_w_1)
                    { total_min.x = save_w_1; }
                    if (total_min.y > save_h_1)
                    { total_min.y = save_h_1; }

                    if (total_max.x < save_w_0)
                    { total_max.x = save_w_0; }
                    if (total_max.y < save_h_0)
                    { total_max.y = save_h_0; }

                    if (total_max.x > save_w_1)
                    { total_max.x = save_w_1; }
                    if (total_max.y > save_h_1)
                    { total_max.y = save_h_1; }

                    int outMinX = Mathf.RoundToInt(total_min.x);
                    int outMinY = Mathf.RoundToInt(total_min.y);
                    int outMaxX = Mathf.RoundToInt(total_max.x);
                    int outMaxY = Mathf.RoundToInt(total_max.y);

                    if(w_1 >= 0 || h_1 >= 0 || w_0 >= 0 || h_0 >= 0)
                    {
                        outMinX -= save_w_0;
                        outMinY -= save_h_0;
                        outMaxX -= save_w_0;
                        outMaxY -= save_h_0;
                    }

                    float boxWidth = outMaxX - outMinX;
                    float boxHeight = outMaxY - outMinY;
                    float boxCenterX = outMinX + boxWidth/2;
                    float boxCenterY = outMinY + boxHeight/2;

                    //filepath,x1,y1,x2,y2,class_name
                    string newContent;

                    //newContent = inImgPath + "," + outMinX + "," + outMinY + "," + outMaxX + "," + outMaxY + "," + objectClass + "\n";
                    newContent = objectClass + " " + boxCenterX/(save_w_1-save_w_0) + " " 
                    + boxCenterY/(save_h_1-save_h_0) + " " + boxWidth/(save_w_1-save_w_0) + " " + boxHeight/(save_h_1-save_h_0) + "\n";
                    
                    if(boxText == " ")
                    {
                        boxText = newContent;
                    }
                    else
                    {
                        boxText = boxText + newContent;
                    }

                    /*
                    if(w_1 >= 0 || h_1 >= 0 || w_0 >= 0 || h_0 >= 0)
                    {
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

                            newContent = inImgPath + "," + (outMinX) + "," + outMinY + "," + outMaxX + "," + outMaxY + "," + inClass + "\n";
                            if(boxText == " ")
                            {
                                boxText = newContent;
                            }
                            else
                            {
                                boxText = boxText + newContent;
                            }
                        }
                    }
                    else
                    {
                        newContent = inImgPath + "," + outMinX + "," + outMinY + "," + outMaxX + "," + outMaxY + "," + inClass + "\n";
                        if(boxText == " ")
                        {
                            boxText = newContent;
                        }
                        else
                        {
                            boxText = boxText + newContent;
                        }
                    }

                    */


                    //boxTexts.Add()

                    //r = Rect.MinMaxRect(total_min.x, total_min.y, total_max.x, total_max.y);
                    //GUI.Box(r, "");
                }
            }
        }
        File.WriteAllText(inSavePath, boxText);


    }

}
