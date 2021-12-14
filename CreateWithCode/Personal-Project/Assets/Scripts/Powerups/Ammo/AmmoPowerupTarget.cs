using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerupTarget : PowerupTarget {
    /// <summary>
    /// The increase in player ammo.
    /// </summary>
    [SerializeField] int ammoIncrease;
    PlayerController playerController;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected override void Start() {
        // Call the superclass start method
        base.Start();

        // Get the player controller
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public override void HealthDepleted() {
        // Add the ammo to the gun
        playerController.weaponSwitcher.AddAmmoToCurrentGun(ammoIncrease);

        // Call the superclass
        base.HealthDepleted();
    }
}
