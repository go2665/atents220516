using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreLine : MonoBehaviour
{
    TextMeshProUGUI highName;
    TextMeshProUGUI highScore;

    private void Awake()
    {
        highName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        highScore = transform.Find("Score").GetComponent<TextMeshProUGUI>();
    }

    public void SetHighName(string newName)
    {
        highName.text = newName;
    }

    public void SetHighScore(int score)
    {
        highScore.text = score.ToString();
    }
}
