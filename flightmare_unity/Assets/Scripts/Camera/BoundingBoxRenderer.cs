using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxRenderer : MonoBehaviour
{

    //public GameObject target;
    //public GameObject[] all_targets;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void OnGUI()
    {
        //GameObject allGOs = FindObjectsOfType<GameObject>();

        GameObject[] allGOs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        Camera camera = GetComponent<Camera>();
        Vector3[] pts = new Vector3[8];
        Vector3[] world_pts = new Vector3[8];
        Vector3[] vertices = new Vector3[8];
        Vector3 world_min = new Vector3(0,0,0);
        Vector3 world_max = new Vector3(0, 0, 0);
        bool firstRound = true;
        Bounds b;
        // b = target.GetComponent<SkinnedMeshRenderer>().bounds;
        Rect r;

        foreach (GameObject a in allGOs)
        {
            //for (int i = 0; i < vertices.Length; i++)

            GameObject o;
            if (a.transform.Find("BoundingBox"))
            {
                print("Object found");
                o = a.transform.Find("BoundingBox").gameObject;


                if (o.GetComponent<MeshFilter>())
                //if (o.GetComponent<BoxCollider>())
                {
                    vertices = o.GetComponent<MeshFilter>().mesh.vertices;
                    //b = o.GetComponent<SkinnedMeshRenderer>().bounds;
                    //b = o.GetComponent<BoxCollider>().bounds;
                    print(o);


                    Quaternion objectRotation = o.transform.rotation;
                    //Vector3 rotatedExtent = objectRotation * b.extents;

                    //print("Object rotation");
                    //print(o.transform.eulerAngles);
                    //print(o.transform.rotation);
                    //print("Object extent");
                    //print(b.extents);
                    //print("Extent rotated");
                    //print(rotatedExtent);

                    // All 8 vertices of the bounds 
                    /*
                    world_pts[0] = new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z);
                    world_pts[1] = new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z);
                    world_pts[2] = new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z);
                    world_pts[3] = new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z);
                    world_pts[4] = new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z);
                    world_pts[5] = new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z);
                    world_pts[6] = new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z);
                    world_pts[7] = new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z);
                    */
                    /*
                    pts[0] = camera.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
                    pts[1] = camera.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
                    pts[2] = camera.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
                    pts[3] = camera.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
                    pts[4] = camera.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
                    pts[5] = camera.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
                    pts[6] = camera.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
                    pts[7] = camera.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
                    */
                    /*
                    pts[0] = camera.WorldToScreenPoint(new Vector3(b.center.x + rotatedExtent.x, b.center.y + rotatedExtent.y, b.center.z + rotatedExtent.z));
                    pts[1] = camera.WorldToScreenPoint(new Vector3(b.center.x + rotatedExtent.x, b.center.y + rotatedExtent.y, b.center.z - rotatedExtent.z));
                    pts[2] = camera.WorldToScreenPoint(new Vector3(b.center.x + rotatedExtent.x, b.center.y - rotatedExtent.y, b.center.z + rotatedExtent.z));
                    pts[3] = camera.WorldToScreenPoint(new Vector3(b.center.x + rotatedExtent.x, b.center.y - rotatedExtent.y, b.center.z - rotatedExtent.z));
                    pts[4] = camera.WorldToScreenPoint(new Vector3(b.center.x - rotatedExtent.x, b.center.y + rotatedExtent.y, b.center.z + rotatedExtent.z));
                    pts[5] = camera.WorldToScreenPoint(new Vector3(b.center.x - rotatedExtent.x, b.center.y + rotatedExtent.y, b.center.z - rotatedExtent.z));
                    pts[6] = camera.WorldToScreenPoint(new Vector3(b.center.x - rotatedExtent.x, b.center.y - rotatedExtent.y, b.center.z + rotatedExtent.z));
                    pts[7] = camera.WorldToScreenPoint(new Vector3(b.center.x - rotatedExtent.x, b.center.y - rotatedExtent.y, b.center.z - rotatedExtent.z));
                    */


                    for (int i = 0; i < vertices.Length; i++)
                    {
                        // World space 
                        vertices[i] = o.transform.TransformPoint(vertices[i]);
                        // GUI space 
                        vertices[i] = camera.WorldToScreenPoint(vertices[i]);
                        vertices[i].y = Screen.height - vertices[i].y;
                    }

                    // Get them in GUI space 
                    for (int i = 0; i < pts.Length; i++) pts[i].y = Screen.height - pts[i].y;


                    //print("points found");
                    // Calculate the min and max positions 
                    //Vector3 min = pts[0];
                    //Vector3 max = pts[0];
                    Vector3 min = vertices[0];
                    Vector3 max = vertices[0];
                    if(firstRound)
                    {
                        firstRound = false;
                        world_min = vertices[0];
                        world_max = vertices[0];

                    }
                    for (int i = 1; i < pts.Length; i++)
                    {
                        print(world_pts[i]);
                        print(pts[i]);
                        //min = Vector3.Min(min, pts[i]);
                        //max = Vector3.Max(max, pts[i]);
                        min = Vector3.Min(min, vertices[i]);
                        max = Vector3.Max(max, vertices[i]);

                        world_min = Vector3.Min(world_min, min);
                        world_max = Vector3.Max(world_max, max);
                    }

                    // Construct a rect of the min and max positions 
                    r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
                    GUI.Box(r, "");
                }
            }
        }

        // Construct a rect of the min and max positions 
        r = Rect.MinMaxRect(world_min.x, world_min.y, world_max.x, world_max.y);
        GUI.Box(r, "");
    }
}
