using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTraffic : MonoBehaviour
{
    // random speed with Unity's Random
    [SerializeField] private float speed;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        speed = Random.Range(5, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }
    }
}
