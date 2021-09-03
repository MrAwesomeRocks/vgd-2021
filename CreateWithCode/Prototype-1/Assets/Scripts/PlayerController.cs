using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // control vars
    private const float speed = 20.0f;
    private const float turnSpeed = 55.0f;
    private float horizontalInput = 0.0f;
    private float forwardInput = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // get inputs
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // move the vehicle forward based on vertical input
        transform.Translate(forwardInput * speed * Time.deltaTime * Vector3.forward);
        // rotate the vehicle forward based on horizontal input
        transform.Rotate(Vector3.up, horizontalInput * turnSpeed * Time.deltaTime);
    }
}
