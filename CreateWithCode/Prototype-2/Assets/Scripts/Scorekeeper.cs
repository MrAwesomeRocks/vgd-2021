using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorekeeper : MonoBehaviour
{
    private int lives;
    public int Lives
    {
        get { return lives; }
        set
        {
            lives = value;
            if (lives > 0)
            {
                Debug.Log($"Lives = {lives}");
            }
            else
            {
                Debug.Log("Game Over!");
            }
        }
    }

    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            Debug.Log($"Score = {score}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Lives = 9;
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
