using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Start platform controller. Keeps the player in the maze once started.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class StartPlatformController : MonoBehaviour {
    GameManager gameManager;
    BoxCollider boxCollider;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Called when the player leaves the platform.
    /// </summary>
    public void OnPlayerLeave() {
        // Only run when the player hasn't started the maze.
        if (!gameManager.PlayerStartedMaze) {
            // Tell the game manager the player has started the maze.
            gameManager.MazeStarted();
            Debug.Log("Player started maze!");

            // Move the box collider up to prevent the player from re-entering
            boxCollider.center += new Vector3(0, 40, 0);
            boxCollider.size += new Vector3(0, 80, 0);
        }
    }
}
