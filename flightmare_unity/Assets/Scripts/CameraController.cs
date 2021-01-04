using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    //private Vector3 rotateValue;
    public GameObject drone;
    public Vector3 offset = new Vector3(0f, 20f, 0f);
    //public float speedCamera = 1.0f;
    //public Vector3 eulerAngleVelocity;

    //public Vector3 Vec3torque = new Vector3(10.0f, 0.0f, 0.0f);
    //public float torque = 10.0f;

    //public Camera camera;
    //public Vector3 Kp = new Vector3(.1f, .1f, .8f);
    //public Vector3 Kd = new Vector3(.05f, .1f, .05f);
    //public Vector3 cameraAngle;
    //public Vector3 oldCameraAngle;
    //public Vector3 droneAngle;
    //public Vector3 oldDroneAngle;
    //public Vector3 error;
    //public Vector3 dotError;
    //public Vector3 desiredAngleVelocity;
    //public Vector3 cameraAngleVelocity;
    //public Vector3 inputSpeed;
    //public Vector3 cameraPosDisplacement = new Vector3(0.0f, 0.2f, 0.0f);
    //public Vector3 cameraAngleDisplacement = new Vector3(90f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {

        //camera = gameObject.GetComponent<Camera>();

        //droneMovementScript = drone.GetComponentInParent<DroneMovement>();


        //Set the axis the Rigidbody rotates in (100 in the x axis)
        //eulerAngleVelocity = new Vector3(100.0f, 0.0f, 0.0f);


        //oldDroneAngle = new Vector3(0f, 0f, 0f); //drone.transform.eulerAngles;
        //oldCameraAngle = new Vector3(0f, 0f, 0f);  //transform.eulerAngles;
    }



    // Update is called once per frame
    void Update()
    {
        //MoveCamera();
        //ControlCamera();
        //ControlCamera1();

        transform.position = drone.transform.position - offset;
        //transform.eulerAngles = drone.transform.eulerAngles + new Vector3(90.0f, 0.0f, 0.0f);
        //Quaternion targetAngle = new Quaternion(droneMovementScript.tiltAmountSideways, 0f, droneMovementScript.tiltAmountForward, 1f);

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, .1f);
        //targetTransform.rotation = Quaternion.Slerp(transform.rotation, drone.transform.rotation, .1f);
        //transform.rotation = Quaternion.Euler(new Vector3(0f, targetTransform.rotation.y, 0f));

    }



    //private void MoveCamera()
    //{
    //    Quaternion perfectRotation = new Quaternion(90f, 0.0f, 0.0f, 1f);

    //    transform.localRotation = Quaternion.RotateTowards(transform.rotation, perfectRotation, speedCamera * Time.deltaTime);
    //}

    //private void ControlCamera()
    //{
    //    cameraAngle = transform.eulerAngles;
    //    droneAngle = droneMovementScript.ourDrone.transform.eulerAngles;
    //    //print("#######droneAngle:" + droneAngle);

    //    error.x = Mathf.DeltaAngle(cameraAngle.x, droneAngle.x / 10f); //10*3.14/180
    //    error.z = Mathf.DeltaAngle(cameraAngle.z, droneAngle.z / 10f);

    //    desiredAngleVelocity = droneMovementScript.ourDrone.angularVelocity;
    //    //print("#######desiredAngleVelocity:" + desiredAngleVelocity);
    //    print("#######droneMovementScript.ourDrone.velocity:" + droneMovementScript.ourDrone.velocity);
    //    print("#######droneMovementScript.ourDrone.angularVelocity:" + droneMovementScript.ourDrone.angularVelocity);
    //    //desiredAngleVelocity = (droneAngle - oldDroneAngle) / Time.deltaTime;
    //    cameraAngleVelocity = (cameraAngle - oldCameraAngle) / Time.deltaTime;

    //    dotError = desiredAngleVelocity - cameraAngleVelocity;


    //    inputSpeed = Vector3.Scale(Kp, error) + Vector3.Scale(Kd, dotError);

    //    //oldDroneAngle = droneAngle;
    //    oldCameraAngle = cameraAngle;

    //    rotateValue = new Vector3(inputSpeed.x, droneAngle.y, 0.0f);    //inputSpeed.z);
    //    //transform.eulerAngles = rotateValue;


    //    //print("desiredAngleVelocity: " + desiredAngleVelocity);
    //    //print("cameraAngleVelocity: " + cameraAngleVelocity);
    //    //print("dotError: " + dotError);
    //    //print("rotateValue: " + rotateValue);

    //}

    //private void ControlCamera1()
    //{
    //    rbCamera.AddTorque(transform.right * torque);
    //    rbCamera.AddRelativeTorque(Vec3torque, ForceMode.VelocityChange);

    //    transform.position = drone.transform.position + new Vector3(0.0f, -1.0f, 0.0f);
    //    transform.localEulerAngles = new Vector3(droneAngle.x/10f, 0, droneAngle.z/10f);//Quaternion.identity;


    //    print("drone angle: " + drone.transform.localEulerAngles);
    //    print("camera angle: " + transform.localEulerAngles);
    //}



    //Wrap between 0 and 360 degrees
    float WrapAngle(float inputAngle)
    {
        //The inner % 360 restricts everything to +/- 360
        //+360 moves negative values to the positive range, and positive ones to > 360
        //the final % 360 caps everything to 0...360
        return ((inputAngle % 360f) + 360f) % 360f;
    }

}