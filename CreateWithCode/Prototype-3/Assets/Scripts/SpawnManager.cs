using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float startDelay = 2.0f;
    public float repeatRate = 2.0f;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
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
    }
}
