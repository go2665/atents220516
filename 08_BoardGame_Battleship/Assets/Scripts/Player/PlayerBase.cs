using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        
    /// <summary>
    /// 자동 함선 배치 함수
    /// 가능한 배끼리 붙지 않고 벽부분에 배가 배치되지 않도록 설정
    /// </summary>
    public void AutoShipDeployment()
    {
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

        // lowPriority도 섞기
        temp = lowPriority.ToArray();
        for (int i = temp.Length - 1; i > -1; i--) // 오른쪽 기준으로 셔플
        {
            int randIndex = UnityEngine.Random.Range(0, i);
            (temp[randIndex], temp[i]) = (temp[i], temp[randIndex]);
        }
        lowPriority = new List<int>(temp);

        // 함선마다 배치
        foreach (var ship in ships)
        {
            if (ship.IsDeployed)
                continue;

            // 함선 회전 시키기
            int rotateCount = UnityEngine.Random.Range(0, ShipManager.Inst.ShipDirectionCount);
            bool isCCW = (UnityEngine.Random.Range(0, 10) % 2) == 0;
            for (int i = 0; i < rotateCount; i++)
            {
                ship.Rotate(isCCW);
            }

            // 위치 가져오기
            Vector2Int pos;
            Vector2Int[] shipPositions;
            bool failDeployment = true;
            int counter = 0;
            // high 쪽 탐색
            do
            {
                int headIndex = highPriority[0];
                highPriority.RemoveAt(0);
                pos = new Vector2Int(headIndex % Board.BoardSize, headIndex / Board.BoardSize);

                failDeployment = !board.IsShipDeployment(ship, pos, out shipPositions);
                if (failDeployment)
                {
                    // 배치 실패. 원래 리스트에 되돌리기
                    highPriority.Add(headIndex);
                }
                else
                {
                    // 머리는 배치 성공. 머리를 제외한 나머지 부분이 전부 high에 있는지 확인
                    for (int i = 1; i < shipPositions.Length; i++)
                    {
                        int bodyIndex = shipPositions[i].x + shipPositions[i].y * Board.BoardSize;
                        if (!highPriority.Exists((x) => x == bodyIndex))
                        {
                            highPriority.Add(headIndex);
                            failDeployment = true;
                            break;
                        }
                    }
                }
                counter++;
            } while (failDeployment && counter < 5 && highPriority.Count > 0);
            //Debug.Log($"Counter : {counter}");

            // low쪽 탐색
            while (failDeployment)
            {
                //Debug.Log("Low 확인 중");
                int headIndex = lowPriority[0];
                lowPriority.RemoveAt(0);
                pos = new Vector2Int(headIndex % Board.BoardSize, headIndex / Board.BoardSize);

                failDeployment = !board.IsShipDeployment(ship, pos, out shipPositions);
                if (failDeployment)
                {
                    // 배치 실패. 원래 리스트에 되돌리기
                    lowPriority.Add(headIndex);
                }
            }

            // 위치는 골라졌다.
            board.ShipDeployment(ship, pos);        // 함선 배치

            // 함선이 차지하는 영역은 모든 리스트에서 제거
            List<int> tempList = new List<int>(shipPositions.Length);
            foreach (var tempPos in shipPositions)   // 배치할 위치(그리드 좌표)를 index로 변환
            {
                tempList.Add(tempPos.x + tempPos.y * Board.BoardSize);
            }
            foreach (var tempIndex in tempList)
            {
                highPriority.Remove(tempIndex);     // 전체 목록에서 제거
                lowPriority.Remove(tempIndex);
            }

            // 함선 주변 지역을 high에서 low로 이동
            List<int> toLowList = new List<int>(ship.Size * 2 + 6);
            // 함선 주변 위치를 구하는 방법은?

            // 가로 방향인지 세로 방향인지 확인한다.
            // 가로 방향이면 배의 각 칸별 위치와 배의 머리 앞칸과 꼬리 뒤칸에서 y를 +-1씩 한 위치들을 추가한다. 
            // 세로 방향이면 배의 각 칸별 위치와 배의 머리 앞칸과 꼬리 뒤칸에서 x를 +-1씩 한 위치들을 추가한다. 
            if( ship.Direction == ShipDirection.NORTH || ship.Direction == ShipDirection.SOUTH)
            {
                // 배가 세로 방향
                foreach (var tempPos in shipPositions)
                {
                    toLowList.Add((tempPos.x + 1) + tempPos.y * Board.BoardSize);
                    toLowList.Add((tempPos.x - 1) + tempPos.y * Board.BoardSize);
                }

                Vector2Int headFrontPos;
                Vector2Int tailRearPos;
                if ( ship.Direction == ShipDirection.NORTH )
                {
                    // 머리 위치 -y
                    headFrontPos = shipPositions[0] + Vector2Int.down;
                    tailRearPos = shipPositions[^1] + Vector2Int.up;                    
                }
                else
                {
                    // 머리 위치 +y
                    headFrontPos = shipPositions[0] + Vector2Int.up;
                    tailRearPos = shipPositions[^1] + Vector2Int.down;
                }
                toLowList.Add(headFrontPos.x + headFrontPos.y * Board.BoardSize);
                toLowList.Add((headFrontPos.x + 1) + headFrontPos.y * Board.BoardSize);
                toLowList.Add((headFrontPos.x - 1) + headFrontPos.y * Board.BoardSize);
                toLowList.Add(tailRearPos.x + tailRearPos.y * Board.BoardSize);
                toLowList.Add((tailRearPos.x + 1) + tailRearPos.y * Board.BoardSize);
                toLowList.Add((tailRearPos.x - 1) + tailRearPos.y * Board.BoardSize);
            }
            else
            {
                // 배가 가로 방향
                foreach (var tempPos in shipPositions)
                {
                    toLowList.Add(tempPos.x + (tempPos.y + 1) * Board.BoardSize);
                    toLowList.Add(tempPos.x + (tempPos.y - 1) * Board.BoardSize);
                }

                Vector2Int headFrontPos;
                Vector2Int tailRearPos;
                if (ship.Direction == ShipDirection.EAST)
                {
                    // 머리 위치 +x
                    headFrontPos = shipPositions[0] + Vector2Int.right;
                    tailRearPos = shipPositions[^1] + Vector2Int.left;
                }
                else
                {
                    // 머리 위치 -x
                    headFrontPos = shipPositions[0] + Vector2Int.left;
                    tailRearPos = shipPositions[^1] + Vector2Int.right;
                }
                toLowList.Add(headFrontPos.x + headFrontPos.y * Board.BoardSize);
                toLowList.Add(headFrontPos.x + (headFrontPos.y + 1) * Board.BoardSize);
                toLowList.Add(headFrontPos.x + (headFrontPos.y - 1) * Board.BoardSize);
                toLowList.Add(tailRearPos.x + tailRearPos.y * Board.BoardSize);
                toLowList.Add(tailRearPos.x + (tailRearPos.y + 1) * Board.BoardSize);
                toLowList.Add(tailRearPos.x + (tailRearPos.y - 1) * Board.BoardSize);
            }

            foreach(var index in toLowList)
            {
                if( highPriority.Exists((x) => x == index) )
                {
                    highPriority.Remove(index);
                    lowPriority.Add(index);
                }
            }
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
