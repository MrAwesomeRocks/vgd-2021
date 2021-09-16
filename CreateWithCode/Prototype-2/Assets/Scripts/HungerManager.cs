using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerManager : MonoBehaviour
{
    public Slider hungerSlider;
    public int food = 0;
    public int totalFood;

    private Scorekeeper scorekeeper;

    // Start is called before the first frame update
    void Start()
    {
        hungerSlider.maxValue = totalFood;
        hungerSlider.value = 0;
        hungerSlider.gameObject.SetActive(false);  // Hide slider

        scorekeeper = GameObject.Find("Scorekeeper").GetComponent<Scorekeeper>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FeedAnimal() {
        food++;
        hungerSlider.gameObject.SetActive(true);  // Show slider
        hungerSlider.value = food;

        if (food >= totalFood) {
            GetComponent<MoveForward>().enabled = false;
            
            scorekeeper.Score++;
            Destroy(gameObject, 0.15f);
        }
    }
}
