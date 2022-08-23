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
    public Tilemap background;  // 타일맵들의 전체 크기를 확인할 타일맵(무조건 background가 가장 큰 타일맵이라는 전제)
    public Tilemap obstacle;    // 장애물이 배치되어있는 타일맵
    public LineRenderer line;   // 경로 표시용 라인 랜더러
    GridMap map;                // A* 알고리즘에서 사용하기 위한 그리드 맵

    public GridMap Map => map;
    //--------------------------------------------------------------------------------------------------------------

    // 심리스용 -----------------------------------------------------------------------------------------------------
    MapManager mapManager;
    public MapManager MapManager => mapManager;
    //--------------------------------------------------------------------------------------------------------------


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
        Initialize();
    }   

    private void Initialize()
    {
        //map = new GridMap(background, obstacle);    // 그리드 맵 생성

        if(mapManager == null)
        {
            mapManager = GetComponent<MapManager>();
        }

        mapManager.Initialize();
    }    

    /// <summary>
    /// 월드좌표를 그리드맵의 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="postion">변환할 월드좌표</param>
    /// <returns>변환된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 postion)
    {
        return (Vector2Int)background.WorldToCell(postion);
    }

    /// <summary>
    /// 그리드맵의 그리드 좌표를 월드좌표로 변경해주는 함수
    /// </summary>
    /// <param name="gridPos">그리드맵에서의 위치</param>
    /// <returns>변환된 월드좌표</returns>
    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        return background.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f);   // 0.5는 그리드의 가운데 위치
    }

    /// <summary>
    /// 길찾기 경로 표시용 함수
    /// </summary>
    /// <param name="path">길찾기로 찾은 경로</param>
    public void DrawPath(List<Vector2Int> path)
    {
        if (path != null && path.Count > 1) // 최소 2개의 위치는 필요함
        {
            line.gameObject.SetActive(true);
            line.positionCount = path.Count;
            int index = 0;
            foreach (var pos in path)
            {
                line.SetPosition(index, new(pos.x + 0.5f, pos.y + 0.5f, 1));
                index++;
            }
        }
        else
        {
            line.gameObject.SetActive(false);
        }
    }
}
