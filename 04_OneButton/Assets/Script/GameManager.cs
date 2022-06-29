using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

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

    //TextMeshProUGUI scoreText;
    ImageNumber imageNumber;

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
        }
        else
        {
            if( instance != this )
            {
                Destroy(this.gameObject);
            }
        }
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

        imageNumber = FindObjectOfType<ImageNumber>();
        //imageNumber.Number = 0;

        //SaveGameData();
        LoadGameData();
    }

    public void SaveGameData()
    {
        SaveData saveData = new();
        saveData.highScore = highScore;

        string json = JsonUtility.ToJson(saveData);

        string path = $"{Application.dataPath}/Save/";
        if( !Directory.Exists(path) )
        {
            // path 폴더가 없다.
            Directory.CreateDirectory(path);
        }

        string fullPath = $"{path}Save.json";
        File.WriteAllText(fullPath, json);
    }

    public void LoadGameData()
    {
        string path = $"{Application.dataPath}/Save/";
        string fullPath = $"{path}Save.json";

        if( Directory.Exists(path) && File.Exists(fullPath) )
        {
            string json = File.ReadAllText(fullPath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            highScore = saveData.highScore;
            Debug.Log($"High Score : {saveData.highScore}");
        }
    }
}
