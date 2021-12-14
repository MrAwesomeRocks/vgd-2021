using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour {
    // Bookeeping and componenets
    [SerializeField] int selectedWeapon = 0;
    GameManager gameManager;
    StatsTracker statsTracker;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        // Get componenets
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        statsTracker = GameObject.Find("Game Manager").GetComponent<StatsTracker>();

        // Select initial weapon
        SelectWeapon();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        // Only do things when the game is running
        if (gameManager.IsRunning) {
            // Get the previous weapon to prevent unnecessary updates
            int previousSelectedWeapon = selectedWeapon;

            //$ Scrollwheel checks
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                // Scrolled up
                if (selectedWeapon >= transform.childCount - 1) {
                    // Overflowed
                    selectedWeapon = 0;
                } else {
                    // Go to next weapon
                    selectedWeapon++;
                }
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                // Scrolled down
                if (selectedWeapon <= 0) {
                    // Overflowed
                    selectedWeapon = transform.childCount - 1;
                } else {
                    // Go the previous weapon
                    selectedWeapon--;
                }
            }

            // Check for number key press
            for (int i = 1; i <= transform.childCount; i++) {
                if (Input.GetKeyDown($"{i}")) {
                    // Numeric button pressed, set weapon and break
                    selectedWeapon = i - 1;  // List index
                    break;
                }
            }

            // Only update when weapon was changed
            if (previousSelectedWeapon != selectedWeapon) {
                SelectWeapon();
            }
        }
    }

    /// <summary>
    /// Show the selected weapon.
    /// </summary>
    void SelectWeapon() {
        // Loop through each child weapon
        int i = 0;
        foreach (Transform weapon in transform) {
            if (i == selectedWeapon) {
                // Selected, show weapon
                weapon.gameObject.SetActive(true);
            } else {
                // Not selected, hide
                weapon.gameObject.SetActive(false);
            }

            // Increment counter
            i++;
        }

        // Update weapon stats
        statsTracker.UpdateAmmoDisplay(GetCurrentWeapon());
    }

    /// <summary>
    /// Add ammunition to the current selected weapon.
    /// </summary>
    /// <param name="amount">How much ammunition to add.</param>
    public void AddAmmoToCurrentGun(int amount) {
        // Get the current weapon
        AbstractWeaponController weapon = GetCurrentWeapon();

        // Add ammo
        weapon.AddAmmo(amount);

        // Update display
        statsTracker.UpdateAmmoDisplay(weapon);
    }

    /// <summary>
    /// Get the current selected weapon controller.
    /// </summary>
    /// <returns>The weapon controller of the currently selected weapon.</returns>
    public AbstractWeaponController GetCurrentWeapon() {
        return transform.GetChild(selectedWeapon).GetComponent<AbstractWeaponController>();
    }
}
