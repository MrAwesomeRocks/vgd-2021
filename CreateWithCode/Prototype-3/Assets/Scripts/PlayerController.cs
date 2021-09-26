using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public bool gameOver;
    public byte jumpCount = 0;
    public bool speedUp;
    public KeyCode dashKey;
    public KeyCode jumpKey;
    public int score;
    public float scoreDelay;
    public float scorePrintDelay;

    public float walkInSpeed;
    public float startTransition;
    public bool starting = true;
    private Vector3 startPos;
    private Vector3 endPos = new Vector3(0, 0, 0);
    private float startTime;
    private float journeyLength;

    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    private AudioSource cameraAudio;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        cameraAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        // Walk-in animation
        startPos = transform.position;
        playerAnim.SetFloat("Speed_f", 0.5f);
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPos, endPos);

        Physics.gravity *= gravityModifier;

        // Increment score every tenth of a second
        InvokeRepeating(nameof(IncrementScore), scoreDelay, scoreDelay);
        // Print score every second
        InvokeRepeating(nameof(PrintScore), scorePrintDelay, scorePrintDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (starting)
        {
            // https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
            // Distance moved equals elapsed time times speed.
            float distCovered = (Time.time - startTime) * walkInSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            if (fractionOfJourney >= 1.0f)
            {
                starting = false;
            }
            else if (fractionOfJourney > startTransition)
            {
                // Start transition to running
                playerAnim.SetFloat("Speed_f", 1.0f);
            }

            // Set our position as a fraction of the distance between start and end positions
            transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
        }
        else
        {
            if (!gameOver && jumpCount < 2 && Input.GetKeyDown(jumpKey))
            {
                jumpCount++;

                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                playerAnim.SetTrigger("Jump_trig");
                dirtParticle.Stop();
                playerAudio.PlayOneShot(jumpSound, 1.0f);
            }

            // On the ground and dash key pressed down and game not over
            if (!gameOver && jumpCount == 0 && Input.GetKey(dashKey))
            {
                speedUp = true;
                playerAnim.speed = 2;
            }
            else
            {
                speedUp = false;
                playerAnim.speed = 1;
            }
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (!gameOver && !starting)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                jumpCount = 0;

                dirtParticle.Play();
            }
            else if (other.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("Game Over!");
                gameOver = true;

                playerAnim.SetInteger("DeathType_int", 1);
                playerAnim.SetBool("Death_b", true);

                explosionParticle.Play();
                dirtParticle.Stop();

                playerAudio.PlayOneShot(crashSound, 1.0f);
                Invoke(nameof(StopMusic), crashSound.length / 2);
            }
        }
    }

    void StopMusic()
    {
        cameraAudio.Stop();
    }

    void IncrementScore()
    {
        if (!gameOver && !starting)
        {
            // Player trying to speedup
            if (Input.GetKey(dashKey))
            {
                score += 2;
            }
            else
            {
                score += 1;
            }
        }
    }

    void PrintScore()
    {
        if (!gameOver && !starting)
        {
            // Score is actually score / time score inc per sec
            int actualScore = Mathf.RoundToInt(score * scoreDelay);
            Debug.Log($"Score: {actualScore}");
        }
    }
}
