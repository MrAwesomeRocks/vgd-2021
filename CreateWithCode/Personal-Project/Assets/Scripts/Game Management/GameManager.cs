using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The main game manager. Handles game states and menus.
/// </summary>
public class GameManager : MonoBehaviour {
    // Game states
    [SerializeField] bool isRunning;
    /// <summary>
    /// Is the game running?
    /// </summary>
    public bool IsRunning
    {
        get { return isRunning; }
        protected set { isRunning = value; }
    }
    [SerializeField] bool playerStartedMaze;
    /// <summary>
    /// Has the player started the maze?
    /// </summary>
    public bool PlayerStartedMaze
    {
        get { return playerStartedMaze; }
        protected set {
            Debug.Log("Player started the maze!");
            playerStartedMaze = value;
        }
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

    #region Unity Messages
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        // Reset game control vars.
        IsRunning = false;
        PlayerStartedMaze = false;

        // Stop all audio
        cameraAudio.Stop();
        playerAudio.Stop();

        // Show the title screen
        ShowTitleScreen();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        // Check for the pause menu
        if (Input.GetButtonDown("Cancel")) {
            // Player hit Escape
            if (IsRunning) {
                // In the game, show the pause menu
                PauseGame();
            } else if (pauseScreen.activeSelf) {
                // In the pause menu, leave it
                UnPauseGame();
            }
        }
    }
    #endregion

    #region Menu Functions
    /// <summary>
    /// Start the game
    /// </summary>
    public void StartGame() {
        // Set the control variable
        IsRunning = true;

        // Show the game UI
        HideAllUI();
        gameUIScreen.SetActive(true);

        // Start the music!
        cameraAudio.Play();

        // Grab the mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame() {
        // Set the control variable
        IsRunning = false;

        // Show the pause screen
        pauseScreen.SetActive(true);

        // Stop all audio
        cameraAudio.Pause();
        playerAudio.Pause();

        // Release the cursor
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Unpause the game. DIFFERENT FROM START!!!
    /// </summary>
    public void UnPauseGame() {
        // Set the control variable
        IsRunning = true;

        // Hide the pause screen
        pauseScreen.SetActive(false);

        // Resume the audio
        cameraAudio.UnPause();
        playerAudio.UnPause();

        // Grab the cursor again
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Show the settings screen
    /// </summary>
    public void ShowSettings() {
        // Reset UI
        HideAllUI();

        // Show screen and back button
        settingsScreen.SetActive(true);
        backButton.SetActive(true);
    }

    /// <summary>
    /// Show the directions screen
    /// </summary>
    public void ShowDirections() {
        // Reset UI
        HideAllUI();

        // Show screen and back button
        directionsScreen.SetActive(true);
        backButton.SetActive(true);
    }

    /// <summary>
    /// Show the credits screen
    /// </summary>
    public void ShowCredits() {
        // Reset UI
        HideAllUI();

        // Show screen and back button
        creditsScreen.SetActive(true);
        backButton.SetActive(true);
    }

    /// <summary>
    /// Show the title screen.
    /// For the back button.
    /// </summary>
    public void ShowTitleScreen() {
        // Reset UI
        HideAllUI();

        // Show screen
        titleScreen.SetActive(true);
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    public void RestartGame() {
        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame() {
        Debug.Log("Application is exiting!");

#if UNITY_EDITOR
        // In the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // In a built app
        Application.Quit();
#endif
    }
    #endregion

    #region Game Events
    /// <summary>
    /// Play the game win sequence.
    /// </summary>
    public void GameWon() {
        // Stop the game
        IsRunning = false;

        // Show the win screen.
        winScreen.SetActive(true);

        // Release the mouse
        Cursor.lockState = CursorLockMode.None;

        // Play the win audio.
        cameraAudio.Stop();
        playerAudio.PlayOneShot(winSound);
    }

    /// <summary>
    /// Play the game lose sequence.
    /// </summary>
    public void GameLost() {
        // Stop the game
        IsRunning = false;

        // Show the lose screen
        loseScreen.SetActive(true);

        // Release the mouse
        Cursor.lockState = CursorLockMode.None;

        // Play the win audio
        cameraAudio.Stop();
        playerAudio.PlayOneShot(loseSound);
    }

    /// <summary>
    /// Set that the maze has been started.
    /// </summary>
    public void MazeStarted() {
        PlayerStartedMaze = true;
    }
    #endregion

    #region Utilities
    /// <summary>
    /// Reset all the UI by hiding it.
    /// </summary>
    void HideAllUI() {
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
    #endregion
}
