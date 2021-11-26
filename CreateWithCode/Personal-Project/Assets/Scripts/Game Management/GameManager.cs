using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Game Management
    [SerializeField] bool isRunning;
    public bool IsRunning
    {
        get { return isRunning; }
        protected set { isRunning = value; }
    }
    [SerializeField] bool playerStartedMaze;
    public bool PlayerStartedMaze
    {
        get { return playerStartedMaze; }
        protected set {
            Debug.Log("Player started the maze!");
            playerStartedMaze = value; }
    }

    // UI elements
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject gameUIScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject directionsScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject backButton;

    // Sounds
    [SerializeField] AudioSource cameraAudio;
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] AudioClip actionSound;

    #region Unity Messages
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        IsRunning = false;
        PlayerStartedMaze = false;

        cameraAudio.Stop();
        playerAudio.Stop();

        // Show the title screen
        ShowTitleScreen();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (IsRunning)
            {
                // In the game, show the pause menu
                PauseGame();
            }
            else if (pauseScreen.activeSelf)
            {
                // In the pause menu, leave it
                UnPauseGame();
            }
        }
    }
    #endregion

    #region Menu Functions
    public void StartGame()
    {
        IsRunning = true;

        // Show the game UI
        HideAllUI();
        gameUIScreen.SetActive(true);

        // Start the music!
        cameraAudio.Play();

        // Grab the mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        IsRunning = false;
        pauseScreen.SetActive(true);

        // Stop all audio
        cameraAudio.Pause();
        playerAudio.Pause();

        // Release the cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnPauseGame()
    {
        IsRunning = true;
        pauseScreen.SetActive(false);

        // Resume the audio
        cameraAudio.UnPause();
        playerAudio.UnPause();

        // Grab the cursor again
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowSettings()
    {
        HideAllUI();
        settingsScreen.SetActive(true);
        backButton.SetActive(true);
    }

    public void ShowDirections()
    {
        HideAllUI();
        directionsScreen.SetActive(true);
        backButton.SetActive(true);
    }

    public void ShowCredits()
    {
        HideAllUI();
        creditsScreen.SetActive(true);
        backButton.SetActive(true);
    }

    public void ShowTitleScreen()
    {
        HideAllUI();
        titleScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    #endregion

    #region Game Events
    public void GameWon()
    {
        IsRunning = false;
        winScreen.SetActive(true);

        // Release the mouse
        Cursor.lockState = CursorLockMode.None;

        cameraAudio.Stop();
        // TODO: Sounds
        // playerAudio.PlayOneShot(winSound);
    }

    public void GameLost()
    {
        IsRunning = false;
        loseScreen.SetActive(true);

        // Release the mouse
        Cursor.lockState = CursorLockMode.None;

        cameraAudio.Stop();
        // TODO: Sounds
        // playerAudio.PlayOneShot(loseSound);
    }

    public void MazeStarted()
    {
        PlayerStartedMaze = true;
    }
    #endregion

    #region Utilities
    void HideAllUI()
    {
        titleScreen.SetActive(false);
        gameUIScreen.SetActive(false);
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        directionsScreen.SetActive(false);
        backButton.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    IEnumerator PlaySoundNumTimes(AudioClip sound, int numTimes)
    {
        for (int i = 0; i < numTimes; i++)
        {
            playerAudio.PlayOneShot(sound);

            while (playerAudio.isPlaying)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    #endregion
}
