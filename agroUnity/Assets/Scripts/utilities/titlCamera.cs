using UnityEngine;
using System.Collections;

public class titlCamera : MonoBehaviour
{
    private float x;
    private Vector3 rotateValue;
    public float minAngle = -25;
    public float maxAngle = 45;
    public GameObject player;

    void LateUpdate()
    {

        MoveCamera();

    }

    private float WrapAngle(float angle) 
    {
        return ((angle + 180f) % 360f) - 180f;
    }

    private void MoveCamera()
    {
        x = Input.GetAxis("Mouse Y");
        //Debug.Log(x + ":");
        float increment = WrapAngle((float)(transform.eulerAngles.x - x));
        rotateValue = new Vector3(Mathf.Clamp(increment, minAngle, maxAngle), player.transform.eulerAngles.y, 0);

        transform.eulerAngles = rotateValue;
    }


}
