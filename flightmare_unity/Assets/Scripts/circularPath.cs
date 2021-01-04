using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularPath : MonoBehaviour
{

    public float timer = 0f;
    public float desiredTime = 10f;
    public float timeLeft = 10f; 

    public float desiredangY = 0f;
    public Vector3 desiredPos = new Vector3(0, 0, 0);
    public Vector3 desiredVel = new Vector3(0, 0, 0);
    public float radius = 10f;
    public Vector3 C = new Vector3(0, 0, 0);
    public bool onOff = true;
    private float distance = 0f;
    public GameObject target;

    void Start()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
    }
    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(distance) >= 1f)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);
        }
        else
        {
            if (timeLeft >= 0.0f)
            {
                timeLeft -= Time.deltaTime;
                timer += Time.deltaTime % 60;
                DesiredPosition();
                DesiredOrientation();
                distance = .5f; //valore a cazzo per non far verificare il vecchio if
            }
        }
    }

    private Vector3 DesiredPosition()
    {
        //desiredPos.z = C.z * Mathf.Sin(timer * F.z); //* Mathf.PI / 180
        //desiredPos.x = C.x * Mathf.Cos(timer * F.x); //* Mathf.PI / 180
        //print("timer / desiredTime: " + (timer / desiredTime));
        desiredPos.z = C.z + (radius * Mathf.Sin(timer / desiredTime * 2 * Mathf.PI)); //* Mathf.PI / 180
        desiredPos.x = C.x + (radius * Mathf.Cos(timer / desiredTime * 2 * Mathf.PI)); //* Mathf.PI / 180



        //print("desiredPos : " + desiredPos);
        transform.position = new Vector3(desiredPos.x, 20f, desiredPos.z);



        return transform.position;
    }

    private float DesiredOrientation()
    {
        desiredangY = -Mathf.Atan2(desiredPos.z, desiredPos.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0f, desiredangY, 0);
        return desiredangY;
    }
}
