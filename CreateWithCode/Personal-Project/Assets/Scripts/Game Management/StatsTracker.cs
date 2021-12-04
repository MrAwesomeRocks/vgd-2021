using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(GameManager))]
public class StatsTracker : MonoBehaviour
{
    // The player score
    int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            UpdateScoreDisplay();
        }
    }
    [SerializeField] TextMeshProUGUI scoreText;

    // The player health
    int health;
    [SerializeField] int startingHealth;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            UpdateHealthDisplay();
        }
    }
    [SerializeField] TextMeshProUGUI healthText;

    // The player ammo
    int ammo;
    bool gunReloaded;
    [SerializeField] int startingAmmo;
    [SerializeField] int ammoClipSize;
    public int Ammo
    {
        get { return ammo; }
        set
        {
            ammo = value;
            if (Ammo % ammoClipSize == 0) { GunReloaded = false; }
            UpdateAmmoDisplay();
        }
    }
    public bool GunReloaded
    {
        get { return gunReloaded; }
        set
        {
            gunReloaded = value;
            UpdateAmmoDisplay();
        }
    }
    [SerializeField] TextMeshProUGUI ammoText;

    // Game manager
    GameManager gameManager;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Score = 0;
        Health = startingHealth;
        Ammo = startingAmmo;
        GunReloaded = true;

        gameManager = GetComponent<GameManager>();
    }

    public void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {Score}";
    }

    public void UpdateHealthDisplay()
    {
        healthText.text = $"Health: {Health}/{startingHealth}";

        if (Health < 0)
        {
            gameManager.GameLost();
        }
    }

    public void UpdateAmmoDisplay()
    {
        int ammoInGun = Ammo % ammoClipSize;
        int clipsRemaining = Ammo / ammoClipSize;
        if (ammoInGun == 0)
        {
            if (GunReloaded)
            {
                clipsRemaining--;
                ammoInGun = ammoClipSize;
            }
            else
            {
                ammoInGun = 0;
            }
        }

        ammoText.text = $"Ammo: {ammoInGun}/{ammoClipSize} ({clipsRemaining})";
    }
}
