using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Part of the game managers, this one keeps track of stats and updates the game stats UI.
/// </summary>
[RequireComponent(typeof(GameManager))]
public class StatsTracker : MonoBehaviour {
    // The player score
    int score;
    /// <summary>
    /// The player score. Setting updates the score display.
    /// </summary>
    public int Score
    {
        get { return score; }
        set {
            score = value;
            UpdateScoreDisplay();
        }
    }
    [SerializeField] TextMeshProUGUI scoreText;

    // The player health
    int health;
    [SerializeField] int startingHealth;
    /// <summary>
    /// The player health. Setting updates the score display.
    /// </summary>
    public int Health
    {
        get { return health; }
        set {
            health = value;
            UpdateHealthDisplay();
        }
    }
    [SerializeField] TextMeshProUGUI healthText;

    // The player ammo
    [SerializeField] TextMeshProUGUI ammoText;

    // Game manager
    GameManager gameManager;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        // Init vars
        Score = 0;
        Health = startingHealth;

        // Get components.
        gameManager = GetComponent<GameManager>();
    }

    /// <summary>
    /// Update the score display with the current score.
    /// </summary>
    public void UpdateScoreDisplay() {
        scoreText.text = $"Score: {Score}";
    }

    /// <summary>
    /// Update the health display with the current health.
    /// </summary>
    public void UpdateHealthDisplay() {
        healthText.text = $"Health: {Health}/{startingHealth}";

        if (Health <= 0) {
            gameManager.GameLost();
        }
    }

    /// <summary>
    /// Update the ammo display.
    /// </summary>
    /// <param name="weapon">The weapon to use for the ammo display.</param>
    public void UpdateAmmoDisplay(AbstractWeaponController weapon) {
        ammoText.text = $"Ammo: {weapon.AmmoInGun}/{weapon.AmmoClipSize} ({weapon.AmmoRemaining})";
    }
}
