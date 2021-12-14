using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple powerup. Should be subclassed.
/// </summary>
public class PowerupTarget : ShotTarget {
    public override void HealthDepleted() {
        // Just simply destroy the game object
        Destroy(gameObject);
    }
}
