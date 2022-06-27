using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public int sceneID = 0;
    CanvasFadeInOut fadeInOut;  // Fade In/Out효과가 있는 캔버스

    private void Start()
    {
        // 하나밖에 없기 때문에 타입으로 정확히 찾을 수 있음
        fadeInOut = FindObjectOfType<CanvasFadeInOut>();
        // 애니메이션이 끝날 때 실행될 델리게이트에 함수 등록
        fadeInOut.OnFadeOutEnd = SceneLoad;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거 안에 들어가면 캔버스에 있는 애니메이션 실행
        fadeInOut.StartFadeOut();
        GameManger.Inst.ShowClearTime();
    }

    /// <summary>
    /// sceneID씬으로 씬을 변경하는 함수
    /// </summary>
    void SceneLoad()
    {
        SceneManager.LoadScene(sceneID);
    }
}
