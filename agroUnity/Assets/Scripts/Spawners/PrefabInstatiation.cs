using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteAlways]
[ExecuteInEditMode]
public class PrefabInstatiation : MonoBehaviour
{
    private bool DEBUG_ALL = false;

    [Tooltip("Select the object to instantiate")]
    public GameObject prefab;
    //public int numberOfObjects = 20;
    //public float radius = 5f;
    public bool useSeed = false;
    public int seed = 0;

    public bool assignScalatingIds = false;

    [Tooltip("Randomness in position of overall field of instances")]
    public Vector3 overallPositionRandomness = new Vector3(0, 0, 0);
    [Tooltip("Randomness in rotation of overall field of instances")]
    public Vector3 overallRotationRandomness = new Vector3(0, 0, 0);
    
    private Vector3 newOverallPositionRandomness = new Vector3(0, 0, 0);
    private Vector3 newOverallRotationRandomness = new Vector3(0, 0, 0);

    private Vector3 myPosition;// = transform.position;
    protected List<GameObject> createdPrefabs = new List<GameObject>();
    [Tooltip("How many repetitions of the instance must be created in the X axis")]
    public float gridX = 5f;
    [Tooltip("How many repetitions of the instance must be created in the Y axis")]
    public float gridY = 5f;
    [Tooltip("Space between each row of instances in the X direction")]
    public float spacingX = 2f;
    [Tooltip("Space between each row of instances in the Y direction")]
    public float spacingY = 2f;
    [Tooltip("Rotation applied individually to all instances (in case your mesh default rotation is not the desired one for the field)")]
    public Vector3 Rotation; //for rotating the mesh in case the mesh is upside down or something else
    [Tooltip("Scale applied individually to all instances (in case your mesh default scale is not the desired one for the field)")]
    public Vector3 Scale = new Vector3(1,1,1);
    private Quaternion newRotation;

    [Tooltip("Position offset applied individually to all instances (in case your mesh default rotation is not the desired one for the field)")]
    public Vector3 positionOffset = new Vector3(0, 0, 0);
    [Tooltip("Position randomness applied to each individual instance. For example, if 1 is added in X then the noise in the X axis for the spawned instance will be between -1 and 1")]
    public Vector3 positionRandomness = new Vector3(0, 0, 0);

    private Vector3 addRandomRotation;

    [Tooltip("Rotation randomness applied to each individual instance. For example, if 1 is added in X then the noise in the X axis for the spawned instance will be between -180 and 180")]
    [Range(0.0f, 1.0f)]
    public float addRandomRotationX = 0;
    [Tooltip("Rotation randomness applied to each individual instance. For example, if 1 is added in Y then the noise in the Y axis for the spawned instance will be between -180 and 180")]
    [Range(0.0f, 1.0f)]
    public float addRandomRotationY = 0;
    [Tooltip("Rotation randomness applied to each individual instance. For example, if 1 is added in Z then the noise in the Z axis for the spawned instance will be between -180 and 180")]
    [Range(0.0f, 1.0f)]
    public float addRandomRotationZ = 0;


    private Vector3 randomRotationValue;


    [Tooltip("Scale randomness applied to each individual instance. For example, if 1 is added in X then the noise in the X axis for the spawned instance will be between -1 and 1")]
    public float scaleRandomness = 0;// = new Vector3(0, 0, 0);
    /*
    [Range(0.0f, 1.0f)]
    public float scaleRandomnessX = 0;
    [Range(0.0f, 1.0f)]
    public float scaleRandomnessY = 0;
    [Range(0.0f, 1.0f)]
    public float scaleRandomnessZ = 0;
    [Range(0.0f, 100.0f)]
    */


    [Tooltip("Density of instances. If 100, all the instances based on the previous values will be spawned, it 50 then only half of the instances will actually spawned by random selection")]
    public float Density = 100;
    public int ForcedAmountLow = -1;
    public int ForcedAmountHigh = -1;
    private int current_forcedAmount = -1;

    public int sub_ForcedAmountLow = -1;
    public int sub_ForcedAmountHigh = -1;
    private int sub_current_forcedAmount = -1;

