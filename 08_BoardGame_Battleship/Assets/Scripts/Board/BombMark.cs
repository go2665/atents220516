using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMark : MonoBehaviour
{
    /// <summary>
    /// 공격이 명중되었을 때 보일 마크. O
    /// </summary>
    public GameObject successMark;

    /// <summary>
    /// 공격이 빗나갔을 때 보일 마크. X
    /// </summary>
    public GameObject failMark;

    /// <summary>
    /// 공격된 위치에 표시할 테스트용 구
    /// </summary>
    public GameObject testInfoPrefab;

    /// <summary>
    /// 테스트용 구의 머티리얼
    /// </summary>
    public Material testInfoMaterial;

    /// <summary>
    /// 공격받은 위치에 포탄 명중 여부 표시해주는 함수
    /// </summary>
    /// <param name="position">공격받은 위치</param>
    /// <param name="isSuccess">배에 명중했으면 true, 아니면 false로 입력받음</param>
    public void SetBombMark(Vector3 position, bool isSuccess)
    {
        GameObject markPrefab = isSuccess ? successMark : failMark;     // isSuccess가 true면 O, false면 X 프리팹 선택

        GameObject markInstance = Instantiate(markPrefab, transform);   // 마크 생성
        markInstance.transform.position = position + Vector3.up * 2;    // 마크 위치를 grid위치로 옮기기

#if UNITY_EDITOR
        GameObject obj = Instantiate(testInfoPrefab, transform);        // 에디터에서만 보일 회색 구 생성
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = testInfoMaterial;                           // 회색 머티리얼 적용
        obj.transform.position = position + Vector3.up;                 // 잘보이도록 위치 옮기기
#endif
    }
}
