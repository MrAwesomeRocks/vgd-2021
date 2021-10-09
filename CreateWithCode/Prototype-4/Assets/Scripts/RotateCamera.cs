using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Have to multiply by -1 so that right arrow causes a right rotation (left to the camera)
        transform.Rotate(Vector3.up, -1.0f * horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
