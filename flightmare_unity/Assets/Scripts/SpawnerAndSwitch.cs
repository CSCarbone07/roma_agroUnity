using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAndSwitch : MonoBehaviour
{


    // Start is called before the first frame update
    public virtual void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Spawn()
    {

        SetAllChildrenStatic(this.transform);

    }

    void SetAllChildrenStatic(Transform ob)
    {
        foreach (Transform child in ob)//.GetComponentsInChildren<Transform>())
        {
            child.gameObject.isStatic = true;
            SetAllChildrenStatic(child);
        }
    }

    public virtual void SwitchToRGB()
    {

    }


    public virtual void SwitchToNIR()
    {
        /*
        foreach (GameObject leaf in createdPrefabLeaf)
        {


        }
        */       
    }

    public virtual void SwitchToTAG()
    {
        /*
        foreach (GameObject leaf in createdPrefabLeaf)
        {


        }
        */
    }
}
