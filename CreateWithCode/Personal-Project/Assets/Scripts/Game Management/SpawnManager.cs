using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager), typeof(MazeManager))]
public class SpawnManager : MonoBehaviour
{
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
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        mazeManager = GetComponent<MazeManager>();

        if (enemyPrefabs.Length > 0) { StartCoroutine(SpawnEnemies()); }
        if (powerupPrefabs.Length > 0) { StartCoroutine(SpawnPowerups()); }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (gameManager.IsRunning)
            {
                var (posX, posZ) = JitterSpawnPos(mazeManager.GetRandomSpawnPosition());
                int index = Random.Range(0, enemyPrefabs.Length);
                Instantiate(
                    enemyPrefabs[index],
                    new Vector3(posX, enemyPrefabs[index].transform.position.y, posZ),
                    enemyPrefabs[index].transform.rotation,
                    enemyContainer
                );
            }
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    IEnumerator SpawnPowerups()
    {
        while (true)
        {
            if (gameManager.IsRunning)
            {
                var (posX, posZ) = JitterSpawnPos(mazeManager.GetRandomSpawnPosition());
                int index = Random.Range(0, powerupPrefabs.Length);
                Instantiate(
                    powerupPrefabs[index],
                    new Vector3(posX, powerupPrefabs[index].transform.position.y, posZ),
                    powerupPrefabs[index].transform.rotation,
                    powerupContainer
                );
            }
            yield return new WaitForSeconds(powerupSpawnDelay);
        }
    }

    (float X, float Z) JitterSpawnPos((float, float) pos)
    {
        var (posX, posZ) = pos;
        float jitterRange = (MazeManager.TILE_SIZE - 1) / 2.0f;

        return (
            posX + Random.Range(-jitterRange, jitterRange),
            posZ + Random.Range(-jitterRange, jitterRange)
        );
    }
}
