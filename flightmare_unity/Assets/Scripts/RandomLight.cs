//using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;

public class RandomLight : MonoBehaviour
{
    Light lt;
    float initialIntensity = 1;
    Color color;
    Vector3 pos;
    public float lowest_intensity_multiplier = 0.8f;
    public float highest_intensity_multiplier = 1.2f;
    public Vector3 noise_Illumination_rotation_min = new Vector3(60.0f, -180.0f, 0.0f);
    public Vector3 noise_Illumination_rotation_max = new Vector3(120.0f, 180.0f, 0.0f);

    private float spawnDelay = 3f;
    private float nextSpawnTime = 0f;
    private float intensity;
    private float rotationX;
    private float rotationY;
    private float rotationZ;

    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<Light>();
        initialIntensity = lt.intensity;
        color = lt.color;
        pos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeLight_intensity()
    {
        intensity = Random.Range(lowest_intensity_multiplier, highest_intensity_multiplier);
        print("change light " + intensity);

        //lt.color = color * intensity;
        //lt.intensity = Mathf.PingPong(Time.time, intensity);
        lt.intensity = initialIntensity * intensity;
    }
    public void changeLight_orientation()
    { 
        rotationX = Random.Range(noise_Illumination_rotation_min.x, noise_Illumination_rotation_max.x);
        rotationY = Random.Range(noise_Illumination_rotation_min.y, noise_Illumination_rotation_max.y);
        rotationZ = Random.Range(noise_Illumination_rotation_min.z, noise_Illumination_rotation_max.z);

        lt.transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, rotationZ));

        nextSpawnTime = Time.time + spawnDelay;
    }

}
