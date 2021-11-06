using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public bool thirdPersonCamera = true;
    [SerializeField] private Vector3 offset1st = new Vector3(0, 4.5f, 0);
    [SerializeField] private Vector3 offset3rd = new Vector3(0, 6.25f, -7);

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        if (thirdPersonCamera)
        {
            transform.position = player.transform.position + offset3rd;
        }
        else
        {
            transform.SetPositionAndRotation(player.transform.position + offset1st, player.transform.rotation);
        }
    }
}
