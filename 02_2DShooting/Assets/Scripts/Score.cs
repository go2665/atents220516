using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Text scoreText = null;
    float currentScore = 0.0f;      // 보여질 스코어

    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    private void Start()
    {
        GameManager.Inst.onScoreChange = Refresh;   // onScoreChange 이전에 들어간 함수는 제거하고 Refresh로 대체
        scoreText.text = $"Score : {(int)currentScore,4}";
    }

    private void Update()
    {
        if(currentScore < GameManager.Inst.Score)   // 게임 메니저가 가지고 있는 실제 스코어보다 currentScore가 작으면 계속 증가
        {
            currentScore += (Time.deltaTime * 20.0f); // 1초에 최대 10까지 증가
            scoreText.text = $"Score : {(int)currentScore, 4}";
        }
    }

    /// <summary>
    /// 텍스트 갱신용 함수. 우리가 만든 것.
    /// </summary>
    void Refresh()
    {
        //scoreText.text = $"Score : {score,4}";    // 무조건 score를 4자리로 표현
        //scoreText.text = $"Score : {score:d4}";   // score를 4자리로 표현하는데 빈칸은 0으로 채움
        scoreText.text = $"Score : {(int)currentScore, 4}";
    }
}
