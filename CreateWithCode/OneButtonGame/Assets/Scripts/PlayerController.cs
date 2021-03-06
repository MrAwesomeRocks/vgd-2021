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
    [SerializeField] float maxRpm;
    [SerializeField] float trickTorque;

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
    ButtonManager buttonManager;
    Scorekeeper scorekeeper;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        buttonManager = GameObject.Find("Game Manager").GetComponent<ButtonManager>();
        scorekeeper = GameObject.Find("Game Manager").GetComponent<Scorekeeper>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (gameManager.IsRunning)
        {
            float zPos = transform.position.z;
            for (int i = 0; i < buttonManager.drivingMilestones.Count - 1; i++)
            {
                if (buttonManager.drivingMilestones[i].zPos < zPos
                    && buttonManager.drivingMilestones[i + 1].zPos > zPos)
                {
                    buttonManager.drivingMilestones[i].Reach();
                    break;
                }
            }

            int lastMilestoneIndex = buttonManager.drivingMilestones.Count - 1;
            if (buttonManager.drivingMilestones[lastMilestoneIndex].zPos < zPos)
            {
                buttonManager.drivingMilestones[lastMilestoneIndex].Reach();
            }
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void FixedUpdate()
    {
        if (gameManager.IsRunning)
        {
            float motor, steering;
            if (IsOnGround())
            {
                motor = motorTorque * buttonManager.motorAmount;
                steering = steeringAngle * GetSteeringMultiplier();

                if (transform.position.y > yMax)
                {
                    buttonManager.ChangeVehicleGrounded(false);
                }
                else
                {
                    buttonManager.ChangeVehicleGrounded(true);
                }
            }
            else
            {
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
                    // https://forum.unity.com/threads/wheels-cause-undesired-acceleration.442501/#post-2863034
                    axle.leftWheel.motorTorque = axle.leftWheel.rpm < maxRpm ? motor : 0;
                    axle.rightWheel.motorTorque = axle.rightWheel.rpm < maxRpm ? motor : 0;
                }
            }
        }
    }

    public enum TrickTypes
    {
        One80,
        Three60,
        Flip
    }

    public void DoTrick(TrickTypes type)
    {
        scorekeeper.AddScoreForTrick(type);
        switch (type)
        {
            case TrickTypes.One80:
                // Handled by default
                return;
            case TrickTypes.Three60:
                rb.AddRelativeTorque(0, Mathf.Sign(GetSteeringMultiplier()) * trickTorque, 0, ForceMode.Impulse);
                gameManager.PlayTrickSound(1);
                return;
            case TrickTypes.Flip:
                rb.AddRelativeTorque(trickTorque, 0, 0, ForceMode.Impulse);
                gameManager.PlayTrickSound(2);
                return;
            default:
                // Should never happen
                return;
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

    float GetSteeringMultiplier()
    {
        float adjustedYRotation = GetAdjustedYRotation();
        float difference = adjustedYRotation - buttonManager.targetRotationY;

        if (difference < -1)
        {
            // Debug.Log($"Raw Rotation: {transform.rotation.eulerAngles.y}; Rotation: {adjustedYRotation}; Difference: {difference}; Mult: 1");
            return 1;
        }
        else if (difference > 1)
        {
            // Debug.Log($"Raw Rotation: {transform.rotation.eulerAngles.y}; Rotation: {adjustedYRotation}; Difference: {difference}; Mult: -1");
            return -1;
        }
        else
        {
            return 0;
        }
    }

    float GetAdjustedYRotation()
    {
        float yRotation = transform.rotation.eulerAngles.y;
        if (yRotation > 180)
        {
            return yRotation - 360;
        }
        else
        {
            return yRotation;
        }
    }
}
