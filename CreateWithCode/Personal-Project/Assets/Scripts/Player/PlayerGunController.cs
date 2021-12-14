using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The controller of the player's weapon, handling shooting.
/// </summary>
[RequireComponent(typeof(AbstractWeaponController))]
public class PlayerGunController : MonoBehaviour {
    // Control vars
    [SerializeField] KeyCode reloadKey;

    // Components
    [SerializeField] Camera fpsCamera;
    GameManager gameManager;
    StatsTracker statsTracker;
    AbstractWeaponController weaponController;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        // Get componenets
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        statsTracker = GameObject.Find("Game Manager").GetComponent<StatsTracker>();
        weaponController = GetComponent<AbstractWeaponController>();

        // Update the stats display with the current weapon state.
        statsTracker.UpdateAmmoDisplay(weaponController);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        // Only do things when the game is running
        if (gameManager.IsRunning) {
            // Check for inputs
            if (Input.GetButton("Fire1")) {
                // Player hit the fire button
                Debug.Log("Fire1");
                if (weaponController.GunReloaded) {
                    // Gun is reloaded, so shoot and update ammo display
                    weaponController.Shoot(fpsCamera.transform.forward, fpsCamera.transform.position);
                    statsTracker.UpdateAmmoDisplay(weaponController);
                } else {
                    // Gun not reloaded, reload with a callback to update the ammo display.
                    weaponController.Reload(() => { statsTracker.UpdateAmmoDisplay(weaponController); });
                }
            }
            if (Input.GetKeyDown(reloadKey)) {
                // Player asked for a reload, reload with a callback to update the ammo display.
                Debug.Log("reload");
                weaponController.Reload(() => { statsTracker.UpdateAmmoDisplay(weaponController); });
            }
        }
    }
}
