using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipsInfo : MonoBehaviour
{
    /// <summary>
    /// 이 패널에서 표시할 배들을 가진 플레이어
    /// </summary>
    public PlayerBase player;

    /// <summary>
    /// 배 HP 표시할 텍스트
    /// </summary>
    TextMeshProUGUI[] texts;

    /// <summary>
    /// 표시될 배들
    /// </summary>
    Ship[] ships;

    private void Awake()
    {
        // 컴포넌트 찾기
        texts = GetComponentsInChildren<TextMeshProUGUI>();        
    }

    private void Start()
    {
        // 배 정보 가져오기
        ships = player.Ships;
        for (int i=0;i <ships.Length;i++)
        {
            // 초기값 출력
            texts[i].text = $"{ships[i].HP}/{ships[i].Size}";

            // 배가 공격 당할 때나 충돌했을 때 실행될 델리게이트에 함수 등록
            int index = i;
            ships[i].onHit += (ship) => { TextRefresh(texts[index], ship); };
            ships[i].onSinking += (ship) => { ShinkingPrint(texts[index], ship); };
        }
    }

    /// <summary>
    /// HP 텍스트 갱신
    /// </summary>
    /// <param name="hpText">수정할 텍스트</param>
    /// <param name="ship">수정될 배</param>
    void TextRefresh(TextMeshProUGUI hpText, Ship ship)
    {
        hpText.text = $"{ship.HP}/{ship.Size}";
    }

    /// <summary>
    /// 함선 칠몰 표시용
    /// </summary>
    /// <param name="hpText">주정할 텍스트</param>
    /// <param name="_">사용안함</param>
    void ShinkingPrint(TextMeshProUGUI hpText, Ship _)
    {
        hpText.fontSize = 40;   // 크기 살짝 줄이고
        hpText.text = "<#ff0000>Destroyed!</color>";    // 빨간색으로 Destroy 찍기
    }

}
