using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SquareWave : MonoBehaviour
{
    public int vertexCount = 1000;
    public float lineWidth = .2f;
    public float A = 10f; //Ampiezza
    public float offset = 0f;
    public float T = 10f; //Mathf.PI * 2;
    public float heightWave = 20f; //Altezza dall'origine
    public float numberRepetition = 5f;
    
    private LineRenderer lineRenderer;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupWave();
    }

    private void SetupWave()
    {
        lineRenderer.widthMultiplier = lineWidth;
        float deltaTheta = numberRepetition * (2f * Mathf.PI) / vertexCount;
        float theta = 0f;
        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            //Vector3 pos = new Vector3(theta, heightWave, (A * Mathf.Sign(Mathf.Sin(theta))));
            Vector3 pos = new Vector3(theta, heightWave, (A * Mathf.Sign(Mathf.Sin(2f * Mathf.PI * ((theta - offset) / T)))));
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

}
