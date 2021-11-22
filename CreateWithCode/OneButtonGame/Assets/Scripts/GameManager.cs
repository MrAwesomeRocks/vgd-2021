using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Bookeeping
    bool isOnGround;
    float buttonFirstPressedTime;
    bool buttonDown;
    float buttonLastTapTime;
    [SerializeField] bool buttonTapped;
    float buttonLastHeldTime;
    [SerializeField] bool buttonHeld;

    // Button control
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] float minTimeForHeldButton;
    [SerializeField] float timeToResetButtonBookeeping;

    // Player control
    public float motorAmount;
    public float targetRotationY;
    [SerializeField] PlayerController player;

    // Milestones
    [System.Serializable]
    public class MilestoneInfo
    {
        public float zPos;
        bool passed = false;
        [SerializeField] UnityEvent milestoneEvent;

        public void Reach()
        {
            if (!passed)
            {
                passed = true;
                milestoneEvent.Invoke();
            }
        }
    }
    public List<MilestoneInfo> drivingMilestones;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (buttonDown && Time.time - buttonFirstPressedTime > minTimeForHeldButton)
        {
            // Button was held
            HandleMainButtonHeld();
        }

        // Reset held and tapped times
        if (Time.time - buttonLastHeldTime > timeToResetButtonBookeeping)
        {
            buttonHeld = false;
        }
        if (Time.time - buttonLastTapTime > timeToResetButtonBookeeping)
        {
            buttonTapped = false;
        }
    }

    public void OnCourseStart(float drivingRotation)
    {
        targetRotationY = drivingRotation;
    }

    void HandleMainButtonTapped()
    {
        // Driving only works with a held button
        if (isOnGround) return;

        buttonLastTapTime = Time.time;
        buttonTapped = true;

        if (buttonHeld)
        {
            // Player does a 360
            Debug.Log("Player did a 360!");
            player.DoTrick(PlayerController.TrickTypes.Three60);
        }
        else
        {
            // Player does a flip
            Debug.Log("Player did a flip!");
            player.DoTrick(PlayerController.TrickTypes.Flip);
        }
    }

    void HandleMainButtonHeld()
    {
        if (isOnGround)
        {
            motorAmount = 1;
        }
        else
        {
            buttonLastHeldTime = Time.time;
            buttonHeld = true;
        }
    }

    public void OnMainButtonDown()
    {
        buttonFirstPressedTime = Time.time;
        buttonDown = true;
    }

    public void OnMainButtonUp()
    {
        buttonDown = false;
        if (Time.time - buttonFirstPressedTime < minTimeForHeldButton)
        {
            // Button was tapped
            HandleMainButtonTapped();
        }
        // Button was held, already handled
        motorAmount = 0;
    }

    public void ChangeVehicleGrounded(bool grounded)
    {
        if (grounded == isOnGround) return;  // Only update when new value is different

        if (grounded)
        {
            // Landed
            buttonText.text = "Accelerate!";
        }
        else
        {
            buttonText.text = "Trick!";
            targetRotationY *= -1;
        }

        isOnGround = grounded;
    }

    public void QuitGame()
    {
        Debug.Log("Application is exiting!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
