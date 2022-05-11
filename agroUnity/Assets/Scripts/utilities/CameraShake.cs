//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraShake : MonoBehaviour
//{
//    public bool shaking;

//    // How long the object should shake for.
//    public float shakeDuration = 10f;

//    // Amplitude of the shake. A larger value shakes the camera harder.
//    public float shakeAmount = 0.7f;
//    public float decreaseFactor = 1.0f;

//    public Vector3 originalPos;
//    public Quaternion originalRot;


//    public char axis = 'z';

//    void Start()
//    {
//        shaking = false;
//    }
//    //void Awake()
//    //{
//    //    if (camTransform == null)
//    //    {
//    //        camTransform = GetComponent(typeof(Transform)) as Transform;
//    //    }
//    //}

//    void OnEnable()
//    {
//        originalPos = transform.localPosition;
//        originalRot = transform.localRotation;
//    }

//    void Update()
//    {
//        ApplyShaking(axis);
//    }

//    public void ApplyShaking(char ax)
//    {

//        if (shakeDuration > 0)
//        {
//            if (ax.Equals('z'))
//            {
//                //float shakePos = originalPos.z + Random.insideUnitSphere.z * shakeAmount;
//                //transform.localPosition = new Vector3(originalPos.x, originalPos.y, shakePos);

//                float shakeRot = originalRot.x + Random.Range(-shakeAmount, shakeAmount) * 10f;
//                print("originalRot.x: " + originalRot.x);
//                print("shakeRot: " + shakeRot);
//                //transform.localRotation = new Quaternion(shakeRot, originalRot.y, originalRot.z, originalRot.w);
//                //Vector3 angles = new Vector3(shakeRot, originalRot.y, originalRot.z);
//                //transform.Rotate(angles, Space.Self);

//                //Quaternion fromRotation = transform.rotation;
//                Quaternion toRotation = Quaternion.Euler(shakeRot, originalRot.y, originalRot.z);
//                transform.localRotation = Quaternion.Lerp(originalRot, toRotation, Time.deltaTime * .5f);
//            }
//            else if (ax.Equals('x'))
//            {
//                float shakePos = originalPos.x + Random.insideUnitSphere.x * shakeAmount;
//                transform.localPosition = new Vector3(shakePos, originalPos.y, originalPos.z);

//                //float shakeRot = originalRot.z + Random.Range(-shakeAmount, shakeAmount) * .2f;
//                //transform.localRotation = new Quaternion(originalRot.x, originalRot.y, shakeRot, originalRot.w);
//            }



//            shakeDuration -= Time.deltaTime * decreaseFactor;
//        }
//        else
//        {
//            shakeDuration = 0f;
//            transform.localPosition = originalPos;
//            //transform.localRotation = originalRot;
//        }
//    }
//}

using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    public bool debugMode = false;//Test-run/Call ShakeCamera() on start

    public float shakeAmount;//The amount to shake this frame.
    public float shakeDuration;//The duration this frame.

    //Readonly values...
    float shakePercentage;//A percentage (0-1) representing the amount of shake to be applied when setting rotation.
    float startAmount;//The initial shake amount (to determine percentage), set when ShakeCamera is called.
    float startDuration;//The initial shake duration, set when ShakeCamera is called.

    bool isRunning = false; //Is the coroutine running right now?

    public bool smooth;//Smooth rotation?
    public float smoothAmount = 5f;//Amount to smooth

    void Start()
    {

        if (debugMode) ShakeCamera();
    }


    void ShakeCamera()
    {

        startAmount = shakeAmount;//Set default (start) values
        startDuration = shakeDuration;//Set default (start) values

        if (!isRunning) StartCoroutine(Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
    }

    public void ShakeCamera(float amount, float duration)
    {

        shakeAmount += amount;//Add to the current amount.
        startAmount = shakeAmount;//Reset the start amount, to determine percentage.
        shakeDuration += duration;//Add to the current time.
        startDuration = shakeDuration;//Reset the start time.

        if (!isRunning) StartCoroutine(Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
    }


    IEnumerator Shake()
    {
        isRunning = true;

        while (shakeDuration > 0.01f)
        {
            Vector3 rotationAmount = Random.insideUnitSphere * shakeAmount;//A Vector3 to add to the Local Rotation
            rotationAmount.z = 0;//Don't change the Z; it looks funny.

            shakePercentage = shakeDuration / startDuration;//Used to set the amount of shake (% * startAmount).

            shakeAmount = startAmount * shakePercentage;//Set the amount of shake (% * startAmount).
            shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);//Lerp the time, so it is less and tapers off towards the end.


            if (smooth)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * smoothAmount);
            else
                transform.localRotation = Quaternion.Euler(rotationAmount);//Set the local rotation the be the rotation amount.

            yield return null;
        }
        transform.localRotation = Quaternion.identity;//Set the local rotation to 0 when done, just to get rid of any fudging stuff.
        isRunning = false;
    }

}
