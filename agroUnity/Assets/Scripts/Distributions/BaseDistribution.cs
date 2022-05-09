using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDistribution
{
    public GameObject terrain;

    public abstract Vector2 getNextPosition();

    public List<Vector2> generated_positions = new List<Vector2>();

    float min_x = 0;
    float max_x = 62.5f;
    float min_y = 0;
    float max_y = 40;

    protected BaseDistribution(int? seed = null) 
    {
        if(seed is int seed_) {
            UnityEngine.Random.seed = seed_;
            Debug.Log("Using seed " + seed_);
        } else {
            UnityEngine.Random.seed = System.Environment.TickCount;   
        }

    }

    public void setFieldMinMax(float in_min_x, float in_max_x, float in_min_y, float in_max_y)
    {
        min_x = in_min_x;
        max_x = in_max_x;
        min_y = in_min_y;
        max_y = in_max_y;
    }

    public Vector4 getFieldMinMax()
    {
        return new Vector4(min_x, max_x, min_y, max_y);
    }
}
