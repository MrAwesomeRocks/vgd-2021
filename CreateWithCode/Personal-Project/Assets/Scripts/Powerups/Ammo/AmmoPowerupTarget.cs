using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerupTarget : PowerupTarget
{
    [SerializeField] int ammoIncrease;

    public override void HealthDepleted()
    {
        scorekeeper.Ammo += ammoIncrease;
        base.HealthDepleted();
    }
}
