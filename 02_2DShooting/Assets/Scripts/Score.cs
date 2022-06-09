using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Text scoreText = null;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    private void Start()
    {
        GameManager.Inst.onScoreChange = Refresh;
    }

    void Refresh()
    {
        //scoreText.text = $"Score : {score,4}";    // 무조건 score를 4자리로 표현
        //scoreText.text = $"Score : {score:d4}";   // score를 4자리로 표현하는데 빈칸은 0으로 채움
        scoreText.text = $"Score : {GameManager.Inst.Score, 4}";
    }
}
