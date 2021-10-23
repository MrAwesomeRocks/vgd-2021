using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;
    public bool isGameActive;

    private float spawnRate = 1.0f;
    private int score;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnTarget()
    {
        // Only spawn objects when the game is running
        while (isGameActive)
        {
            // Delay spawning
            yield return new WaitForSeconds(spawnRate);

            // Choose a random object and spawn it
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        // Add score
        score += scoreToAdd;
        // Re-display
        scoreText.text = $"Score: {score}";
    }

    public void GameOver()
    {
        // Turn on game-over screen
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        // Turn off game
        isGameActive = false;
    }

    public void RestartGame()
    {
        // Reload the scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        // Initialize variables
        isGameActive = true;
        score = 0;
        // Set spawn rate based on difficulty
        spawnRate /= difficulty;

        // Start spawning and display initial score
        StartCoroutine(SpawnTarget());
        UpdateScore(0);

        // Turn off the title screen
        titleScreen.gameObject.SetActive(false);
    }
}
