using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wavePath : MonoBehaviour
{

    public float timer = 0f;
    public float timeLeft = 30f;

    public float k = 5f; //direttamente propriorzionale alla velocità della sfera sulla x
    public float A = 10f;
    public float offset = 0f;
    public float T = 10f;
    public float linearT = .2f; //inversamente propriorzionale alla velocità della sfera sulla z

    public Vector3 actualPosition;
    public Vector3 oldPosition = new Vector3(0, 0, 0);
    public bool vertical = false;
    public bool forward = false;

    public float velocity = 0f;


    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime % 60;
        if(timeLeft >= 0f)
        {
            oldPosition = transform.position;
            actualPosition = DesiredPosition();
            velocity = (actualPosition - oldPosition).magnitude / Time.deltaTime;
        }

    }

    private Vector3 DesiredPosition()
    {
        //desiredPos.z = C.z * Mathf.Sin(timer * F.z); //* Mathf.PI / 180
        //desiredPos.x = C.x * Mathf.Cos(timer * F.x); //* Mathf.PI / 180
        //print("timer / desiredTime: " + (timer / desiredTime));
        //desiredPos.z = C.z + (radius * Mathf.Sin(timer / desiredTime * 2 * Mathf.PI)); //* Mathf.PI / 180
        //desiredPos.x = C.x + (radius * Mathf.Cos(timer / desiredTime * 2 * Mathf.PI)); //* Mathf.PI / 180

        if (vertical == false)
        {

            timer += Time.deltaTime % 60;

            print("Vertical is FALSE");
            Vector3 newPosition = new Vector3(k * timer, 20f, (A * Mathf.Sign(Mathf.Sin(2f * Mathf.PI * ((k * timer - offset) / T)))));
            //print("desiredPos : " + desiredPos);

            if (Mathf.Abs(newPosition.z - oldPosition.z) < 2f)
            {
                transform.position = newPosition;
            }
            else
            {
                vertical = true;
            }
        }

        if (vertical == true)
        {

            float timer1 = 0f;
            timer1 += Time.deltaTime % 60;

            if (!forward)
            {
                transform.Translate(-Vector3.forward * timer1 / linearT);
                if ((transform.position.z) <= -A)
                {
                    vertical = false;
                    forward = true;
                }
            }

            else if (forward)
            {
                transform.Translate(Vector3.forward * timer1 / linearT);
                if ((transform.position.z) >= A)
                {
                    vertical = false;
                    forward = false;
                }
            }
        }

        return transform.position;
    }

   
}
