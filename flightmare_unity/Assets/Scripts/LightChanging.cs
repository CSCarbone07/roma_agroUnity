using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanging : MonoBehaviour
{
    // Interpolate light color between two colors back and forth
    float duration = 1.0f;
    Color color0 = Color.red;
    Color color1 = Color.blue;
    public float minIntensity = 0.2f;
    public float maxIntensity = 2.5f;

    public Vector3 minRotation = new Vector3(10,0,0);
    public Vector3 maxRotation = new Vector3(90,0,0);


    private float intensity;
    private Vector3 rotation;
    private Quaternion qRotation;

    Light lt;

    void Start()
    {
        lt = GetComponent<Light>();
        ChangeLightParameters();
        //print(maxRotation);

    }

    public void ChangeLightParameters()
    {
        print("light change");

        float alpha = Random.Range(0.0f, 1.0f);
        intensity = Mathf.Lerp(minIntensity, maxIntensity, alpha);
        rotation = Vector3.Lerp(minRotation, maxRotation, alpha);
        qRotation = Quaternion.Euler(rotation);

        if(lt)
        {
            lt.intensity = intensity;
        }


        //this.GetComponent<Transform>().rotation = qRotation;
        this.transform.rotation = qRotation;
        //this.transform.SetPositionAndRotation(this.transform.position, qRotation);

    }

    void Update()
    {
        // set light color
        //float t = Mathf.PingPong(Time.time, duration) / duration;
        //lt.color = Color.Lerp(color0, color1, t);

    }
}
