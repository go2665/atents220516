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

    /// <summary>
    /// 배가 배치되었는지에 대한 정보
    /// </summary>
    bool isDeployed = false;

    /// <summary>
    /// 배의 HP
    /// </summary>
    int hp = 0;

    /// <summary>
    /// 함선의 생존 여부
    /// </summary>
    bool isAlive = true;    

    /// <summary>
    /// 배가 가지고 있는 모델의 랜더러. 머티리얼 변경용
    /// </summary>
    Renderer shipRenderer;

    // 델리게이트 ----------------------------------------------------------------------------------
    public Action<Ship> onDead;

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

    /// <summary>
    /// 배의 배치 여부 확인용 프로퍼티
    /// </summary>
    public bool IsDeployed { get => isDeployed; set => isDeployed = value; }

    /// <summary>
    /// 함선의 생존 여부를 알려주는 프로퍼티
    /// </summary>
    public bool IsAlive => isAlive;

    /// <summary>
    /// 배가 가지고 있는 모델의 랜더러에 접근하기 위한 프로퍼티
    /// </summary>
    public Renderer Renderer => shipRenderer;

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
        hp = size;

        // 배의 모델 생성 후 자식으로 추가
        GameObject modelPrefab = ShipManager.Inst.GetShipModel(type);
        GameObject obj = Instantiate(modelPrefab, transform);

        // 배의 모델의 랜더러 가져오기
        shipRenderer = obj.GetComponentInChildren<Renderer>();  

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

    /// <summary>
    /// 배를 랜덤한 방향으로 랜덤덤한 횟수만큼 90도씩 회전 시키는 함수
    /// </summary>
    public void RandomRotate()
    {
        int rotateCount = UnityEngine.Random.Range(0, ShipManager.Inst.ShipDirectionCount); // 랜덤한 회수로
        bool isCCW = (UnityEngine.Random.Range(0, 10) % 2) == 0;                            // 랜덤한 방향으로
        for (int i = 0; i < rotateCount; i++)
        {
            Rotate(isCCW);
        }
    }

    /// <summary>
    /// 이 배가 공격당했을 때 실행될 함수
    /// </summary>
    public void OnAttacked()
    {
        //Debug.Log($"{type} 공격 받음");
        hp--;
        if(hp <= 0)
        {
            OnDie();
        }
    }

    /// <summary>
    /// 이 배가 침몰당했을 때 실행될 함수
    /// </summary>
    private void OnDie()
    {
        Debug.Log($"{type} 침몰");
        isAlive = false;
        onDead?.Invoke(this);
    }
}
