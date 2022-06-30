using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    public Sprite[] medalSprites;

    ImageNumber score;
    ImageNumber highScore;
    Image highScoreMark;
    Image medalImage;

    private void Awake()
    {
        score = transform.Find("Score_ImageNumber").GetComponent<ImageNumber>();
        highScore = transform.Find("HighScore_ImageNumber").GetComponent<ImageNumber>();
        highScoreMark = transform.Find("New").GetComponent<Image>();
        medalImage = transform.Find("MedalImage").GetComponent<Image>();

        Button restartButton = transform.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(Restart);
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        score.Number = GameManager.Inst.Score;
        highScore.Number = GameManager.Inst.BestScore;
    }

    public void Open(bool isHighScore)
    {
        // 하이스코어 일 때 빨간 new 표시
        if( isHighScore )
        {
            highScoreMark.color = Color.white;
        }
        else
        {
            highScoreMark.color = Color.clear;
        }

        // 점수별 메달 설정
        int score = GameManager.Inst.Score;        
        if (score >= 400)
        {
            medalImage.sprite = medalSprites[3];
            medalImage.color = Color.white;
        }
        else if(score >= 300)
        {
            medalImage.sprite = medalSprites[2];
            medalImage.color = Color.white;
        }
        else if (score >= 200)
        {
            medalImage.sprite = medalSprites[1];
            medalImage.color = Color.white;
        }
        else if (score >= 100)
        {
            medalImage.sprite = medalSprites[0];
            medalImage.color = Color.white;
        }
        else
        {
            medalImage.color = Color.clear;
        }

        this.gameObject.SetActive(true);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
