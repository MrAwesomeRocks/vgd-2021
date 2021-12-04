using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTarget : ShotTarget
{
    public override void HealthDepleted()
    {
        Destroy(gameObject);
    }
}
