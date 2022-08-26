using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    const string SceneNameBase = "Seamless";    // 씬의 기본 이름

    private const int Height = 3;   // 이 게임의 씬 갯수(세로폭)
    private const int Width = 3;    // 이 게임의 씬 갯수(가로폭)

    string[,] sceneNames;   // 각 씬의 이름

    Player player;          // 플레이어

    enum SceneLoadState : byte
    {
        Unload = 0,     // 로딩에 해제되었음
        PendingUnload,  // 로딩 해제 중
        PendingLoad,    // 로딩 중
        Loaded          // 로딩 완료
    }
    SceneLoadState[,] sceneLoadState;    // 각 씬의 로딩 상태
    
    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Initialize()
    {
        // 씬 이름과 상태를 저장하기 위한 배열 생성
        sceneNames = new string[Height, Width];
        sceneLoadState = new SceneLoadState[Height, Width];

        // 각 씬의 이름과 상태 초기 설정
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                string temp = $"{SceneNameBase}_{x}_{y}";   // x가 가로(왼->오른쪽), y가 세로(아래->위)
                sceneNames[y, x] = temp;
                sceneLoadState[y, x] = SceneLoadState.Unload;   // 기본 로딩 상태는 unload
            }
        }

        player = GameManager.Inst.Player;       // 게임메니저에서 플레이어 가져오기
        player.onMapChange += RefreshScenes;    // 플레이어의 위치가 변경되어 맵이 변경되면 실행할 함수 등록
        RequestAsyncSceneLoad(player.CurrentMap.x, player.CurrentMap.y);    // 현재 플레이어의 위치에 해당하는 맵 로딩 요청
        RefreshScenes(player.CurrentMap);       // 플레이어 주변맵 로딩 요청(여기서는 해제할 맵은 없음)
    }

    /// <summary>
    /// 플레이어 주변맵은 로딩하고 그 외의 맵은 해제
    /// </summary>
    /// <param name="current">플레이어가 있는 맵의 위치</param>
    void RefreshScenes(Vector2Int current)
    {
        // 이웃을 구해서 맵 로딩 요청
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (x != 0 || y != 0)   // 플레이어가 있는 맵의 중복로딩 방지
                {
                    RequestAsyncSceneLoad(player.CurrentMap.x + x, player.CurrentMap.y + y);
                }
            }
        }

        // 이제 이웃이 아닌 곳은 맵 로딩 해제 요청
        // 기본적인 컨셉은 로딩을 해제할 맵의 위치는 현재 플레이어 위치에서 두칸씩 떨어진 맵일 수 밖에 없다.
        // 따라서 플레이어 위치의 -2 ~ +2 범위만 찾아서 해제 요청을 한다.
        for (int y = -2; y < 3; y++)
        {
            for (int x = -2; x < 3; x++)
            {
                if( x == 2 || x == -2 || y == 2 || y == -2)
                {
                    RequestAsyncSceneUnload(player.CurrentMap.x + x, player.CurrentMap.y + y);
                }
            }
        }
    }

    /// <summary>
    /// 씬 로딩 요청
    /// </summary>
    /// <param name="x">로딩할 씬의 x 위치</param>
    /// <param name="y">로딩할 씬의 y 위치</param>
    void RequestAsyncSceneLoad(int x, int y)
    {
        if (IsValidePosition(x, y)) // x,y가 적절한 값인지 확인
        {
            if (sceneLoadState[y, x] == SceneLoadState.Unload)  // 언로드 상태일 때만 로딩 요청
            {
                AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[y, x], LoadSceneMode.Additive);
                async.completed += (_) => sceneLoadState[y, x] = SceneLoadState.Loaded; // 로드가 끝나면 Loaded로 상태 변경
                sceneLoadState[y, x] = SceneLoadState.PendingLoad;  // 로드 시작했으니까 pending 상태로 변경
            }
        }
    }

    /// <summary>
    /// 씬 로딩 해제 요청
    /// </summary>
    /// <param name="x">로딩 해제할 씬의 x 위치</param>
    /// <param name="y">로딩 해제할 씬의 y 위치</param>
    void RequestAsyncSceneUnload(int x, int y)
    {
        if (IsValidePosition(x, y)) // x,y가 적절한 값인지 확인
        {
            if (sceneLoadState[y, x] == SceneLoadState.Loaded)  // Loaded 상태일 때만 로딩 요청
            {
                AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[y, x]);
                async.completed += (_) => sceneLoadState[y, x] = SceneLoadState.Unload; // 로딩해제가 끝나면 Unload로 상태 변경
                sceneLoadState[y, x] = SceneLoadState.PendingUnload;    // 로드가 해제되기 시작했으니까 pending 상태로 변경
            }
        }
    }

    /// <summary>
    /// x,y가 범위 안인지 확인
    /// </summary>
    /// <param name="x">확인할 좌표 x</param>
    /// <param name="y">확인할 좌표 y</param>
    /// <returns></returns>
    bool IsValidePosition(int x, int y)
    {
        return (x > -1 && x < Width && y > -1 && y < Height);   // 0보다 크거나 같고 가로 또는 세로 폭보다 작다
    }
}