    [Tooltip("Click the box to respwan prefabs")]
    public bool updateInstantiation = false;    // Click to update instances
    [Tooltip("Click the box to switch prefabs to RGB channel if available")]
    public bool switchToRGB = false;
    [Tooltip("Click the box to switch prefabs to NIR channel if available")]
    public bool switchToNIR = false;
    [Tooltip("Click the box to switch prefabs to TAG channel if available")]
    public bool switchToTAG = false;
    //private bool canUpdate = true;
    private bool regenerate = true;


    public virtual List<GameObject> procedural_Instantiate(GameObject inGameObject)
    {
        //gameObject.isStatic = true;
	
	int appearanceId = 0;

        List<GameObject> childrenToDestroy = new List<GameObject>();
        foreach (Transform child in transform) //this.gameObject.transform)
        {
            childrenToDestroy.Add(child.gameObject);
            //DestroyImmediate(child.gameObject);
            //GameObject.DestroyImmediate(child.gameObject);
        }
        foreach (GameObject o in childrenToDestroy) //this.gameObject.transform)
        {
            GameObject.DestroyImmediate(o);
        }

        current_forcedAmount = ForcedAmountLow;

        if(ForcedAmountLow >= 0 && ForcedAmountHigh > ForcedAmountLow)
        {current_forcedAmount = Random.Range(ForcedAmountLow, ForcedAmountHigh+1);}

        sub_current_forcedAmount = sub_ForcedAmountLow;

        if(sub_ForcedAmountLow >= 0 && sub_ForcedAmountHigh > sub_ForcedAmountLow)
        {sub_current_forcedAmount = Random.Range(sub_ForcedAmountLow, sub_ForcedAmountHigh+1);}


        newOverallPositionRandomness = new Vector3(Random.Range(-overallPositionRandomness.x, overallPositionRandomness.x),
        Random.Range(-overallPositionRandomness.y, overallPositionRandomness.y), Random.Range(-overallPositionRandomness.z, overallPositionRandomness.z));

        newOverallRotationRandomness = new Vector3(Random.Range(-overallRotationRandomness.x, overallRotationRandomness.x),
        Random.Range(-overallRotationRandomness.y, overallRotationRandomness.y), Random.Range(-overallRotationRandomness.z, overallRotationRandomness.z));


	if(DEBUG_ALL) 
	{
	  print("Instatiating prefabs. current forced amount " + current_forcedAmount + " sub_current_forcedAmount " + sub_current_forcedAmount);
	  print("Overall rotation " + newOverallRotationRandomness);
	  print(inGameObject);
	}
        if (inGameObject != null)
        {
            prefab = inGameObject;
        }
        if (prefab != null)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }

            myPosition = transform.position;
            newRotation = Quaternion.Euler(Rotation);
            
	    //print("Debugging1");
            if(createdPrefabs != null)
            {
                createdPrefabs.Clear();
            }
            //print("Debugging2");

