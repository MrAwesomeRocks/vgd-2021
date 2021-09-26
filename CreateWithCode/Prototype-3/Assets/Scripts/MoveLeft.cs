using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private const float slowSpeed = 40;
    private const float fastSpeed = 60;

    private float speed;
    private PlayerController playerControllerScript;
    private const float leftBound = -15.0f;
    private bool isObstacle;

    // Start is called before the first frame update
    void Start()
    {
        speed = slowSpeed;

        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        isObstacle = gameObject.CompareTag("Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.starting)
        {
            if (!playerControllerScript.gameOver)
            {
                transform.Translate(speed * Time.deltaTime * Vector3.left);
            }

            if (playerControllerScript.speedUp)
            {
                speed = fastSpeed;
            }
            else
            {
                speed = slowSpeed;
            }

            if (isObstacle && transform.position.x < leftBound)
            {
                Destroy(gameObject);
            }
        }
    }
}
