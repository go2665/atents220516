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
    public Player Player => player;

    Volume postProcessVolume;
    public Volume PostProcessVolume => postProcessVolume;


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
        postProcessVolume = FindObjectOfType<Volume>();
        player = FindObjectOfType<Player>();    // mapManager의 초기화보다 앞에 있어야 한다.

        if (mapManager == null)     // 없으면 찾는다.
        {
            mapManager = GetComponent<MapManager>();
        }
        mapManager.Initialize();
    }
}
