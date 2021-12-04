using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotTarget : MonoBehaviour
{
    [SerializeField] protected float health;
    protected StatsTracker scorekeeper;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected virtual void Start()
    {
        scorekeeper = GameObject.Find("Game Manager").GetComponent<StatsTracker>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            HealthDepleted();
        }
    }

    public abstract void HealthDepleted();
}
