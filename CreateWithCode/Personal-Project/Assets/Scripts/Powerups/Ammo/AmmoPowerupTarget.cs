using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerupTarget : PowerupTarget
{
    [SerializeField] int ammoIncrease;
    PlayerController playerController;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public override void HealthDepleted()
    {
        playerController.weaponSwitcher.AddAmmoToCurrentGun(ammoIncrease);
        base.HealthDepleted();
    }
}
