using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤으로 구성된 게임 관리용 클래스
/// </summary>
public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 입력 처리용 컴포넌트
    /// </summary>
    InputController input;

    /// <summary>
    /// 인풋 컨트롤러에 접근하기 위한 프로퍼티
    /// </summary>
    public InputController Input { get => input; }

    private UserPlayer userPlayer;
    private EnemyPlayer enemyPlayer;

    public UserPlayer UserPlayer => userPlayer;
    public EnemyPlayer EnemyPlayer => enemyPlayer;

    protected override void Awake()
    {
        base.Awake();

        input = GetComponent<InputController>();    // 인풋 컨트롤러 찾기
    }

    protected override void Initialize()
    {
        userPlayer = FindObjectOfType<UserPlayer>();
        enemyPlayer = FindObjectOfType<EnemyPlayer>();
    }

}
