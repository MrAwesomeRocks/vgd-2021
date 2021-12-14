using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Part of the game managers. this one spawns enemies and powerups.
/// </summary>
[RequireComponent(typeof(GameManager), typeof(MazeManager))]
public class SpawnManager : MonoBehaviour {
    // Enemies
    [SerializeField] Transform enemyContainer;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float enemySpawnDelay;

    // Powerups
    [SerializeField] Transform powerupContainer;
    [SerializeField] GameObject[] powerupPrefabs;
    [SerializeField] float powerupSpawnDelay;

    // Components
    GameManager gameManager;
    MazeManager mazeManager;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        // Get components
        gameManager = GetComponent<GameManager>();
        mazeManager = GetComponent<MazeManager>();

        // Start spawning if prefabs are available.
        if (enemyPrefabs.Length > 0) { StartCoroutine(SpawnEnemies()); }
        if (powerupPrefabs.Length > 0) { StartCoroutine(SpawnPowerups()); }
    }

    IEnumerator SpawnEnemies() {
        while (true) {
            // Only spawn enemies when the game is running
            if (gameManager.IsRunning) {
                // Jitter the spawn pos so spawns don't happen in a line
                var (posX, posZ) = JitterSpawnPos(mazeManager.GetRandomSpawnPosition());

                // Get a random enemy
                int index = Random.Range(0, enemyPrefabs.Length);

                // Spawn the enemy
                Instantiate(
                    enemyPrefabs[index],
                    new Vector3(posX, enemyPrefabs[index].transform.position.y, posZ),
                    enemyPrefabs[index].transform.rotation,
                    enemyContainer
                );
            }
            // Pause
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    IEnumerator SpawnPowerups() {
        while (true) {
            // Only spawn powerups when the game is running
            if (gameManager.IsRunning) {
                // Jitter the spawn pos so spawns don't happen in a line
                var (posX, posZ) = JitterSpawnPos(mazeManager.GetRandomSpawnPosition());

                // Get a random powerup
                int index = Random.Range(0, powerupPrefabs.Length);

                // Spawn the powerup
                Instantiate(
                    powerupPrefabs[index],
                    new Vector3(posX, powerupPrefabs[index].transform.position.y, posZ),
                    powerupPrefabs[index].transform.rotation,
                    powerupContainer
                );
            }
            // Pause
            yield return new WaitForSeconds(powerupSpawnDelay);
        }
    }

    /// <summary>
    /// Jitter a spawn position within a maze square.
    /// </summary>
    /// <param name="pos">The initial position in the center of the square.</param>
    /// <returns>The newly jittered spawn position.</returns>
    (float X, float Z) JitterSpawnPos((float, float) pos) {
        var (posX, posZ) = pos;
        float jitterRange = (MazeManager.TILE_SIZE - 1) / 2.0f;

        return (
            posX + Random.Range(-jitterRange, jitterRange),
            posZ + Random.Range(-jitterRange, jitterRange)
        );
    }
}
