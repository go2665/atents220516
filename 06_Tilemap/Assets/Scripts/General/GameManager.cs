using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // A* 용 -------------------------------------------------------------------------------------------------------
    //public Tilemap background;  // 타일맵들의 전체 크기를 확인할 타일맵(무조건 background가 가장 큰 타일맵이라는 전제)
    //public Tilemap obstacle;    // 장애물이 배치되어있는 타일맵
    //public LineRenderer line;   // 경로 표시용 라인 랜더러
    //GridMap map;                // A* 알고리즘에서 사용하기 위한 그리드 맵

    //public GridMap Map => map;
    //--------------------------------------------------------------------------------------------------------------

    // 심리스용 -----------------------------------------------------------------------------------------------------
    MapManager mapManager;
    public MapManager MapManager => mapManager;
    //--------------------------------------------------------------------------------------------------------------

    Player player;
    public Player Player => player;


    static GameManager instance = null;
    public static GameManager Inst => instance;

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
        // 한번에 한씬만 사용할 때는 문제가 없었으나 씬이 Additive로 추가되면서 예상치 못한 상황에서 실행이됨.
        // 그래서 Inittialize가 여러번 호출된다.
        // 그것을 해결하기 위해 한번 실행되면 다시 실행안되도록 델리게이트 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;  // 한번만 실행하기 위해
        Initialize();
    }   

    /// <summary>
    /// 초기화 함수라 무조건 한번만 실행될 것으로 가정하고 만든 함수
    /// </summary>
    public void Initialize()
    {
        //map = new GridMap(background, obstacle);    // 그리드 맵 생성

        player = FindObjectOfType<Player>();    // mapManager의 초기화보다 앞에 있어야 한다.

        if (mapManager == null)     // 없으면 찾는다.
        {
            mapManager = GetComponent<MapManager>();
        }
        mapManager.Initialize();
    }

    // SceneMonsterManager로 옮겨졌음
    ///// <summary>
    ///// 월드좌표를 그리드맵의 그리드 좌표로 변경해주는 함수
    ///// </summary>
    ///// <param name="postion">변환할 월드좌표</param>
    ///// <returns>변환된 그리드 좌표</returns>
    //public Vector2Int WorldToGrid(Vector3 postion)
    //{
    //    return (Vector2Int)background.WorldToCell(postion);
    //}
    ///// <summary>
    ///// 그리드맵의 그리드 좌표를 월드좌표로 변경해주는 함수
    ///// </summary>
    ///// <param name="gridPos">그리드맵에서의 위치</param>
    ///// <returns>변환된 월드좌표</returns>
    //public Vector2 GridToWorld(Vector2Int gridPos)
    //{
    //    return background.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f);   // 0.5는 그리드의 가운데 위치
    //}

    
}
