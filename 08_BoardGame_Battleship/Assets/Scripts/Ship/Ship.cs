using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // 변수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 배의 타입
    /// </summary>
    ShipType type;

    /// <summary>
    /// 배가 바라보는 방향
    /// </summary>
    ShipDirection direction;

    /// <summary>
    /// 배의 크기
    /// </summary>
    int size = 2;

    /// <summary>
    /// 배의 모델링 게임오브젝트의 트랜스폼
    /// </summary>
    Transform model;    

    // 프로퍼티들 ----------------------------------------------------------------------------------
    /// <summary>
    /// 배의 타입 확인용 프로퍼티
    /// </summary>
    public ShipType Type { get => type; }

    /// <summary>
    /// 배의 방향 확인 및 설정용 프로퍼티
    /// </summary>
    public ShipDirection Direction 
    { 
        get => direction; 
        set
        {
            // 배의 방향을 설정하고 모델의 회전도 즉시 적용
            direction = value;
            model.rotation = Quaternion.Euler(0, (int)direction * 90.0f, 0);
        }            
    }

    /// <summary>
    /// 배의 크기 확인용 프로퍼티
    /// </summary>
    public int Size { get => size; }

    // 함수들 --------------------------------------------------------------------------------------

    /// <summary>
    /// 배를 만들고 각종 데이터를 초기화 하기 위한 함수
    /// </summary>
    /// <param name="newType">이 배의 타입</param>
    public void Initialize(ShipType newType)
    {
        type = newType;
        switch (type)   // 종류별로 크기 지정
        {
            case ShipType.Carrier:
                size = 5;
                break;
            case ShipType.Battleship:
                size = 4;
                break;
            case ShipType.Destroyer:
            case ShipType.Submarine:
                size = 3;
                break;
            case ShipType.PatrolBoat:
                size = 2;
                break;
            case ShipType.None:
            default:
                break;
        }

        // 배의 모델 생성 후 자식으로 추가
        GameObject modelPrefab = ShipManager.Inst.GetShipModel(type);
        GameObject obj = Instantiate(modelPrefab, transform);

        // 모델의 트랜스폼 저장
        model = obj.transform;

        // 배의 초기방향 지정
        Direction = ShipDirection.NORTH;
    }

    /// <summary>
    /// 배를 90도씩 회전 시키는 함수
    /// </summary>
    /// <param name="isCCW">반시계방항인지 여부(true면 반시계방향)</param>
    public void Rotate(bool isCCW)
    {
        int count = ShipManager.Inst.ShipDirectionCount;
        if ( isCCW )
        {
            // 반시계방향으로 회전시키기
            //Debug.Log("반시계방향");
            Direction = (ShipDirection)(((int)direction + count - 1) % count);            
        }
        else
        {
            // 시계방향으로 회전시키기
            //Debug.Log("시계방향");
            Direction = (ShipDirection)(((int)direction + 1) % count);
        }
    }

}
