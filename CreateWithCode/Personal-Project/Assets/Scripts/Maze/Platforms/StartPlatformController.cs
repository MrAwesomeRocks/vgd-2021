using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StartPlatformController : MonoBehaviour
{
    GameManager gameManager;
    BoxCollider boxCollider;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void OnPlayerLeave()
    {
        if (!gameManager.PlayerStartedMaze)
        {
            gameManager.MazeStarted();
            Debug.Log("Player started maze!");

            // Move the box collider up to prevent the player from re-entering
            boxCollider.center += new Vector3(0, 40,0);
            boxCollider.size += new Vector3(0, 80, 0);
        }
    }
}
