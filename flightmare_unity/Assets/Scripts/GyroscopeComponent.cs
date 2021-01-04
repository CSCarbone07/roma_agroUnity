using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeComponent : MonoBehaviour
{

    public Vector3 AngularVelocity;

    private Vector3 rotation;
    private Vector3 previousFrame_rotation;
    private void FixedUpdate()
    {
        rotation = transform.rotation.eulerAngles;
        AngularVelocity = (rotation - previousFrame_rotation) / Time.fixedDeltaTime;
        previousFrame_rotation = rotation;
        //Debug.Log(AngularVelocity);
    }
}