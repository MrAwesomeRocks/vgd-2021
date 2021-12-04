using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] Image crosshair;
    [SerializeField] int fireRange;

    Vector3 fireDirection;
    Vector3 fireStartPoint;

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Raycast to see what the crosshair is pointing at
        // and update colors accordingly
        fireDirection = transform.forward * fireRange;
        fireStartPoint = transform.position;

        if (Physics.Raycast(fireStartPoint, fireDirection, out RaycastHit hit, Mathf.Infinity))
        {
            // Change the color based on what is under the crosshair
            if (hit.transform.CompareTag("Enemy"))
            {
                crosshair.color = Color.red;
            }
            else if (hit.transform.CompareTag("Powerup"))
            {
                crosshair.color = Color.green;
            }
            else
            {
                crosshair.color = Color.white;
            }
        }
        else
        {
            crosshair.color = Color.white;
        }

        // Debug the ray out in the editor:
        Debug.DrawRay(fireStartPoint, fireDirection, Color.green);
    }
}
