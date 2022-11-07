using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{    
    /// <summary>
    /// 셀 이미지 매니저 접근용 변수
    /// </summary>
    CellImageManager cellImage;

    // 프로퍼티

    /// <summary>
    /// 셀 이미지 매니저에 접근하기 위한 프로퍼티
    /// </summary>
    public CellImageManager CellImage => cellImage;


    protected override void Initialize()
    {
        // 컴포넌트 찾기
        cellImage = GetComponent<CellImageManager>();
    }

    /// <summary>
    /// 실행되면 게임 재시작
    /// </summary>
    public void GameReset()
    {
        // 씬을 다시 부를 필요는 없음
    }

    /// <summary>
    /// 게임 오버 처리 함수
    /// </summary>
    public void GameOver()
    {
        // 찾은 지뢰는 전부 열고 찾았다는 표시로 바꿔주기
        // 못찾은 지뢰도 전부 열어서 확인 시켜주기
    }
}
