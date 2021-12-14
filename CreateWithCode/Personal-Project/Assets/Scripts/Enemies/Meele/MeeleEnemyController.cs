using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A meele enemy that charges the player.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MeeleEnemyController : EnemyTarget {
    // Control vars
    [SerializeField] float speed;

    // Components
    Transform player;
    GameManager gameManager;
    AudioSource enemyAudio;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected override void Start() {
        // Superclass method
        base.Start();

        // Get components
        player = GameObject.Find("Player").GetComponent<Transform>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemyAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        if (gameManager.IsRunning) {
            // Game is running, make noise!
            if (gameManager.PlayerStartedMaze) {
                // Player has entered the maze, start attacking too!
                Vector3 path = player.position - transform.position;
                path = new Vector3(path.x, 0, path.z);
                transform.Translate(speed * Time.deltaTime * path.normalized);
            }

            if (!enemyAudio.isPlaying) {
                // Audio was stopped, restart it!
                enemyAudio.Play();
            }
        } else {
            // Game was stopped, be quiet!
            if (enemyAudio.isPlaying) {
                // Just paused, now stop audio.
                enemyAudio.Stop();
            }
        }
    }
}
