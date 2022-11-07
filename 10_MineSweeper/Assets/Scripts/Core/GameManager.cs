using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Initialize()
    {
    }

    /// <summary>
    /// 실행되면 게임 재시작
    /// </summary>
    public void GameReset()
    {
        // 씬을 다시 부를 필요는 없음
    }
}
