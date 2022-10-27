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
    /// 배의 이름
    /// </summary>
    string shipName;

    /// <summary>
    /// 배가 바라보는 방향
    /// </summary>
    ShipDirection direction;

    /// <summary>
    /// 배의 크기
    /// </summary>
    int size = 2;

    /// <summary>
    /// 배의 칸별 그리드 위치
    /// </summary>
    Vector2Int[] positions;

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

    /// <summary>
    /// 이 배를 가지고 있는 플레이어
    /// </summary>
    PlayerBase owner;

    // 델리게이트 ----------------------------------------------------------------------------------

    /// <summary>
    /// 배가 배치되거나 배치해제가 되었을 때 실행되는 델리게이트.
    /// 파라메터 : 배치할때는 true, 해제할 때는 false
    /// </summary>
    public Action<bool> onDeploy;

    /// <summary>
    /// 배가 공격을 당했을 때 실행될 델리게이트
    /// 파라메터 : 자기자신
    /// </summary>
    public Action<Ship> onHit;

    /// <summary>
    /// 배가 침몰할 때 실행될 델리게이트.
    /// 파라메터 : 자기 자신
    /// </summary>
    public Action<Ship> onSinking;

    // 프로퍼티들 ----------------------------------------------------------------------------------
    /// <summary>
    /// 배의 타입 확인용 프로퍼티
    /// </summary>
    public ShipType Type { get => type; }

    /// <summary>
    /// 배의 이름 확인용 프로퍼티
    /// </summary>
    public string Name { get => shipName; }

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
    public int Size => size;

    /// <summary>
    /// 배의 HP
    /// </summary>
    public int HP => hp;

    /// <summary>
    /// 배의 배치 여부 확인용 프로퍼티. 읽기 전용
    /// </summary>
    public bool IsDeployed => isDeployed;

    /// <summary>
    /// 함선의 생존 여부를 알려주는 프로퍼티
    /// </summary>
    public bool IsAlive => isAlive;

    /// <summary>
    /// 배가 가지고 있는 모델의 랜더러에 접근하기 위한 프로퍼티
    /// </summary>
    public Renderer Renderer => shipRenderer;

    /// <summary>
    /// 배의 칸별 그리드 좌표를 읽을 수 있는 프로퍼티
    /// </summary>
    public Vector2Int[] Positions => positions;

    /// <summary>
    /// 이 배를 가지고 있는 플레이어를 확인 할 수 있는 프로퍼티
    /// </summary>
    public PlayerBase Owner => owner;

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
                shipName = "항공모함";
                break;
            case ShipType.Battleship:
                shipName = "전함";
                size = 4;
                break;
            case ShipType.Destroyer:
                shipName = "구축함";
                size = 3;
                break;
            case ShipType.Submarine:
                shipName = "잠수함";
                size = 3;
                break;
            case ShipType.PatrolBoat:
                shipName = "경비정";
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

        // 배의 소유자 등록
        owner = GetComponentInParent<PlayerBase>();
    }

    /// <summary>
    /// 함선이 배치될 때 실행될 함수
    /// </summary>
    /// <param name="positions">배치된 위치들</param>
    public void Deploy(Vector2Int[] positions)
    {
        this.positions = positions;
        isDeployed = true;
        onDeploy?.Invoke(true);
    }

    /// <summary>
    /// 함선이 배치 해제되었을 때 실행될 함수
    /// </summary>
    public void UnDeploy()
    {
        isDeployed = false;
        onDeploy?.Invoke(false);
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
        //Debug.Log($"{owner.name}의 {shipName}이 공격 받음");
        hp--;
        if(hp > 0)
        {
            onHit?.Invoke(this);    // 맞았는데 침몰되지 않았으면 맞았다고 델리게이트 실행
        }
        else
        {
            OnSinking();            // 맞았는데 HP가 0 이하면 침몰
        }
    }

    /// <summary>
    /// 이 배가 침몰당했을 때 실행될 함수
    /// </summary>
    private void OnSinking()
    {
        Debug.Log($"{type} 침몰");
        isAlive = false;
        onSinking?.Invoke(this);
    }
}
