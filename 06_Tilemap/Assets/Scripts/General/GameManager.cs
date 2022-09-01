using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // 심리스용 -----------------------------------------------------------------------------------------------------
    MapManager mapManager;      // 심리스용으로 서브씬들 로드/언로드 처리
    public MapManager MapManager => mapManager;
    //--------------------------------------------------------------------------------------------------------------

    Player player;
    public Player Player => player;     // 플레이어

    Volume postProcessVolume;
    public Volume PostProcessVolume => postProcessVolume;   // 블룸, 비네트가 있는 포스트 프로세스 볼륨

    const int Seamless_Base_Index = 1;  // 메인 맵의 인덱스

    // 싱글톤 용 코드 ---------------------------------------------------------------------------------------------
    static GameManager instance = null;
    public static GameManager Inst => instance;

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;  // 씬이 로딩되면 OnSceneLoaded 함수를 실행시켜라.(SceneManager가 가지고 있는 델리게이트에 함수 추가)
        }
        else
        {
            if( instance != this )
            {
                Destroy(this.gameObject);
            }
        }
    }
    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// 씬이 로딩 완료됬을 때 실행되는 함수
    /// </summary>
    /// <param name="sceneData">로딩 완료된 씬의 데이터</param>
    /// <param name="mode">씬을 로딩한 방법</param>
    private void OnSceneLoaded(Scene sceneData, LoadSceneMode mode)
    {
        // Seamless_Base씬이 로딩 되었을 때만 초기화 함수 실행
        if (sceneData.buildIndex == Seamless_Base_Index)
        {
            Initialize();
        }
    }

    /// <summary>
    /// Seamless_Base이 로딩되었을 때만 실행될 것으로 가정하고 만든 함수
    /// </summary>
    public void Initialize()
    {
        postProcessVolume = FindObjectOfType<Volume>(); // 후처리용 볼륨찾기
        player = FindObjectOfType<Player>();            // mapManager의 초기화보다 앞에 있어야 한다.

        if (mapManager == null)     // 맵매니저 없으면 찾는다.
        {
            mapManager = GetComponent<MapManager>();
        }
        mapManager.Initialize();    // 맵매니저 초기화
    }
}
