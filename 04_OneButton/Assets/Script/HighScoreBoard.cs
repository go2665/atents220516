using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreBoard : MonoBehaviour
{
    // HighScoreLine 5개 변수로 다 가져오기
    const int NumberOfHighScore = 5;
    HighScoreLine[] highScoreLines;
    TMP_InputField inputName;
    int inputRank = GameManager.INVALID_RANK;

    private void Awake()
    {
        highScoreLines = new HighScoreLine[NumberOfHighScore];
        for (int i=0;i < NumberOfHighScore; i++)
        {
            highScoreLines[i] = transform.GetChild(i).GetComponent<HighScoreLine>();
        }
        inputName = transform.GetChild(transform.childCount - 1).GetComponent<TMP_InputField>();
        inputName.onEndEdit.AddListener(OnInputNameEnd);    // 입력이 끝날 때 실행되는 델리게이트에 OnInputNameEnd를 연결
        inputName.gameObject.SetActive(false);
    }

    private void OnInputNameEnd(string input)
    {
        highScoreLines[inputRank].SetHighName(input);   // inputRank번째의 이름을 변경한다.
        inputName.gameObject.SetActive(false);          // 입력이 끝났으면 보이지 않게 만든다.

        GameManager.Inst.SetHighName(inputRank, input); // 이름 변경하고 저장      
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Open(int rank)
    {
        this.gameObject.SetActive(true);

        // HighScoreLine 채워넣기
        for (int i=0; i< NumberOfHighScore; i++)
        {
            highScoreLines[i].SetHighScore(GameManager.Inst.HighScore[i]);
            highScoreLines[i].SetHighName(GameManager.Inst.HighName[i]);
        }
                
        if( rank != GameManager.INVALID_RANK) 
        {
            // 순위가 변경되었다.
            RectTransform rect = (RectTransform)inputName.transform;    // anchoredPosition을 받아오기 위해 캐스팅
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -25 + (-125 * rank));  // x는 고정, y는 랭크값에 맞춰서 계산
            inputRank = rank;                       // 어떤 랭크에 추가되었는지 저장한 후 OnInputNameEnd에서 사용
            inputName.gameObject.SetActive(true);   // 인풋필드를 보여준다.            
        }

    }

    


}
