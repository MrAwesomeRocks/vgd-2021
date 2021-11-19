using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Bookeeping
    bool isOnGround;
    float buttonFirstPressedTime;
    bool buttonDown;

    // Button control
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] float minTimeForHeldButton;

    // Player control
    public float motorAmount;
    public float targetRotationY;


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
    }

    void HandleMainButtonTapped()
    {
        if (!isOnGround)
        {
            // Do a trick
        }
        // Driving only works with a held button
    }

    void HandleMainButtonHeld()
    {
        if (isOnGround)
        {
            motorAmount = 1;
        }
        else
        {
            // Do a trick
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
        isOnGround = grounded;

        buttonText.text = isOnGround ? "Accelerate!" : "Trick!";
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
