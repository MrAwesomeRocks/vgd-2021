using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Scorekeeper))]
public class DifficultyManager : MonoBehaviour
{
    [SerializeField] List<int> difficultyToScore;
    [SerializeField] TMP_Dropdown dropdown;
    Scorekeeper scorekeeper;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        scorekeeper = GetComponent<Scorekeeper>();

        // Add event listener for when the dropdown changes
        dropdown.onValueChanged.AddListener(delegate
        {
            OnDifficultyChange(dropdown);
        });

        // Set the default difficulty to Medium
        dropdown.value = 1;
        OnDifficultyChange(dropdown);
    }

    public void OnDifficultyChange(TMP_Dropdown change)
    {
        Debug.Log("Difficulty dropdown changed!");

        scorekeeper.RequiredScore = difficultyToScore[change.value];
        scorekeeper.UpdateScoreDisplay();
    }
}
