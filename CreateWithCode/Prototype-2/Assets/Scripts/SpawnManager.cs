using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;

    private const float spawnRangeX = 20.0f;
    private const float spawnPosX = 30.0f;
    private const float spawnPosZ = 20.0f;
    private const float spawnMinZ = 0.0f;
    private const float spawnMaxZ = 15.0f;
    private const float startDelay = 2;
    private const float spawnInterval = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomVerticalAnimal), startDelay, spawnInterval);
        InvokeRepeating(nameof(SpawnRandomHorizontalAnimal), startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     SpawnRandomAnimal();
        // }
    }

    void SpawnRandomVerticalAnimal()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);

        Instantiate(animalPrefabs[animalIndex], spawnPos, animalPrefabs[animalIndex].transform.rotation);
    }

    void SpawnRandomHorizontalAnimal()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        int spawnSide = Random.Range(0, 2) * 2 - 1; // +1 for right side, -1 for left side

        // Get the spawn position and rotation
        Vector3 spawnPos = new Vector3(spawnSide * spawnPosX, 0, Random.Range(spawnMinZ, spawnMaxZ));
        Quaternion rotation = spawnSide == -1 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 270, 0);

        Instantiate(animalPrefabs[animalIndex], spawnPos, rotation);
    }
}
