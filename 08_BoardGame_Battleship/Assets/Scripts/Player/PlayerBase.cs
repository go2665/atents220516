using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 기본 클래스
/// </summary>
public class PlayerBase : MonoBehaviour
{
    /// <summary>
    /// 이 플레이어의 게임판( 플레이어의 자식으로 넣을지 고민중 )
    /// </summary>
    public Board board;

    /// <summary>
    /// 플레이어의 상태
    /// </summary>
    protected PlayerState state = PlayerState.Title;

    /// <summary>
    /// 플레이어가 가지고 있는 배(Start할 때 자동 생성)
    /// </summary>
    protected Ship[] ships;

    protected int hp;

    protected virtual void Start()
    {
        // 배 종류별로 하나씩 생성
        int shipTypeCount = ShipManager.Inst.ShipTypeCount;
        ships = new Ship[shipTypeCount];
        for ( int i=0;i< shipTypeCount; i++)
        {
            ships[i] = ShipManager.Inst.MakeShip((ShipType)(i + 1), this);
            ships[i].onDead += OnShipDestroy;
            board.onShipAttacked[(ShipType)(i + 1)] = ships[i].OnAttacked;  // 배 종류별로 공격 당할 때 실행될 함수 연결
        }
        board.onShipAttacked[ShipType.None] = null; // 키값 추가용.

        hp = shipTypeCount;
    }

    private void OnShipDestroy(ShipType type)
    {
        hp--;
        if( hp <= 0 )
        {
            OnDefeat();
        }
    }

    private void OnDefeat()
    {

    }

    public void AutoShipDeployment_v1()
    {
        foreach (var ship in ships)
        {
            if (ship.IsDeployed)
                continue;

            int rotateCount = UnityEngine.Random.Range(0, ShipManager.Inst.ShipDirectionCount);
            bool isCCW = (UnityEngine.Random.Range(0, 10) % 2) == 0;
            for (int i=0;i< rotateCount;i++)
            {
                ship.Rotate(isCCW);
            }

            bool result;
            Vector2Int randPos;
            do
            {
                randPos = board.RandomPosition();
                result = board.IsShipDeployment(ship, randPos);
            } while (!result);

            board.ShipDeployment(ship, randPos);
        }        
    }

        

    public void AutoShipDeployment()
    {
        // 1. 배들끼리 붙으면 안된다.
        // 2. 벽에 배의 옆면이 붙으면 안된다.
        int capacity = Board.BoardSize * Board.BoardSize;
        List<int> highPriority = new(capacity);
        List<int> lowPriority = new(capacity);

        // 가장자리 부분을 낮은 우선 순위에 배치
        for (int i = 0; i < capacity; i++)
        {
            if ( i % 10 == 0 
                || i % 10 == (Board.BoardSize - 1)
                || (0 < i && i < (Board.BoardSize - 1))
                || ((Board.BoardSize - 1) * Board.BoardSize < i && i < (Board.BoardSize * Board.BoardSize - 1)))
            {
                // Board.BoardSize가 10일때
                // 0~9
                // 10,20,30,40,50,60,70,80,90
                // 19,29,39,49,59,69,79,89,99
                // 90~99
                lowPriority.Add(i);
            }
            else
            {
                highPriority.Add(i);
            }
        }

        // highPriority를 섞기(겹치는 숫자없이 순서만 섞여야 한다.)
        int[] temp = highPriority.ToArray();

        // temp 랜덤한 위치에 있는 숫자 하나를 선택한다.
        // 선택한 숫자와 제일 마지막에 있는 숫자와 교환한다.
        // 교환한 마지막 부분을 제외한 나머지 부분에서 랜덤하게 선택한다.
        // 선택한 숫자와 제일 마지막에서 두번째 숫자와 교환한다.
        // 계속 반복



        foreach (var ship in ships)
        {
            if (ship.IsDeployed)
                continue;


        }
    }

    public void UndoAllShipDeployment()
    {        
        foreach(var ship in ships)
        {
            board.UndoShipDeployment(ship);
        }
    }

    // 테스트 용도(플레이어의 상태 설정)
    public void Test_SetState(PlayerState state)
    {
        this.state = state;
    }
}
