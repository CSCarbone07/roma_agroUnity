﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class InitialPath : MonoBehaviour
{
    public float lineWidth = .2f;
    public float heightWave = 20f;

    public LineRenderer lineRenderer;

    public List<Vector3> points = new List<Vector3>();
    public Vector4 corners; //= new Vector4(0f, -15f, 35f, 15f);
    public float initialHeight = 20f;
    public Rect initialPath;
    public float cameraWidth = 1f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupWave();
    }

    private void SetupWave()
    {
        initialPath = Rect.MinMaxRect(corners.x, corners.y, corners.z, corners.w);


        //for (int i = 0; initialPath.xMin + ((i + 1) * cameraWidth) <= initialPath.xMax; i += 2) //i<rect.xMax;
        //{
        //    points.Add(new Vector3(initialPath.xMin + ((i + 1) * cameraWidth), initialHeight, initialPath.yMin + cameraWidth));
        //    points.Add(new Vector3(initialPath.xMin + ((i + 1) * cameraWidth), initialHeight, initialPath.yMax - cameraWidth));
        //}


        //for (int j = 2; (j < points.Count & j + 1 < points.Count); j += 4)
        //{
        //    Vector3 pt1 = points[j];
        //    Vector3 pt2 = points[j + 1];
        //    points[j] = pt2;
        //    points[j + 1] = pt1;
        //}

        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = 4;
        lineRenderer.SetPosition(0, new Vector3(initialPath.xMin, cameraWidth, initialPath.yMin));
        lineRenderer.SetPosition(1, new Vector3(initialPath.xMax, cameraWidth, initialPath.yMin));
        lineRenderer.SetPosition(2, new Vector3(initialPath.xMax, cameraWidth, initialPath.yMax));
        lineRenderer.SetPosition(3, new Vector3(initialPath.xMin, cameraWidth, initialPath.yMax));
        //for (int j = 0; j < points.Count; j++)
        //{
        //    lineRenderer.SetPosition(j, points[j]);
        //}

        lineRenderer.loop = true;
    }

}