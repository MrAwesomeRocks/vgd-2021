using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    // Control vars
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float impactForce;
    [SerializeField]
    float fireRate;

    // Effects
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem mazeImpactEffect;
    [SerializeField] ParticleSystem nonMazeImpactEffect;

    // Other
    [SerializeField] Camera fpsCamera;
    GameManager gameManager;
    StatsTracker statsTracker;

    // Bookeeping
    float nextTimeToFire = 0f;

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
            if (Input.GetButton("Fire1") && statsTracker.Ammo > 0 && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
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
        muzzleFlash.Play();

        Vector3 fireDirection = fpsCamera.transform.forward * range;
        Vector3 fireStartPoint = fpsCamera.transform.position;

        if (Physics.Raycast(fireStartPoint, fireDirection, out RaycastHit hit, range))
        {
            ShotTarget target = hit.transform.GetComponent<ShotTarget>();

            if (target != null)
            {
                // Hit an enemy or powerup
                target.TakeDamage(damage);
                Instantiate(nonMazeImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            }
            else
            {
                if (hit.transform.CompareTag("Ground") || hit.transform.CompareTag("MazeWall") || hit.transform.CompareTag("Platform"))
                {
                    // Hit the maze
                    Instantiate(mazeImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            // Draw the fire ray
            Debug.DrawRay(fireStartPoint, fireDirection, Color.red);
        }
    }
}
