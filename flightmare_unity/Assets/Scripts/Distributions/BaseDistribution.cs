using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDistribution
{
    public GameObject terrain;

    public abstract Vector2 getNextPosition();

    public List<Vector2> generated_positions = new List<Vector2>();

    protected BaseDistribution(int seed = 1) 
    {
        Random.seed = seed;
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
