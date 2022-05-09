using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundTruthCropSwitch : MonoBehaviour
{
    private bool regularOn = true;
    public GameObject[] Regular;
    public GameObject[] GroundTruth;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject ob in GroundTruth)
        {
            ob.SetActive(false);
        }
        StartCoroutine(setGT(2));

    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator setGT(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (GameObject ob in Regular)
        {
            if (regularOn == true)
            { ob.SetActive(false); }
            else
            { ob.SetActive(true); }
        }
        foreach (GameObject ob in GroundTruth)
        {
            if (regularOn == true)
            { ob.SetActive(true); }
            else
            { ob.SetActive(false); }
        }
        if (regularOn == true)
        { regularOn = false; }
        else
        { regularOn = true; }

        StartCoroutine(setGT(2));

    }

    IEnumerator setRegular(float time)
    {
        yield return new WaitForSeconds(time);


    }

}
