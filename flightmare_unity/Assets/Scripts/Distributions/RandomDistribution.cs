using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDistribution : BaseDistribution
{
    public RandomDistribution(int? seed = null): base(seed){}

    public override Vector2 getNextPosition() 
    {   
        // Get the extreme values for the field
        Vector4 field_dimensions = getFieldMinMax();

        Vector2 random_position = new Vector2(
            UnityEngine.Random.Range(field_dimensions[0], field_dimensions[1]), 
            UnityEngine.Random.Range(field_dimensions[2], field_dimensions[3])
        );
        
        // Add to the list
        generated_positions.Add(random_position);

        return random_position;
    }
}
