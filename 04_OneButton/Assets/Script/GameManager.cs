using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    public int point = 10;  // 한 칸 넘을 때마다 얻는 점수

    int highScore = 0;

    float currentScore = 0.0f;
    int score = 0;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            //Debug.Log($"Score : {score}");
            //scoreText.text = score.ToString();
        }
    }
    public int HighScore
    {
        get => highScore;
    }

    //TextMeshProUGUI scoreText;
    ImageNumber imageNumber;
    ScoreBoard scoreBoard;

    // static 맴버 변수 : 주소가 고정이다. => 이 클래스의 모든 인스턴스는 같은 값을 가진다.
    static GameManager instance = null;
    public static GameManager Inst
    {
        get => instance;
    }

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            if( instance != this )
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Initialize();
    }

    private void Update()
    {
        if( currentScore < Score )
        {
            currentScore += (Time.deltaTime * 20.0f);
            //scoreText.text = ((int)currentScore).ToString();
            imageNumber.Number = (int)currentScore;
        }
    }

    private void Initialize()
    {
        //scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        //scoreText.text = "0";
        Score = 0;
        currentScore = 0;

        imageNumber = GameObject.Find("MainScore_ImageNumber").GetComponent<ImageNumber>();
        scoreBoard = FindObjectOfType<ScoreBoard>();

        LoadGameData();
    }

    public void SaveGameData()
    {
        SaveData saveData = new();          // json 저장용 클래스 인스턴스화
        saveData.highScore = highScore;     // json 저장용 클래스에 값을 넣기

        string json = JsonUtility.ToJson(saveData);     // json 저장용 클래스의 내용을 json포맷의 문자열로 변경

        string path = $"{Application.dataPath}/Save/";  // 저장할 폴더의 경로 구하기
        if( !Directory.Exists(path) )           // 경로가 존재하는지 확인
        {
            // path 폴더가 없다.
            Directory.CreateDirectory(path);    // 경로에 해당하는 폴더가 없으면 만들기
        }

        string fullPath = $"{path}Save.json";   // 경로명과 파일명을 합쳐서 전체경로(fullPath) 만들기
        File.WriteAllText(fullPath, json);      // 전체경로대로 파일을 만들어서 json 문자열 내용을 쓰기
    }

    public void LoadGameData()
    {
        string path = $"{Application.dataPath}/Save/";          // 저장된 폴더 이름 만들기
        string fullPath = $"{path}Save.json";                   // 전체경로 만들기

        if( Directory.Exists(path) && File.Exists(fullPath) )   // 경로와 파일 둘 다 존재하는지 확인
        {
            string json = File.ReadAllText(fullPath);           // 실제로 파일에 써있는 문자열 읽기
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);   // 특정 클래스(SaveData) 규격에 맞게 파싱하기
            highScore = saveData.highScore;                     // json 데이터를 불러온 클래스에서 원하는 값 가져오기
            //Debug.Log($"High Score : {saveData.highScore}");
        }
    }

    public void OnGameOver()
    {
        bool isHighScore = score > highScore;
        if ( isHighScore )
        {
            highScore = score;
            SaveGameData();            
        }
        scoreBoard.Open(isHighScore);
    }
}
