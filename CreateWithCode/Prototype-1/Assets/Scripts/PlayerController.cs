using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // cameras
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public KeyCode cameraSwitchKey;

    // Driving improvements
    private Rigidbody playerRb;
    [SerializeField] private float horsePower;
    [SerializeField] private GameObject centerOfMass;
    [SerializeField] List<WheelCollider> allWheels;

    // UI
    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] TextMeshProUGUI rpmText;
    [SerializeField] private int speed;
    [SerializeField] private int rpm;

    // control var
    [SerializeField] private string inputSuffix;
    [SerializeField] private float turnSpeed = 55.0f;
    [SerializeField] private float horizontalInput = 0.0f;
    [SerializeField] private float forwardInput = 0.0f;

    // Call this function to disable FPS camera,
    // and enable third person camera.
    private void ToggleCamera()
    {
        firstPersonCamera.enabled = !firstPersonCamera.enabled;
        thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
    }

    // Start is called before the first frame update
    void Start()
    {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.enabled = true;

        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (IsOnGround())
        {

            // get inputs
            horizontalInput = Input.GetAxis($"Horizontal{inputSuffix}");
            forwardInput = Input.GetAxis($"Vertical{inputSuffix}");

            // Drive wheels the right way
            foreach (WheelCollider wheel in allWheels)
            {
                // move the vehicle forward based on vertical input
                // transform.Translate(forwardInput * speed * Time.deltaTime * Vector3.forward);
                // playerRb.AddRelativeForce(horsePower * forwardInput * Vector3.forward);
                wheel.motorTorque = horsePower * forwardInput;

                // rotate the vehicle based on horizontal input
                //transform.Rotate(Vector3.up, horizontalInput * turnSpeed * Time.deltaTime);
                if (wheel.name.IndexOf("f") != -1)
                {
                    // Only front wheels steer
                    wheel.steerAngle = turnSpeed * horizontalInput;
                }
            }

            speed = Mathf.RoundToInt(playerRb.velocity.magnitude * 2.237f);
            speedometerText.SetText($"Speed: {speed} mph");

            rpm = speed % 30 * 40;
            rpmText.SetText($"RPM: {rpm}");
        }

        // Cameras
        if (Input.GetKeyDown(cameraSwitchKey))
        {
            ToggleCamera();
        }
    }

    bool IsOnGround()
    {
        int wheelsOnGround = 0;
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }

        return wheelsOnGround == 4;
    }
}
