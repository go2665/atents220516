using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 배에 대한 전반적인 관리를 하는 클래스
/// </summary>
public class ShipManager : Singleton<ShipManager>
{
    /// <summary>
    /// 배의 프리팹
    /// </summary>
    public GameObject shipPrefab;

    /// <summary>
    /// 배의 모델용 프리팹
    /// </summary>
    public GameObject[] shipModels;

    /// <summary>
    /// 배의 머티리얼(0번째는 정상, 1번째는 배치모드)
    /// </summary>
    public Material[] shipMaterials;

    readonly Color successColor = new(0, 1, 0, 0.2f);
    readonly Color failColor = new(1, 0, 0, 0.2f);

    /// <summary>
    /// 배의 종류 개수
    /// </summary>
    int shipTypeCount;

    /// <summary>
    /// 배가 바라보는 방향 갯수
    /// </summary>
    int shipDirectionCount = 0;

    /// <summary>
    /// 배의 종류가 몇가지인지 알기 위한 프로퍼티
    /// </summary>
    public int ShipTypeCount { get => shipTypeCount; }

    /// <summary>
    /// 배가 바라볼 수 있는 방향이 몇 방향인지 알기 위한 프로퍼티
    /// </summary>
    public int ShipDirectionCount { get => shipDirectionCount; }

    /// <summary>
    /// 배가 정상적인 상황일 때 사용하는 머티리얼 프로퍼티
    /// </summary>
    public Material NormalShipMaterial => shipMaterials[0];

    /// <summary>
    /// 배가 배치모드일 때 사용하는 머티리얼 프로퍼티
    /// </summary>
    public Material TempShipMaterial => shipMaterials[1];

    protected override void Awake()
    {
        base.Awake();

        // enum들의 값 가지수 찾아 저장하기
        shipTypeCount = Enum.GetValues(typeof(ShipType)).Length - 1;
        shipDirectionCount = Enum.GetValues(typeof(ShipDirection)).Length;

        SetTempShipColor(true);
    }

    /// <summary>
    /// 배를 만드는 함수
    /// </summary>
    /// <param name="type">만들 배의 종류</param>
    /// <param name="player">만들 배를 가지는 대상</param>
    /// <returns>만든 배</returns>
    public Ship MakeShip(ShipType type, PlayerBase player)
    {
        GameObject shipObj = Instantiate(shipPrefab, player.transform);
        Ship ship = shipObj.GetComponent<Ship>();
        ship.Initialize(type);
        shipObj.name = $"{type}_{ship.Size}";
        shipObj.SetActive(false);

        return ship;
    }

    /// <summary>
    /// 배 생성을 위한 배모델 가져오기
    /// </summary>
    /// <param name="type">리턴할 배의 종류</param>
    /// <returns>배의 모델 프리팹</returns>
    public GameObject GetShipModel(ShipType type)
    {
        return shipModels[(int)type - 1];
    }

    /// <summary>
    /// 배치 모드의 배의 머티리얼 색상을 지정하기
    /// </summary>
    /// <param name="isSuccess">true면 투명한 녹색, false면 투명한 빨강</param>
    public void SetTempShipColor(bool isSuccess)
    {
        if( isSuccess )
        {
            TempShipMaterial.SetColor("_Color", successColor);
        }
        else
        {
            TempShipMaterial.SetColor("_Color", failColor);
        }
    }
}
