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
        protected set
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
        protected set
        {
            health = value;
            UpdateHealthDisplay();
        }
    }
    [SerializeField] TextMeshProUGUI healthText;

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
}
