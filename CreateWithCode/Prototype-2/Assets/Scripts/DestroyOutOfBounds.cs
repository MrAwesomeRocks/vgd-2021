using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private const float topBound = 30.0f;
    private const float lowerBound = -10.0f;
    private const float xRange = 40.0f;
    private Scorekeeper scorekeeper;

    // Start is called before the first frame update
    void Start()
    {
        scorekeeper = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        // If an object goes past the player, remove the object
        if (transform.position.z > topBound)
        {
            // Instead of destroying the projectile when it leaves the screen
            //Destroy(gameObject);

            // Just deactivate it
            gameObject.SetActive(false);
        }
        else if (transform.position.z < lowerBound)
        {
            // Also notify for game over
            scorekeeper.Lives--;
            Destroy(gameObject);
        }

        if (Mathf.Abs(transform.position.x) > xRange)
        {
            // Animal walked off the screen
            scorekeeper.Lives--;
            Destroy(gameObject);
        }
    }
}
