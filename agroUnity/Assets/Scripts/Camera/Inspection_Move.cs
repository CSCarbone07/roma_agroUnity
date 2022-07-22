using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspection_Move : MonoBehaviour
{

    //[Header("test header")]


    [Tooltip("Set the camera altitude based on the GSD (ground sample distance)")]
    public bool altitudeBasedOnGSD = false;
   
    [Tooltip("GSD in m / pixel")]
    public float groundSampleDistance = 0.01f;

    [Tooltip("Set the altitude of the camera, invalid if altitudeBasedOnGSD is active")]
    public float altitude = 5;
    private Vector3 initalPosition = new Vector3(0, 0, 0);
    private Vector3 offset = new Vector3(1f, 1f, 1f);
    [Tooltip("Set overlap in percentage")]
    public Vector3 overlap = new Vector3(0f, 0f, 0f);
    private Vector3 nonOverlap = new Vector3(100f, 100f, 100f);
    [Tooltip("Set world coordinates for camera movement limit")]
    public Vector3 limit = new Vector3(200f, 200f, 200f);
    private Vector3 currentAdvancement = new Vector3(0,0,0);
    private bool movingForward = true;
    private bool sliding = false;

    // camera resolution (extracted from "SaveImage" script
    private int width = 1024;
    private int height = 1024;
    private float fov_vertical  = 60;


    // Start is called before the first frame update
    void Start()
    {
        //print("sup");


        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        width = this.GetComponent<SaveImage>().width;
        height = this.GetComponent<SaveImage>().height;
	//fov_vertical = this.GetComponent<SaveImage>().fieldOfView;


	if (altitudeBasedOnGSD)
	{
	  float smallestCameraDimension;
	  float selected_fov; 
	  float selected_footprint; 
	  float aspectRatio;
	  float diagonal;
	
	  if (width < height)
	  {
	    smallestCameraDimension = width;
	  }
	  else
	  {
	    smallestCameraDimension = height;
	    selected_fov = fov_vertical;
	    selected_footprint = height * groundSampleDistance;
	  } 
	  
	  aspectRatio = width / height;
	  //diagonal = Mathf.Pow((Mathf.Pow(width*groundSampleDistance)+Mathf.Pow(height*groundSampleDistance)), 0.5f);
	  //altitude = diagonal / (2 * Mathf.Tan(selected_fov/2));

	  this.transform.position = new Vector3(this.transform.position.x, altitude, this.transform.position.z);
	}
	else
	{
	  this.transform.position = new Vector3(this.transform.position.x, altitude, this.transform.position.z);
	}
        initalPosition = this.transform.position;
        //print(this.transform.position.y);


        offset = offset * altitude * Mathf.Tan(30 * Mathf.Deg2Rad) * 2;
        Vector3 tempOverlap = (nonOverlap - overlap)/100.0f;
        offset = new Vector3(offset.x * tempOverlap.x, offset.y * tempOverlap.y, offset.z * tempOverlap.z);
        print(offset);
        //offset = new Vector3();

    }

    public void Rellocate()
    {
        Vector3 newOffset = new Vector3(0,0,0);
        if((movingForward && this.transform.position.x + offset.x < limit.x+5) || (!movingForward && this.transform.position.x - offset.x > initalPosition.x-5) || sliding)
        {
            if(movingForward)
            {
                newOffset.x = offset.x;
            }
            else
            {
                newOffset.x = -offset.x;
            }
            sliding = false;

        }
        else
        {
            if (this.transform.position.z + offset.z > limit.z)
            {
                Application.Quit();
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
            }
            else
            {
                sliding = true;
                newOffset.z = offset.z;
                movingForward = !movingForward;
            }
        }



        //this.transform.position = new Vector3(initalPosition.x, altitude, initalPosition.z);
        Vector3 newPosition;// = new Vector3(this.transform.position.x +offset.x, altitude, this.transform.position.z + offset.z);
        newPosition = new Vector3(this.transform.position.x + newOffset.x, altitude, this.transform.position.z + newOffset.z);

        //this.transform.position = new Vector3(this.transform.position.x+offset.x, altitude, this.transform.position.z+offset.z);
        this.transform.position = newPosition;

    }

}
