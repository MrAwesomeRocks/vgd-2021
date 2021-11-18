using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // Control variables
    [SerializeField] float motorTorque;
    [SerializeField] float steeringAngle;
    [SerializeField] float yMax;

    // Axle storage
    [System.Serializable]
    public struct AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool powered;
        public bool steerable;
    }
    [SerializeField] List<AxleInfo> axleInfos;

    // Center of mass
    [SerializeField] GameObject centerOfMass;
    Rigidbody rb;

    // Game manager stuff
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void FixedUpdate()
    {
        float motor, steering;
        if (IsOnGround())
        {
            motor = motorTorque * Input.GetAxis("Vertical");
            steering = steeringAngle * Input.GetAxis("Horizontal");

            if (transform.position.y > yMax)
            {
                gameManager.ChangeVehicleGrounded(false);
            }
            else
            {
                gameManager.ChangeVehicleGrounded(true);
            }
        }
        else
        {
            gameManager.ChangeVehicleGrounded(false);
            motor = 0;
            steering = 0;
        }

        foreach (AxleInfo axle in axleInfos)
        {
            if (axle.steerable)
            {
                axle.leftWheel.steerAngle = steering;
                axle.rightWheel.steerAngle = steering;
            }
            if (axle.powered)
            {
                axle.leftWheel.motorTorque = motor;
                axle.rightWheel.motorTorque = motor;
            }
        }
    }

    bool IsOnGround()
    {
        byte groundedWheels = 0;
        foreach (AxleInfo axle in axleInfos)
        {
            if (axle.leftWheel.isGrounded) { groundedWheels++; }
            if (axle.rightWheel.isGrounded) { groundedWheels++; }
        }

        return groundedWheels > 1;
    }
}
