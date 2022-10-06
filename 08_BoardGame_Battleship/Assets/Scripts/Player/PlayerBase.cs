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

        // highPriority를 섞기(피셔 예이츠 알고리즘 사용)
        int[] temp = highPriority.ToArray();
        for(int i = temp.Length - 1; i>-1; i--) // 오른쪽 기준으로 셔플
        {
            int randIndex = UnityEngine.Random.Range(0, i);
            (temp[randIndex], temp[i]) = (temp[i], temp[randIndex]);
        }
        //for (int i = 0; i < temp.Length - 1; i++) // 왼쪽 기준으로 셔플
        //{
        //    int index = Random.Range(i + 1, temp.Length);
        //    (temp[i], temp[index]) = (temp[index], temp[i]);
        //}
        highPriority = new List<int>(temp);        



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
