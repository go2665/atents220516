using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    ImageNumber score;
    ImageNumber highScore;

    private void Awake()
    {
        score = transform.Find("Score_ImageNumber").GetComponent<ImageNumber>();
        highScore = transform.Find("HighScore_ImageNumber").GetComponent<ImageNumber>();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        score.Number = GameManager.Inst.Score;
        highScore.Number = GameManager.Inst.HighScore;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }
}
