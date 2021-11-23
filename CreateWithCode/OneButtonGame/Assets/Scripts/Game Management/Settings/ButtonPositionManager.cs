using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonPositionManager : MonoBehaviour
{
    [SerializeField] GameObject rightButton;
    [SerializeField] GameObject leftButton;
    [SerializeField] TMP_Dropdown dropdown;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Add event listener for when the dropdown changes
        dropdown.onValueChanged.AddListener(delegate
        {
            OnButtonLocationChange(dropdown);
        });
    }
    public enum ButtonLocations
    {
        RIGHT,
        LEFT
    }

    public void OnButtonLocationChange(TMP_Dropdown change)
    {
        Debug.Log("Dropdown changed!");
        
        rightButton.SetActive(false);
        leftButton.SetActive(false);

        if (change.value == (int)ButtonLocations.RIGHT)
        {
            rightButton.SetActive(true);
        }
        else
        {
            leftButton.SetActive(true);
        }
    }
}
