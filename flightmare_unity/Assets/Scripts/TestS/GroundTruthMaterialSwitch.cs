using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundTruthSwitch : MonoBehaviour
{

    //private int[] originalMaterial = new int[5];
    private int matInd = 0;
    private List<Material> originalMaterials = new List<Material>();
    private List<Material> gtMaterials = new List<Material>();
    public Material groundTruthMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //print("lala");

        if (GetComponent<Renderer>())
        {
            for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
            {
                originalMaterials.Add(GetComponent<Renderer>().materials[i]);
            }
        }
        getMaterialsInChildrens(gameObject);

        //print(originalMaterials.Count);
        //print("lala");


        StartCoroutine(setGTMaterials(2));

    }

    // Update is called once per frame
    void Update()
    {
        //print("time");
        //matInd = 0;
        //setMaterialsInChildrens(gameObject);
    }

    IEnumerator setGTMaterials(float time)
    {
        yield return new WaitForSeconds(time);
        print("start change to ground truth");
        // Code to execute after the delay
        matInd = 0;
        if (GetComponent<Renderer>())
        {
            for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
            {
                //get
            }
            for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
            {
                //GetComponent<Renderer>().enabled = true;
                GetComponent<Renderer>().sharedMaterials[i]= groundTruthMaterial;
                print("changing material");
            }
        }
        setGTMaterialsInChildrens(gameObject);
        print("finish change to ground truth");

    }

    IEnumerator setOriginalMaterials(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        matInd = 0;
        if (GetComponent<Renderer>())
        {
            for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
            {
                GetComponent<Renderer>().materials[i]=originalMaterials[matInd];
            }
        }
        setOriginalMaterialsInChildrens(gameObject);
    }



    void getMaterialsInChildrens(GameObject child)
    {

        int numOfChildren = child.transform.childCount;
        //print(numOfChildren);
        if (child.GetComponent<Renderer>())
        {
            for (int i = 0; i < child.GetComponent<Renderer>().materials.Length; i++)
            {
                originalMaterials.Add(child.GetComponent<Renderer>().materials[i]);
            }
            //print("ecco");
            //print(child.GetComponent<Renderer>().materials.Length);
            //print(transform.GetChild(i).transform.childCount);
            //print(newChild.transform.childCount);
        }
        //print();
        for (int i = 0; i < numOfChildren; i++)
        {
            //print(child.GetComponent<Renderer>());
            GameObject newChild = child.transform.GetChild(i).gameObject;
            getMaterialsInChildrens(newChild);

                //print("ecco");
                                       
        }
    }

    void setGTMaterialsInChildrens(GameObject child)
    {
        print("starting recursion");

        int numOfChildren = child.transform.childCount;
        //print(numOfChildren);
        if (child.GetComponent<Renderer>())
        {
            for (int i = 0; i < child.GetComponent<Renderer>().materials.Length; i++)
            {
                child.GetComponent<Renderer>().sharedMaterials[i] = groundTruthMaterial;
                print("changing material");

                //originalMaterials.Add(child.GetComponent<Renderer>().materials[i]);
            }
            //print("ecco");
            //print(child.GetComponent<Renderer>().materials.Length);
            //print(transform.GetChild(i).transform.childCount);
            //print(newChild.transform.childCount);
        }
        //print();
        for (int i = 0; i < numOfChildren; i++)
        {
            //print(child.GetComponent<Renderer>());
            GameObject newChild = child.transform.GetChild(i).gameObject;
            setGTMaterialsInChildrens(newChild);

            //print("ecco");

        }
    }

    void setOriginalMaterialsInChildrens(GameObject child)
    {
        
        int numOfChildren = child.transform.childCount;
        //print(numOfChildren);
        if (child.GetComponent<Renderer>())
        {
            for (int i = 0; i < child.GetComponent<Renderer>().materials.Length; i++)
            {
                child.GetComponent<Renderer>().materials[i]= originalMaterials[matInd];
                matInd = matInd + 1;
                //originalMaterials.Add(child.GetComponent<Renderer>().materials[i]);
            }
            //print("ecco");
            //print(child.GetComponent<Renderer>().materials.Length);
            //print(transform.GetChild(i).transform.childCount);
            //print(newChild.transform.childCount);
        }
        //print();
        for (int i = 0; i < numOfChildren; i++)
        {
            //print(child.GetComponent<Renderer>());
            GameObject newChild = child.transform.GetChild(i).gameObject;
            setOriginalMaterialsInChildrens(newChild);

            //print("ecco");

        }
    }


}
