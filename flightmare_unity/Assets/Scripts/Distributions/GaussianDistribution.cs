using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianDistribution : BaseDistribution
{
    private float gaussian_position_mean, gaussian_position_std;

    public GaussianDistribution(int seed = 1) : base(seed)
    {
        gaussian_position_mean = 0f;
        gaussian_position_std = 0f;
    }

    public GaussianDistribution(float gaussian_position_mean, float gaussian_position_std, int seed = 1) : base(seed)
    {
        this.gaussian_position_mean = gaussian_position_mean;
        this.gaussian_position_std = gaussian_position_std;
    }

    public override Vector2 getNextPosition() 
    {
        // Get the extreme values for the field
        Vector4 field_dimensions = getFieldMinMax();

        // Get x and y coordinates
        float x = NextGaussian(gaussian_position_mean, gaussian_position_std, field_dimensions[0], field_dimensions[1]);
        float y = NextGaussian(gaussian_position_mean, gaussian_position_std, field_dimensions[2], field_dimensions[3]);
        Vector2 gaussian_position = new Vector2(x, y);

        // Add to the list
        generated_positions.Add(gaussian_position);

        return gaussian_position;
    }

    // Gausian generation from
    // https://alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/
    public static float NextGaussian() {
        float v1, v2, s;
        do {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }

    public static float NextGaussian(float mean, float std)
    {
        return mean + NextGaussian() * std;
    }

    public static float NextGaussian(float mean, float std, float min, float max)
    {
        float x;
        do {
            x = NextGaussian(mean, std);
        } while (x < min || x > max);
        return x;
    }
}
