using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [SerializeField] int selectedWeapon = 0;
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

        SelectWeapon();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (gameManager.IsRunning)
        {
            int previousSelectedWeapon = selectedWeapon;

            // Scroll wheel checks
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                {
                    // Overflowed
                    selectedWeapon = 0;
                }
                else
                {
                    selectedWeapon++;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= 0)
                {
                    // Overflowed
                    selectedWeapon = transform.childCount - 1;
                }
                else
                {
                    selectedWeapon--;
                }
            }

            for (int i = 1; i <= transform.childCount; i++)
            {
                if (Input.GetKeyDown($"{i}"))
                {
                    selectedWeapon = i - 1;
                    break;
                }
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                SelectWeapon();
            }
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
        statsTracker.UpdateAmmoDisplay(GetCurrentWeapon());
    }

    public void AddAmmoToCurrentGun(int amount)
    {
        AbstractWeaponController weapon = GetCurrentWeapon();
        weapon.AddAmmo(amount);
        statsTracker.UpdateAmmoDisplay(weapon);
    }

    public AbstractWeaponController GetCurrentWeapon()
    {
        return transform.GetChild(selectedWeapon).GetComponent<AbstractWeaponController>();
    }
}
