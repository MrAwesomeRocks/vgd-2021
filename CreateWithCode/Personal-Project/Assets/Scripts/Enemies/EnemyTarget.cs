using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple enemy. Should be subclassed.
/// </summary>
public class EnemyTarget : ShotTarget
{
    /// <summary>
    /// How much to increase the player's score when they defeat this enemy.
    /// </summary>
    [SerializeField] protected int scoreIncrease;

    public override void HealthDepleted()
    {
        // Enemy was killed
        scorekeeper.Score += scoreIncrease;
        Destroy(gameObject);
    }
}
