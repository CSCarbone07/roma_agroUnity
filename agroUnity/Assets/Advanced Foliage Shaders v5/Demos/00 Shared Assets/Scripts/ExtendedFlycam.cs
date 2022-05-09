using UnityEngine;

public class ExtendedFlycam : MonoBehaviour
{
    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX;
    private float rotationY;

    private void Start()
    {
        rotationX = transform.eulerAngles.y;
    }

    private void Update() {
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
        }

        Quaternion targetRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        targetRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4f);

        float speedFactor = 1f;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) speedFactor = fastMoveFactor;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) speedFactor = slowMoveFactor;

        transform.position += transform.forward * normalMoveSpeed * speedFactor * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += transform.right * normalMoveSpeed * speedFactor * Input.GetAxis("Horizontal") * Time.deltaTime;
        float upAxis = 0;
        if (Input.GetKey(KeyCode.Q)) upAxis = -0.5f;
        if (Input.GetKey(KeyCode.E)) upAxis = 0.5f;
        transform.position += transform.up * normalMoveSpeed * speedFactor * upAxis * Time.deltaTime;
    }
}