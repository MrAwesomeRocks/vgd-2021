using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(GameManager))]
public class Scorekeeper : MonoBehaviour
{
    // The Score
    int score;
    public int Score
    {
        get { return score; }
        protected set
        {
            score = value;
            scoreText.text = $"Score: {score}/{requiredScore}";
        }
    }

    // Control variables
    [SerializeField] int scoreIncrement;
    [SerializeField] float scorePause;
    [SerializeField] int requiredScore;
    [SerializeField] List<int> scorePerTrick;
    [SerializeField] bool playerStartedCourse;

    // UI Elements
    [SerializeField] TextMeshProUGUI scoreText;

    // Game manager
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        playerStartedCourse = false;

        Score = 0;
        StartCoroutine(UpdateScore());
    }

    IEnumerator UpdateScore()
    {
        while (true)
        {
            if (gameManager.IsRunning && playerStartedCourse)
            {
                Score += scoreIncrement;
            }
            yield return new WaitForSeconds(scorePause);
        }
    }

    public void StartCourse()
    {
        playerStartedCourse = true;
    }

    public void AddScoreForTrick(PlayerController.TrickTypes trick)
    {
        Score += scorePerTrick[(int)trick];
    }
}
