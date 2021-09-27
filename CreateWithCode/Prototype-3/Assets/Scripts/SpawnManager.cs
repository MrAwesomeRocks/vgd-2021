using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float startDelay;
    public float spawnDelayMin;
    public float spawnDelayMax;

    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private PlayerController playerControllerScript;
    [SerializeField] private float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        Invoke(nameof(SpawnObstacle), startDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnObstacle()
    {
        if (!playerControllerScript.gameOver && !playerControllerScript.starting)
        {
            int index = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[index], spawnPos, obstaclePrefabs[index].transform.rotation);
        }

        spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
        Invoke(nameof(SpawnObstacle), spawnDelay);
    }
}
