using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{

    //public Transform[] target;
    //public GameObject tagPoints;
    public List<Vector3> listOfPose = new List<Vector3>();
    public float speedPosition = 5f;
    public float speedRotation = 90f;
    private int current;
    //public int currentTag;
    public bool endPath = false;


    public DrawRect drawScript;
    //public float xMin = 0f;
    //public float yMin = -15f;
    //public float xMax = 35f;
    //public float yMax = 15f;

    public Vector4 corners;// = new Vector4(0f, -15f, 35f, 15f);
    public float initialHeight = 20f;
    private Rect initialPath;
    //public List<Vector3> rectCorner = new List<Vector3>();
    //private Vector3 bl = new Vector3(2f, 10f, 1f);
    //private Vector3 br = new Vector3(15f, 10f, 1f); 
    //private Vector3 tr = new Vector3(6f, 10f, 5f);
    //private Vector3 tl = new Vector3(3f, 10f, 3f);
    public List<Vector3> wayPoints = new List<Vector3>();
    public List<Vector3> tagPoints = new List<Vector3>();
    public float tagHeight = 10f;
    //public List<Vector3> inspectedPoints = new List<Vector3>();
    //******************************CAMERA PARAMETERS
    private readonly Vector2 treshold = new Vector2(0f, 0f);
    public Vector2 cameraWidthHeight = new Vector2(1f, 1f);//(28f, 21f);


    private void Awake()
    {

        current = 0;
        initialPath = Rect.MinMaxRect(corners.x, corners.y, corners.z, corners.w);

        listOfPose.AddRange(drawScript.SetUpPath(initialPath, initialHeight, cameraWidthHeight + treshold));
        drawScript.DrawWaypoint(listOfPose);

        //drawScript.PrintList(listOfPose);

        tagPoints.Insert(0, new Vector3(0f, 20f, 10f));
        tagPoints.Insert(1, new Vector3(30f, 20f, 10f));
    }

    void Update()
    {
        GoTo();
        //if (tagPoints.Count != 0)
        //{
        //    AddPointOfInterest(tagPoints);
        //}

    }

    private void GoTo()
    {
        if (transform.position != listOfPose[current])
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, listOfPose[current], speedPosition * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }

        else if (current < (listOfPose.Count - 1))
        {
            current += 1;
        }
        else if (current == (listOfPose.Count - 1))
        {
            endPath = true;
        }


    }

    public void AddPointOfInterest(List<Vector3> pointsOfInterest)
    {
        if (endPath)
        {
            Rect rect = drawScript.GetRect(pointsOfInterest[pointsOfInterest.Count - 1], initialPath, cameraWidthHeight+treshold);   //AREA AROUND POINT

            //drawScript.DrawRectangle(rect);
            //rect =  drawScript.GetRect(rectCorner);               //AREA AROUND A BOX

            ////print(rect.xMin + "\t" + rect.yMin + "\t" + rect.xMax + "\t" + rect.yMax);
            wayPoints = drawScript.GenerateWaypoints(rect, tagHeight, cameraWidthHeight);
            ////drawScript.PrintList(listOfPose);

            drawScript.DrawWaypoint(wayPoints);

            listOfPose.AddRange(wayPoints);

            endPath = false;
            pointsOfInterest.RemoveAt(pointsOfInterest.Count - 1);
        }

    }
}