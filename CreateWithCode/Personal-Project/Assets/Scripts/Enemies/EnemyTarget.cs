using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : ShotTarget
{
    [SerializeField] protected int scoreIncrease;

    public override void HealthDepleted()
    {
        // Enemy was killed
        scorekeeper.Score += scoreIncrease;
        Destroy(gameObject);
    }
}
