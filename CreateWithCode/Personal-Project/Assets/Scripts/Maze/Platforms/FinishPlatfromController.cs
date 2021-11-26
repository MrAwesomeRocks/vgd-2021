using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlatfromController : MonoBehaviour
{
    GameManager gameManager;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void OnPlayerEnter()
    {
        // Player has finished the maze!
        gameManager.GameWon();
    }
}
