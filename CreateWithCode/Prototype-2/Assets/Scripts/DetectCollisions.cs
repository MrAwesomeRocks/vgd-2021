using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    private Scorekeeper scorekeeper;
    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // https://answers.unity.com/questions/976620/ontriggerenter-called-before-start.html
        scorekeeper = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scorekeeper.Lives--;
            Destroy(gameObject);
        }
        else if (other.CompareTag("Projectile"))
        {
            // Pizza collision detection
            // Instead of destroying the projectile when it collides with an animal
            //Destroy(other.gameObject);

            // Just deactivate the food and feed the animal
            other.gameObject.SetActive(false);
            GetComponent<HungerManager>().FeedAnimal();
        }
    }
}
