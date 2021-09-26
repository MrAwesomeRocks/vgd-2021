using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float sideToSideForce;
    public float jumpForce;
    public KeyCode jumpKey;

    private Rigidbody playerRb;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    [SerializeField] private byte numJumps;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayerSideToSide();
        MovePlayerUp();
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            numJumps = 0;
        }
    }

    // Moves the player side to side based on arrow key input
    void MovePlayerSideToSide()
    {
        // Get the input axes
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Move the player based on axes input
        playerRb.AddForce(horizontalInput * sideToSideForce * Vector3.right);
        playerRb.AddForce(verticalInput * sideToSideForce * Vector3.forward);
    }

    // Moves the player up based on jump input
    void MovePlayerUp()
    {
        // Check if the player can jump
        if (numJumps < 2 && Input.GetKeyDown(jumpKey))
        {
            numJumps++;

            // Move the player
            playerRb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }
    }
}
