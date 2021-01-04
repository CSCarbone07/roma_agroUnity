using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTest : MonoBehaviour
{

    public ParentTest()
    {
        Debug.Log("1st Fruit Constructor Called");
    }

    // Start is called before the first frame update
    public virtual void Chop()
    {
        Debug.Log("The fruit has been chopped.");
    }

    // Update is called once per frame
    public virtual void SayHello()
    {
        Debug.Log("Hello, I am a fruit.");
    }



}
