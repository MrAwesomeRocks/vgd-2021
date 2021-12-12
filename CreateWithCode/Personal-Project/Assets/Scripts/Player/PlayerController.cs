using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // Control vars
    [SerializeField] float movementSpeed;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float verticalLookRange;
    [SerializeField] float jumpSpeed;
    [SerializeField] int meeleDamage;
    [SerializeField] float meeleDamageDelay;

    // Audio
    [SerializeField] AudioClip hurtSound;
    AudioSource playerAudio;

    // Bookeeping
    byte numJumps;
    float verticalRotation;
    float verticalVelocity;
    float nextMeeleDamageTime = 0f;

    // Components
    public PlayerWeaponSwitcher weaponSwitcher;
    GameManager gameManager;
    MazeManager mazeManager;
    StatsTracker statsTracker;
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        // Init vars
        numJumps = 0;
        verticalRotation = 0;
        verticalVelocity = 0;

        // Get components
        characterController = GetComponent<CharacterController>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        statsTracker = GameObject.Find("Game Manager").GetComponent<StatsTracker>();
        mazeManager = GameObject.Find("Game Manager").GetComponent<MazeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsRunning)
        {
            //$ Ground check
            if (characterController.isGrounded)
            {
                // Done jumping
                numJumps = 0;

                // Once the player is on the ground, they aren't moving down anymore
                if (verticalVelocity < 0)
                {
                    verticalVelocity = 0;
                }
            }

            //$ Rotation
            // Rotate left and right
            float leftRightRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
            transform.Rotate(0, leftRightRotation, 0);

            // Rotate camera up and down
            verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookRange, verticalLookRange);
            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

            //! Movement
            //$ XZ Movement (WASD)
            float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
            float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

            //$ Y Movement (Jump + Gravity)
            // Jumping
            if (numJumps < 2 && Input.GetButtonDown("Jump"))
            {
                numJumps++;
                verticalVelocity += jumpSpeed;
            }

            // Gravity
            verticalVelocity += Physics.gravity.y * Time.deltaTime;

            //$ Move the player
            // Get the speed
            Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
            Vector3 localSpeed = transform.TransformDirection(speed);

            // Move the player
            characterController.Move(Time.deltaTime * localSpeed);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (gameManager.IsRunning)
        {
            if (!gameManager.PlayerStartedMaze && hit.collider.gameObject.CompareTag("Ground"))
            {
                // Player started the maze
                mazeManager.StartPlatform.GetComponent<StartPlatformController>().OnPlayerLeave();
                Debug.Log("Sent maze start message");
            }
            else if (hit.collider.gameObject.name == mazeManager.FinishPlatform.name)
            {
                mazeManager.FinishPlatform.GetComponent<FinishPlatfromController>().OnPlayerEnter();
                Debug.Log("Sent maze finish message");
            }
            else if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                if (Time.time >= nextMeeleDamageTime)
                {
                    statsTracker.Health -= meeleDamage;
                    playerAudio.PlayOneShot(hurtSound);

                    nextMeeleDamageTime = Time.time + meeleDamageDelay;
                }
            }
        }
    }

    public void MoveToStartPosition(float x, float z)
    {
        transform.position = new Vector3(x, 0.5f, z);
    }
}
