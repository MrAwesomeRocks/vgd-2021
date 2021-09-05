using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // cameras
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public KeyCode cameraSwitchKey;

    // control var
    public int vehicleNumber;
    private const float speed = 20.0f;
    private const float turnSpeed = 55.0f;
    private float horizontalInput = 0.0f;
    private float forwardInput = 0.0f;

    // Call this function to disable FPS camera,
    // and enable third person camera.
    private void ToggleCamera() {
        firstPersonCamera.enabled = !firstPersonCamera.enabled;
        thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
    }

    // Start is called before the first frame update
    void Start()
    {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // get inputs
        horizontalInput = Input.GetAxis($"Horizontal{vehicleNumber}");
        forwardInput = Input.GetAxis($"Vertical{vehicleNumber}");

        // move the vehicle forward based on vertical input
        transform.Translate(forwardInput * speed * Time.deltaTime * Vector3.forward);
        // rotate the vehicle forward based on horizontal input
        transform.Rotate(Vector3.up, horizontalInput * turnSpeed * Time.deltaTime);

        // Cameras
        if (Input.GetKeyDown(cameraSwitchKey)) {
            ToggleCamera();
        }
    }
}
