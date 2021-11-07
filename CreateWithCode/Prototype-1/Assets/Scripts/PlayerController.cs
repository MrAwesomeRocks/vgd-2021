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
    [SerializeField] int speed;
    [SerializeField] int rpm;
    GameManager gameManager;

    // control var
    [SerializeField] string inputSuffix;
    [SerializeField] float turnSpeed = 55.0f;
    [SerializeField] float horizontalInput = 0.0f;
    [SerializeField] float forwardInput = 0.0f;
    [SerializeField] bool hasLost = false;

    // Sounds
    [SerializeField] AudioClip crashSound;
    AudioSource playerAudio;

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
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerAudio = GetComponent<AudioSource>();

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
        if (gameManager.isGameActive && !hasLost)
        {
            if (transform.position.y < gameManager.yMin)
            {
                // Fell below the road, game over
                gameManager.GameOver(false, GetPlayerNum());
                hasLost = true;
                return;
            }
            else if (transform.position.z > gameManager.zMax)
            {
                // Player made it to the end of the road and won
                gameManager.GameOver(true, GetPlayerNum());
                return;
            }

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
            else
            {
                // Not on ground, reset wheels
                foreach (WheelCollider wheel in allWheels)
                {
                    wheel.motorTorque = 0;
                    wheel.steerAngle = 0;
                }
            }

            // Cameras
            if (Input.GetKeyDown(cameraSwitchKey))
            {
                ToggleCamera();
            }
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (gameManager.isGameActive && !hasLost && !other.gameObject.CompareTag("Road"))
        {
            playerAudio.PlayOneShot(crashSound, 1.0f);
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

    int GetPlayerNum()
    {
        return inputSuffix == "" ? 1 : int.Parse(inputSuffix);
    }
}
