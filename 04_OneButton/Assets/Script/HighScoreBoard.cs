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

    private void Awake()
    {
        highScoreLines = new HighScoreLine[NumberOfHighScore];
        for (int i=0;i < NumberOfHighScore; i++)
        {
            highScoreLines[i] = transform.GetChild(i).GetComponent<HighScoreLine>();
        }
        inputName = transform.GetChild(transform.childCount - 1).GetComponent<TMP_InputField>();
        inputName.onEndEdit.AddListener(OnInputNameEnd);
        inputName.gameObject.SetActive(false);
    }

    private void OnInputNameEnd(string input)
    {
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
            inputName.gameObject.SetActive(true);
            //highScoreLines[rank].SetHighName();
        }

    }

    


}
