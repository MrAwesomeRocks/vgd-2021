using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] Camera fpsCamera;
    GameManager gameManager;
    StatsTracker statsTracker;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        statsTracker = GameObject.Find("Game Manager").GetComponent<StatsTracker>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (gameManager.IsRunning)
        {
            if (Input.GetButtonDown("Fire1") && statsTracker.Ammo > 0)
            {
                if (statsTracker.GunReloaded)
                {
                    Shoot();
                    statsTracker.Ammo--;
                }
                else
                {
                    statsTracker.GunReloaded = true;
                }
            }
        }
    }

    void Shoot()
    {
        Vector3 fireDirection = fpsCamera.transform.forward * range;
        Vector3 fireStartPoint = fpsCamera.transform.position;

        if (Physics.Raycast(fireStartPoint, fireDirection, out RaycastHit hit, range))
        {
            ShotTarget target = hit.transform.GetComponent<ShotTarget>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        // Draw the fire ray
        Debug.DrawRay(fireStartPoint, fireDirection, Color.red);
    }
}
