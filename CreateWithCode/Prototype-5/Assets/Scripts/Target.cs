using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public int pointValue;

    private Rigidbody targetRb;
    private GameManager gameManager;
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -2;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // Since this object is spawned in another script,
        // add a random force and torque immediately
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        // Pick a random spawn position
        transform.position = RandomSpawnPos();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
    void OnMouseDown()
    {
        // Once the game is over, don't alow players to click objects anymore
        if (gameManager.isGameActive)
        {
            // If the object is clicked, destroy it, create a particle effect,
            // and increment the score
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger.
    void OnTriggerEnter(Collider other)
    {
        // The object has fallen off the screen
        Destroy(gameObject);

        if (!gameObject.CompareTag("Bad"))
        {
            // If the object is not a bad one, the game is over
            gameManager.GameOver();
        }
    }

    // Get a random force
    Vector3 RandomForce()
    {
        return Random.Range(minSpeed, maxSpeed) * Vector3.up;
    }

    // Get a random torque
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    // Get a random spawn position
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
