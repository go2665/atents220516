using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreBoard : MonoBehaviour
{
    // HighScoreLine 5개 변수로 다 가져오기

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Open(/*몇번째 등수에 들어왔는지*/)
    {
        // 게임메니저에서 하이스코어 정보 가져오기(이름도 같이)
        // HighScoreLine 채워넣기
    }

    // 순위에 들었을 때 이름 입력처리(InputField 사용)


}