            if (regenerate && current_forcedAmount != 0)
            {

                for (int y = 0; y < gridY; y++)
                {
                    for (int x = 0; x < gridX; x++)
                    {
                        //randomRotationValue = new Vector3(Random.Range(-90.0f, 90.0f)* addRandomRotationX, Random.Range(-90.0f, 90.0f) * addRandomRotationY, Random.Range(-90.0f, 90.0f) * addRandomRotationZ));
                        /*
                        addRandomRotation = new Vector3(addRandomRotationX, addRandomRotationY, addRandomRotationZ);
                        randomRotationValue = addRandomRotation * (Random.Range(-180.0f, 180.0f));
                        newRotation = Quaternion.Euler(Rotation + randomRotationValue);
                        */
                        if (current_forcedAmount>0 || (Density / 100) >= Random.Range(0.0f, 1.0f))
                        {
                            Vector3 newPositionRandomness = new Vector3(Random.Range(-positionRandomness.x, positionRandomness.x),
                            Random.Range(-positionRandomness.y, positionRandomness.y), Random.Range(-positionRandomness.z, positionRandomness.z));

                            Vector3 pos = new Vector3(x * spacingX, 0, y * spacingY) + myPosition + newPositionRandomness + positionOffset + newOverallPositionRandomness;
                            GameObject createdPrefab = Instantiate(prefab, pos, Quaternion.Euler(Vector3.zero));
                            //print("Debugging3");
                            createdPrefabs.Add(createdPrefab);
                            //print("Debugging4");

                            createdPrefab.transform.SetParent(this.gameObject.transform); // = this.transform;
                                                                                          //prefab.transform.parent = transform;



                            //Vector3 newRandomness = new Vector3(Random.Range(0.0f, scaleRandomness.X), Random.Range(0.0f, scaleRandomness.Y), Random.Range(0.0f, scaleRandomness.Z));
                            //Vector3 newScaleRandomness = new Vector3(Random.Range(-scaleRandomnessX, scaleRandomnessX), Random.Range(-scaleRandomnessY, scaleRandomnessY), Random.Range(-scaleRandomnessZ, scaleRandomnessZ));


                            addRandomRotation = new Vector3(Random.Range(-addRandomRotationX,addRandomRotationX), Random.Range(-addRandomRotationY,addRandomRotationY), Random.Range(-addRandomRotationZ,addRandomRotationZ));
                            randomRotationValue = addRandomRotation * (180.0f);//(Random.Range(-180.0f, 180.0f));
                            //randomRotationValue = addRandomRotation;
                            //print(randomRotationValue);
                            newRotation = Quaternion.Euler(Rotation + newOverallRotationRandomness + randomRotationValue);

                            createdPrefab.transform.rotation = newRotation;


                            if (createdPrefab.GetComponent<SpawnerAndSwitch>())
                            {
				if(assignScalatingIds)
				{
				  appearanceId ++;
				  createdPrefab.GetComponent<SpawnerAndSwitch>().appearanceId = appearanceId;
				}

                                createdPrefab.GetComponent<SpawnerAndSwitch>().setPlantScale(Random.Range(-scaleRandomness, scaleRandomness));
                                createdPrefab.GetComponent<SpawnerAndSwitch>().Spawn();

                            }
                            createdPrefab.transform.localScale = Scale;// + newScaleRandomness;
                            


                        }
                    }
                }
            }
        }
        /*
        foreach (Transform child in transform)
        {
            float newFloatScaleRandomness = Random.Range(-scaleRandomness, scaleRandomness);
            Vector3 newScaleRandomness = new Vector3(newFloatScaleRandomness, newFloatScaleRandomness, newFloatScaleRandomness);

            //Vector3 newScaleRandomness = new Vector3(newFloatScaleRandomness, newFloatScaleRandomness, newFloatScaleRandomness);
            //print(child);
            child.transform.localScale = Scale + newScaleRandomness;
        }
        */

	if(DEBUG_ALL) 
	{
	print("main prefabs instantiated");
	}

        if(current_forcedAmount > 0)
        {
	    if(DEBUG_ALL) 
	    {
	    print("removing extra prefabs " + createdPrefabs.Count);
	    }
            while(current_forcedAmount < createdPrefabs.Count)
            {
                //createdPrefabs.RemoveAt(Random.Range(0,createdPrefabs.Count));
                int indexToDestroy = Random.Range(0,createdPrefabs.Count);
                GameObject objectToDestroy = createdPrefabs[indexToDestroy]; 
                createdPrefabs.RemoveAt(indexToDestroy);
                DestroyImmediate(objectToDestroy);

	      if(DEBUG_ALL) 
	      {
                print("removing object " + createdPrefabs.Count);
	      }
            }
        }

        if(sub_current_forcedAmount >= 0 && current_forcedAmount > 0)
        {
	    
	    if(DEBUG_ALL) 
	    {
	      print("removing extra sub prefabs");
	    }

            List<GameObject> objectsToUnspawn = new List<GameObject>();
            //List<GameObject> objectsToUnspawn = createdPrefabs;
	    //
            foreach (GameObject o in createdPrefabs)
            {
                objectsToUnspawn.Add(o);
            }
	    
	    if(DEBUG_ALL) 
	    {
	      print("created extra sub prefabs " + objectsToUnspawn.Count + " sub_current_forcedAmount " + sub_current_forcedAmount);
	    }

            while(sub_current_forcedAmount < objectsToUnspawn.Count)
            {
                int indexToUnspwan = Random.Range(0,objectsToUnspawn.Count);
                GameObject objectToUnspawn = objectsToUnspawn[indexToUnspwan]; 
                objectsToUnspawn.RemoveAt(indexToUnspwan);
                objectToUnspawn.GetComponent<SpawnerAndSwitch>().Unspawn();
	
		if(DEBUG_ALL) 
		{
		  print("unspawning extra sub prefab " + objectsToUnspawn.Count);
		  print("unspawning extra sub prefab " + createdPrefabs.Count);
		}
            }

        }

	if(DEBUG_ALL) 
	{
	  print("sub prefabs instantiated");
	  if(current_forcedAmount > 0)
	  {print("created prefabs " + createdPrefabs.Count);}
	  print("returning prefabs");
	}


        return createdPrefabs;
    }

    public List<GameObject> get_createdPrefabs()
    {
	if(createdPrefabs.Count == 0 && transform.childCount > 0)
	{
	  if(DEBUG_ALL) 
	  {
	  print("PrefabInstatiation | getting " + transform.childCount + " childs for created prefabs");
	  }
	  createdPrefabs = new List<GameObject>();
	  for (int i = 0; i< this.transform.childCount; i++)
	  {
	      createdPrefabs.Add(this.transform.GetChild(i).gameObject);
	  }

	}
	if(DEBUG_ALL) 
	{
	  print("PrefabInstatiation | returning " + createdPrefabs.Count + " created prefabs ");
	  print("returning prefabs");
	}
        return createdPrefabs;
    }

    void spwanInstance()
    {
        
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (useSeed)
        { Random.seed = seed; }
    }

    // Update is called once per frame
    protected virtual void Update()
    //void OnRenderObject()
    {

        if (!Application.IsPlaying(this) && updateInstantiation)
        {
            if (useSeed)
            { Random.seed = seed; }

            procedural_Instantiate(prefab);
            updateInstantiation = false;
        }
        
	if (!Application.IsPlaying(this) && switchToRGB)
        {
            switchToRGB = false;
	    if(createdPrefabs != null)
	    {
	      foreach (GameObject item in createdPrefabs) 
	      {
		if(item != null && item.GetComponent<SpawnerAndSwitch>() != null)
		{
		  item.GetComponent<SpawnerAndSwitch>().SwitchToRGB();
		}
	      }
	    }
        }

	if (!Application.IsPlaying(this) && switchToNIR)
        {
            switchToNIR = false;
	    if(createdPrefabs != null)
	    {
	      foreach (GameObject item in createdPrefabs) 
	      {
		if(item != null && item.GetComponent<SpawnerAndSwitch>() != null)
		{
		  item.GetComponent<SpawnerAndSwitch>().SwitchToNIR();
		}
	      }
	    }
        }

	if (!Application.IsPlaying(this) && switchToTAG)
        {
            switchToTAG = false;
	    if(createdPrefabs != null)
	    {
	      foreach (GameObject item in createdPrefabs) 
	      {
		if(item != null && item.GetComponent<SpawnerAndSwitch>() != null)
		{
		  item.GetComponent<SpawnerAndSwitch>().SwitchToTAG();
		}
	      }
	    }
        }


        /*
        if (updateInstantiation)
        {
            if (canUpdate)
            {
                canUpdate = false;
                foreach (Transform child in this.gameObject.transform)
                {
                    DestroyImmediate(child.gameObject);
                }
                Instantiate();

                updateInstantiation = false;
                canUpdate = true;
            }
        }
        */


        /*
        for (int i = 0; i < numberOfObjects; i++)
        {

            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 pos = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius) + myPosition;
            Instantiate(prefab, pos, Quaternion.identity);
         }
        */


        //print(this.gameObject.name);
    }

    public void setForcedAmounts(int inLow, int inHigh, int inLow_sub, int inHigh_sub)
    {
	if(DEBUG_ALL) 
	{
	print("setting forced amounts. inLow " + inLow + " inHigh " + inHigh + " inLow_sub " + inLow_sub + " inHigh_sub " + inHigh_sub);
	}
        ForcedAmountLow = inLow;
        ForcedAmountHigh = inHigh;

        sub_ForcedAmountLow = inLow_sub;
        sub_ForcedAmountHigh = inHigh_sub;
    }

    public Vector3 getCurrentOverallRotation()
    {
        return newOverallRotationRandomness;
    }

    public int getCurrent_current_forcedAmount()
    {
        return current_forcedAmount;
    }

    public int getCurrent_sub_current_forcedAmount()
    {
        return sub_current_forcedAmount;
    }


}




