using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An <see langword="abstract"/> class for GameObjects that can be shot at by the player.
/// </summary>
public abstract class ShotTarget : MonoBehaviour {
    /// <summary>
    /// The amount of health in the shot target.
    /// </summary>
    [SerializeField] protected float health;
    protected StatsTracker scorekeeper;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected virtual void Start() {
        scorekeeper = GameObject.Find("Game Manager").GetComponent<StatsTracker>();
    }

    /// <summary>
    /// Called when this target is shot.
    /// Deals a certain <paramref name="amount"/> of tamage to the target.
    /// </summary>
    /// <param name="amount">The amount of damage to deal to the target.</param>
    public void TakeDamage(float amount) {
        health -= amount;

        if (health <= 0f) {
            HealthDepleted();
        }
    }

    /// <summary>
    /// What to do when this object runs out of health.
    /// </summary>
    public abstract void HealthDepleted();
}
