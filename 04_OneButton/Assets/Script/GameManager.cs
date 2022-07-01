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

    const int rankCount = 5;
    public const int INVALID_RANK = -1;

    int[] highScore = new int[rankCount];       // 0번째가 가장 높음, 4번째가 강 낮음
    string[] highName = new string[rankCount];  // highScore 기록을 낸 사람 이름

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
    public int BestScore
    {
        get => highScore[0];
    }
    public int[] HighScore
    {
        get => highScore;
    }
    public string[] HighName
    {
        get => highName;
    }

    //TextMeshProUGUI scoreText;
    ImageNumber imageNumber;
    ScoreBoard scoreBoard;
    HighScoreBoard highScoreBoard;

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
        highScoreBoard = FindObjectOfType<HighScoreBoard>();

        LoadGameData();
    }

    public void SaveGameData()
    {
        SaveData saveData = new();          // json 저장용 클래스 인스턴스화
        saveData.highScore = highScore;     // json 저장용 클래스에 값을 넣기
        saveData.highName = highName;

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

        if (Directory.Exists(path) && File.Exists(fullPath))   // 경로와 파일 둘 다 존재하는지 확인
        {
            string json = File.ReadAllText(fullPath);           // 실제로 파일에 써있는 문자열 읽기
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);   // 특정 클래스(SaveData) 규격에 맞게 파싱하기
            highScore = saveData.highScore;                     // json 데이터를 불러온 클래스에서 원하는 값 가져오기
            highName = saveData.highName;
            //Debug.Log($"High Score : {saveData.highScore}");
        }
    }

    public void OnGameOver()
    {
        bool isBestScore = false;
        int rank = INVALID_RANK;            // 초기화
        for (int i=0; i< rankCount; i++)    // 순위 개수만큼 확인
        {
            if( highScore[i] < score )      // highScore에 저장된 값과 score를 비교해서 score가 더 크면 그 순위에 끼워넣기
            {
                isBestScore = (i == 0);     // 0번째보다 크면 최고 점수
                for (int j = rankCount-1; j>i; j--)   // 맨 아래쪽부터 한 칸씩 아래로 내리기
                {
                    highScore[j] = highScore[j-1];
                    highName[j] = highName[j - 1];
                }
                highScore[i] = score;       // 마지막으로 score에 넣기
                rank = i;                   // 랭크 설정                
                break;
            }
        }
        scoreBoard.Open(isBestScore);
        highScoreBoard.Open(rank);
    }

    /// <summary>
    /// HighName을 변경하고 파일에 저장. 함부로 호출하지 말것
    /// </summary>
    /// <param name="rank">이름을 변경할 랭크</param>
    /// <param name="newName">새 이름</param>
    public void SetHighName(int rank, string newName)
    {
        highName[rank] = newName;
        SaveGameData();                // 변경하고 나면 저장
    }
}
