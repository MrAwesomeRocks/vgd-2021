using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbstractWeaponController))]
public class PlayerGunController : MonoBehaviour
{
    [SerializeField] Camera fpsCamera;
    [SerializeField] KeyCode reloadKey;
    GameManager gameManager;
    StatsTracker statsTracker;
    AbstractWeaponController weaponController;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        statsTracker = GameObject.Find("Game Manager").GetComponent<StatsTracker>();
        weaponController = GetComponent<AbstractWeaponController>();

        statsTracker.UpdateAmmoDisplay(weaponController);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (gameManager.IsRunning)
        {
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("Fire1");
                if (weaponController.GunReloaded)
                {
                    weaponController.Shoot(fpsCamera.transform.forward, fpsCamera.transform.position);
                }
                else
                {
                    weaponController.Reload();
                }
                statsTracker.UpdateAmmoDisplay(weaponController);
            }
            if (Input.GetKeyDown(reloadKey))
            {
                Debug.Log("reload");
                weaponController.Reload();
                statsTracker.UpdateAmmoDisplay(weaponController);
            }
        }
    }
}
