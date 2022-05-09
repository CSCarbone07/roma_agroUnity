using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawRect : MonoBehaviour
{
    public float lineWidth = .5f;
    private LineRenderer lineRenderer;
    public Material material;


    private List<Vector3> points;


    public Vector4 corners;// = new Vector4(0f, -15f, 35f, 15f);
    public float initialHeight = 20f;
    private Rect initialPath;

    private void Awake()
    {

         initialPath = Rect.MinMaxRect(corners.x, corners.y, corners.z, corners.w);
         DrawRectangle(initialPath, initialHeight);


    }

    public List<Vector3> SetUpPath(Rect initPath, float h, Vector2 d)
    {
        lineRenderer = GetComponent<LineRenderer>();

        points = GenerateWaypoints(initPath, h, d);
        return points;
    }

    public Rect GetRect(List<Vector3> list) //USE THIS FOR SELECTING AN AREA GIVEN BOUNDS
    {
        float xmin = Mathf.Min(list[0].x, list[1].x, list[2].x, list[3].x);
        float ymin = Mathf.Min(list[0].z, list[1].z, list[2].z, list[3].z);
        float xmax = Mathf.Max(list[0].x, list[1].x, list[2].x, list[3].x);
        float ymax = Mathf.Max(list[0].z, list[1].z, list[2].z, list[3].z);
        Rect rectangle = Rect.MinMaxRect(xmin, ymin, xmax, ymax);
        //print(xmin + "\t" + ymin + "\t" + xmax + "\t" + ymax);
        return rectangle;
    }


    public Rect GetRect(Vector3 center, Rect rect, Vector2 area)  //USE THIS FOR SELECTING AN AREA GIVEN A POINT
    {
        float xMin = Mathf.Clamp(center.x - area.x / 2f, rect.xMin, center.x - area.x / 2f);
        float yMin = Mathf.Clamp(center.z - area.y / 2f, rect.yMin, center.z - area.y / 2f);
        float xMax = Mathf.Clamp(center.x + area.x / 2f, center.x + area.x / 2f, rect.xMax);
        float yMax = Mathf.Clamp(center.z + area.y / 2f, center.z + area.y / 2f, rect.yMax);

        Rect rectangle = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
        //print(xMin + "\t" + yMin + "\t" + xMax + "\t" + yMax);

        return rectangle;
    }

    public void DrawRectangle(Rect rectangle, float h)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = 4;
        lineRenderer.SetPosition(0, new Vector3(rectangle.xMin, h, rectangle.yMin));
        lineRenderer.SetPosition(1, new Vector3(rectangle.xMax, h, rectangle.yMin));
        lineRenderer.SetPosition(2, new Vector3(rectangle.xMax, h, rectangle.yMax));
        lineRenderer.SetPosition(3, new Vector3(rectangle.xMin, h, rectangle.yMax));
        lineRenderer.material = material;
        lineRenderer.loop = true;
    }


    public List<Vector3> GenerateWaypoints(Rect rectangle, float h, Vector2 d)
    {
        print("rectangle.width:" + rectangle.width);
        print("rectangle.height:" + rectangle.height);
        print("CameraWidthHeight:" + d);
        float soglia = .05f * d.x;//Aggiunta una soglia per coprire tutto

        List<Vector3> wayPts = new List<Vector3>();

        if (d.x < rectangle.width & d.y < rectangle.height)
        {
            print("NORMAL!!!!!");
            for (int i = 0; rectangle.xMin + ((i + .5f) * d.x) <= rectangle.xMax + soglia; i += 1) //i<rect.xMax;        
            {
                if (rectangle.xMin + (i + .5f) * d.x <= rectangle.xMax)
                {
                    wayPts.Add(new Vector3(rectangle.xMin + ((i + .5f) * d.x), h, rectangle.yMin + .5f * d.y));
                    wayPts.Add(new Vector3(rectangle.xMin + ((i + .5f) * d.x), h, rectangle.yMax - .5f * d.y));
                }
                else
                {
                    wayPts.Add(new Vector3(rectangle.xMax, h, rectangle.yMin + .5f * d.y));
                    wayPts.Add(new Vector3(rectangle.xMax, h, rectangle.yMax - .5f * d.y));
                }
            }

            for (int j = 2; (j < wayPts.Count & j + 1 < wayPts.Count); j += 4)
            {
                Vector3 pt1 = wayPts[j];
                Vector3 pt2 = wayPts[j + 1];
                wayPts[j] = pt2;
                wayPts[j + 1] = pt1;
            }
        }

        else if (d.x >= rectangle.width)
        {
            print("Camera width maggiorne di ampiezza di campo!!");
            wayPts.Add(new Vector3((rectangle.xMax - rectangle.xMin) / 2f, h, rectangle.yMin + d.y / 2f));
            wayPts.Add(new Vector3((rectangle.xMax - rectangle.xMin) / 2f, h, rectangle.yMax - d.y / 2f));

        }

        else if (d.y >= rectangle.height)
        {
            print("Camera height maggiorne di altezza di campo!!");
            wayPts.Add(new Vector3(rectangle.xMin + (.5f * d.x), h, (rectangle.yMax - rectangle.yMin) / 2f));
            wayPts.Add(new Vector3(rectangle.xMax - (.5f * d.x), h, (rectangle.yMax - rectangle.yMin) / 2f));

        }



        return wayPts;

    }

    public void DrawWaypoint(List<Vector3> pts)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = pts.Count;

        for (int j = 0; j < pts.Count; j++)
        {
            lineRenderer.SetPosition(j, pts[j]);
        }

        lineRenderer.material = material;
        lineRenderer.loop = false;
    }

    public void PrintList(List<Vector3> elements)
    {
        print("List o Vector3");
        foreach (Vector3 element in elements)
        {
            Debug.Log(element);
        }
    }

    public void PrintMinMaxRect(Rect rect)
    {
        print("xMin: " + rect.xMin + "\t yMin: " + rect.yMin + "\t xMax" + rect.xMax + "\t yMax" + rect.yMax);

    }
}