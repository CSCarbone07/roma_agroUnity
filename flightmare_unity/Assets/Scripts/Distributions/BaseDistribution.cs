using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDistribution
{
    public GameObject terrain;

    public abstract Vector2 getNextPosition();

    public List<Vector2> generated_positions = new List<Vector2>();

    protected BaseDistribution(int? seed = null) 
    {
        if(seed is int seed_) {
            UnityEngine.Random.seed = seed_;
            Debug.Log("Using seed " + seed_);
        } else {
            UnityEngine.Random.seed = System.Environment.TickCount;   
        }

    }

    public Vector4 getFieldMinMax()
    {
        // TODO: get these values from gameobject
        float min_x = 0;
        float max_x = 100;
        float min_y = 0;
        float max_y = 100;
        return new Vector4(min_x, max_x, min_y, max_y);
    }
}
