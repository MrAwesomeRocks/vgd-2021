using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public float speed = 500;
    public bool hasPowerup;
    public int powerUpDuration = 5;
    public GameObject powerupIndicator;
    public Vector3 powerupOffset = new Vector3(0, -0.6f, 0);
    public float normalStrength = 10; // how hard to hit enemy without powerup
    public float powerupStrength = 25; // how hard to hit enemy with powerup
    public float boostForce;
    public KeyCode boostKey;
    public ParticleSystem boostParticle;
    public Vector3 boostParticleOffset = new Vector3(0, -0.5f, 0);

    private GameObject focalPoint;
    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(verticalInput * speed * Time.deltaTime * focalPoint.transform.forward);

        // Set powerup indicator and boost particle position to beneath player
        powerupIndicator.transform.position = transform.position + powerupOffset;
        boostParticle.gameObject.transform.position = transform.position + boostParticleOffset;

        if (!boostParticle.isPlaying && Input.GetKeyDown(boostKey))
        {
            // If not specifying a direction, assume forward
            verticalInput = verticalInput == 0 ? 1 : verticalInput;

            playerRb.AddForce(verticalInput * boostForce * focalPoint.transform.forward, ForceMode.Impulse);
            boostParticle.Play();
        }
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }
}
