using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTraffic : MonoBehaviour
{
    // random speed with Unity's Random
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(5, 15);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}
