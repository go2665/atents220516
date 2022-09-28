using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // 게임 판의 크기(10*10)
    const int BoardSize = 10;

    // 배의 배치 정보를 저장한 배열
    ShipType[] shipInfo;    // 2차원 배열이지만 1차원 배열로 표현

    // 공격 당한 위치들을 표시하는 배열
    bool[] bombInfo;        


    // 배를 배치하기
    // 공격을 당하기

    // 그리드 변환( 월드좌표 <-> 그리드 좌표)


    void Test()
    {
        shipInfo = new ShipType[BoardSize * BoardSize];
        for(int i=0;i<BoardSize;i++)
        {
            for(int j=0;j<BoardSize;j++)
            {
                ShipType ship = shipInfo[i*BoardSize+j];
            }
        }
    }
}
