using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MeeleEnemyController : EnemyTarget
{
    [SerializeField] float speed;
    Transform player;
    GameManager gameManager;
    AudioSource enemyAudio;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").GetComponent<Transform>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemyAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (gameManager.IsRunning)
        {
            if (gameManager.PlayerStartedMaze)
            {
                Vector3 path = player.position - transform.position;
                path = new Vector3(path.x, 0, path.z);
                transform.Translate(speed * Time.deltaTime * path.normalized);
            }

            if (!enemyAudio.isPlaying)
            {
                enemyAudio.Play();
            }
        }
        else
        {
            if (enemyAudio.isPlaying)
            {
                enemyAudio.Stop();
            }
        }
    }
}
