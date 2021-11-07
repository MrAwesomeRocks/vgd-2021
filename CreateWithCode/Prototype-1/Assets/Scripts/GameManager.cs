using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = false;
    public float yMin;
    public float zMax;

    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject restartScreen;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] bool onePlayerHasLost = false;

    // Audio
    [SerializeField] AudioSource cameraAudio;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] AudioClip backgroundMusic;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Reset all screens
        ShowTitleScreen();
    }

    public void QuitGame()
    {
        Debug.Log("Application is exiting!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartGame()
    {
        isGameActive = true;
        onePlayerHasLost = false;

        DeactivateScreens();
        inGameUI.SetActive(true);

        cameraAudio.loop = true;
        cameraAudio.PlayOneShot(backgroundMusic);
    }

    public void RestartGame()
    {
        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowCredits()
    {
        DeactivateScreens();
        creditsScreen.SetActive(true);
    }

    public void ShowTitleScreen()
    {
        DeactivateScreens();
        titleScreen.SetActive(true);
    }

    public void GameOver(bool win, int playerNumber)
    {
        if (win)
        {
            // A player won
            // Turn off the game
            isGameActive = false;

            // Turn on game-over screen
            DeactivateScreens();
            restartScreen.SetActive(true);

            // Turn on win text
            GameObject winText = restartScreen.transform.Find("Win Text").gameObject;
            winText.GetComponent<TextMeshProUGUI>().SetText($"Player {playerNumber} Wins!");
            winText.SetActive(true);

            // Turn off audio
            cameraAudio.Stop();
            cameraAudio.loop = false;
            cameraAudio.PlayOneShot(winSound);
        }
        else if (onePlayerHasLost)
        {
            // Not a win, and one player has already lost, so both players have lost
            // Turn off the game
            isGameActive = false;

            // Turn on game-over screen
            DeactivateScreens();
            restartScreen.SetActive(true);

            // Turn on lose text
            restartScreen.transform.Find("Lose Text").gameObject.SetActive(true);

            // Turn off audio
            cameraAudio.Stop();
            cameraAudio.loop = false;
            cameraAudio.PlayOneShot(loseSound);
        }
        else
        {
            // Only one player has lost
            onePlayerHasLost = true;
        }
    }

    void DeactivateScreens()
    {
        titleScreen.SetActive(false);
        restartScreen.SetActive(false);
        creditsScreen.SetActive(false);
        inGameUI.SetActive(false);
    }
}
