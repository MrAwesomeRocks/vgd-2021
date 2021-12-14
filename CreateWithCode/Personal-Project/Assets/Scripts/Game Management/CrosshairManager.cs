using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One of the game managers, this one changes the crosshair color based on enemy/friend.
/// </summary>
public class CrosshairManager : MonoBehaviour {
    // Control vars
    [SerializeField] Image crosshair;
    [SerializeField] int fireRange;  // Only for the debug ray length

    // Bookeeping
    Vector3 fireDirection;
    Vector3 fireStartPoint;

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        // Raycast to see what the crosshair is pointing at
        // and update colors accordingly
        fireDirection = transform.forward * fireRange;
        fireStartPoint = transform.position;

        if (Physics.Raycast(fireStartPoint, fireDirection, out RaycastHit hit, Mathf.Infinity)) {
            // Change the color based on what is under the crosshair
            if (hit.transform.CompareTag("Enemy")) {
                // It's an enemy!
                crosshair.color = Color.red;
            } else if (hit.transform.CompareTag("Powerup")) {
                // It's a powerup!
                crosshair.color = Color.green;
            } else {
                // Not looking at anything important
                crosshair.color = Color.white;
            }
        } else {
            // Not looking at any solid object.
            crosshair.color = Color.white;
        }

        // Draw the debug ray in the editor.
        Debug.DrawRay(fireStartPoint, fireDirection, Color.green);
    }
}
