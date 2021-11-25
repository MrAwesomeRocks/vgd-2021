using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Control vars
    [SerializeField] float movementSpeed;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float verticalLookRange;
    [SerializeField] float jumpSpeed;

    // Bookeeping
    byte numJumps;
    float verticalRotation;
    [SerializeField] float verticalVelocity;
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        // Init vars
        numJumps = 0;
        verticalRotation = 0;
        verticalVelocity = 0;

        // Get the rigidbody
        characterController = GetComponent<CharacterController>();

        // Hide Mouse
        // TODO: Move to GameManager
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //$ Ground check
        if (characterController.isGrounded)
        {
            // Done jumping
            numJumps = 0;

            // Once the player is on the ground, they aren't moving down anymore
            if (verticalVelocity < 0)
            {
                Debug.Log("Reset vertical velocity!");
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
