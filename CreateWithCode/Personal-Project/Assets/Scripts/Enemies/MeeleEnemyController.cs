using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleEnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    Transform player;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector3 path = player.position - transform.position;
        path = new Vector3(path.x, 0, path.z);
        transform.Translate(speed * Time.deltaTime * path.normalized);
    }
}
