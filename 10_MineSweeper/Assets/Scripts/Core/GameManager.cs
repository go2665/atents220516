using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{    
    /// <summary>
    /// 게임 상태표시용 enum
    /// </summary>
    public enum GameState
    {
        Ready = 0,      // 시작 전(새로 스테이지 만들어 진 후 첫번째 셀을 오픈하지 않은 상황)
        Play,           // 게임 중(첫째 셀을 오픈 한 이후)
        GameClear,      // 모든 지뢰를 다 찾았을 때
        GameOver        // 지뢰를 밟았을 때
    }

    /// <summary>
    /// 현재 게임 상태
    /// </summary>
    GameState state = GameState.Ready;

    /// <summary>
    /// 셀 이미지 매니저 접근용 변수
    /// </summary>
    CellImageManager cellImage;

    /// <summary>
    /// 리셋 버튼
    /// </summary>
    ResetButton resetButton;

    /// <summary>
    /// 게임 스테이지
    /// </summary>
    Stage stage;

    /// <summary>
    /// 시간 측정용 UI
    /// </summary>
    TimeCounter timeCounter;

    /// <summary>
    /// 랭크 갯수
    /// </summary>
    const int RankCount = 5;

    /// <summary>
    /// 시간 순위
    /// </summary>
    List<int> timeRank;

    /// <summary>
    /// 클릭 순위
    /// </summary>
    List<int> clickRank;

    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// 셀 이미지 매니저에 접근하기 위한 프로퍼티
    /// </summary>
    public CellImageManager CellImage => cellImage;

    /// <summary>
    /// 리셋 버튼에 접근하기 위한 프로퍼티
    /// </summary>
    public ResetButton ResetBtn => resetButton;

    /// <summary>
    /// 플레이 중인지 확인하는 프로퍼티
    /// </summary>
    public bool IsPlaying => state == GameState.Play;

    /// <summary>
    /// 게임 스테이지 확인용 프로퍼티
    /// </summary>
    public Stage Stage => stage;



    // 델리게이트 ----------------------------------------------------------------------------------

    /// <summary>
    /// 게임이 시작될 때 실행될 델리게이트
    /// </summary>
    public Action onGameStart;

    /// <summary>
    /// 게임이 재시작될 때 실행될 델리게이트(리셋버튼 눌렀을 때)
    /// </summary>
    public Action onGameReset;

    /// <summary>
    /// 게임 오버가 되었을 때(지뢰를 밟았을 때) 실행될 델리게이트
    /// </summary>
    public Action onGameOver;

    /// <summary>
    /// 게임 클리어를 했을 때(모든 지뢰를 다 표시하고 남은 모든 셀을 열었을 때) 실행될 델리게이트
    /// </summary>
    public Action onGameClear;

    /// <summary>
    /// 시간 순위가 갱신 되었을 때 실행되는 델리게이트
    /// </summary>
    public Action<List<int>> onTimeRankUpdated;

    /// <summary>
    /// 클릭 순위가 갱신 되었을 때 실행되는 델리게이트
    /// </summary>
    public Action<List<int>> onClickRankUpdated;
    // 함수들 --------------------------------------------------------------------------------------

    /// <summary>
    /// Awake 때 실행되는 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        // 컴포넌트 찾기
        cellImage = GetComponent<CellImageManager>();
        resetButton = FindObjectOfType<ResetButton>();
        stage = FindObjectOfType<Stage>();
        timeCounter = FindObjectOfType<TimeCounter>();

        // 리스트 생성(랭킹은 5명만 보일테니 capacity를 6으로 설정. 임시 인원용)
        timeRank = new List<int>(RankCount+1);
        clickRank = new List<int>(RankCount+1);
    }

    private void Start()
    {
        LoadGameData();     // 저장 데이터 로딩
    }

    /// <summary>
    /// 게임을 시작을 알리는 함수
    /// </summary>
    public void GameStart()
    {
        if(state == GameState.Ready)
        {
            state = GameState.Play;     // 처음 한번만 실행되게 설정
            onGameStart?.Invoke();      // 델리게이트로 알림
        }
    }

    /// <summary>
    /// 게임 재시작을 알리는 함수
    /// </summary>
    public void GameReset()
    {
        // 씬을 다시 부를 필요는 없음
        state = GameState.Ready;
        onGameReset?.Invoke();
    }

    /// <summary>
    /// 게임 오버를 알리는 함수
    /// </summary>
    public void GameOver()
    {
        Debug.Log("게임 오버");
        state = GameState.GameOver;
        onGameOver?.Invoke();
    }

    /// <summary>
    /// 게임 클리어를 알리는 함수
    /// </summary>
    public void GameClear()
    {
        Debug.Log("게임 클리어");
        RankUpdate();       // 랭크 업데이트
        SaveGameData();     // 결과 저장

        state = GameState.GameClear;
        onGameClear?.Invoke();
    }

    /// <summary>
    /// 2개의 랭크(시간순, 클릭순) 업데이트
    /// </summary>
    private void RankUpdate()
    {
        // 현재 값 추가
        clickRank.Add(Stage.OpenTryCount);  
        timeRank.Add(timeCounter.CountTime);

        // 정렬
        clickRank.Sort();
        timeRank.Sort();

        // 마지막(6등) 제거
        clickRank.RemoveAt(RankCount);
        timeRank.RemoveAt(RankCount);
    }

    /// <summary>
    /// 게임 랭킹을 저장하는 함수
    /// </summary>
    void SaveGameData()
    {
        SaveData saveData = new();                      // json으로 저장하기 위한 클래스 생성
        saveData.clickRank = clickRank.ToArray();       // 클래스에 데이터 저장
        saveData.timeRank = timeRank.ToArray();

        string json = JsonUtility.ToJson(saveData);     // json형식으로 변경

        string path = $"{Application.dataPath}/Save/";  // 폴더 있는지 확인
        if( !Directory.Exists(path) )
        {
            Directory.CreateDirectory(path);            // 없으면 만들기
        }

        string fullPath = $"{path}Save.json";           // 전체 경로 구하고
        File.WriteAllText(fullPath, json);              // 파일에 스기

        onTimeRankUpdated?.Invoke(timeRank);            // 랭킹이 업데이트 되었다고 알림
        onClickRankUpdated?.Invoke(clickRank);
    }

    /// <summary>
    /// 저장한 데이터를 불러오는 함수
    /// </summary>
    void LoadGameData()
    {
        string path = $"{Application.dataPath}/Save/";          // 경로 구하고
        string fullPath = $"{path}Save.json";
        if (Directory.Exists(path)&& File.Exists(fullPath))     // 해당 경로에 폴더가 있고 파일이 있으면
        {
            string json = File.ReadAllText(fullPath);               // 파일 내용을 json 형식으로 읽기
            SaveData data = JsonUtility.FromJson<SaveData>(json);   // json에 있는 내용을 클래스 형식으로 변경
            timeRank = new List<int>(data.timeRank);                // 읽은 데이터를 기반으로 변수에 기록
            clickRank = new List<int>(data.clickRank);
        }
        else
        {
            // 폴더나 파일이 없으면
            timeRank = new List<int>(new int[] { 999, 999, 999, 999, 999 });    // 기본 값 설정
            clickRank = new List<int>(new int[] { 256, 256, 256, 256, 256 });
        }

        onTimeRankUpdated?.Invoke(timeRank);                // 랭킹이 업데이트 되었다고 알림
        onClickRankUpdated?.Invoke(clickRank);
    }
}
