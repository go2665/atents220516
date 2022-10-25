using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLogger : MonoBehaviour
{
    /// 출력할 내용
    /// 유저의 공격이 빗나갔을 때 - "당신의 포탄이 빗나갔습니다."
    /// 유저의 공격이 명중했을 때 - "Enemy의 {배종류}에 포탄이 명중했습니다."
    /// 적의 공격이 빗나갔을 때 - "적의 포탄이 빗나갔습니다."
    /// 적의 공격이 명중했을 때 - "당신의 {배종류}에 포탄이 명중했습니다."
    /// 유저가 적의 배를 침몰 시켰을 때 - "Enemy의 {배종류}를 침몰 시켰습니다."
    /// 적이 유저의 배를 침몰 시켰을 때 - "당신의 {배종류}가 침몰 했습니다."
    /// 턴이 시작했을 때 - "{턴숫자} 번째 턴이 시작했습니다."
    /// 

    public Color userColor;
    public Color enemyColor;
    public Color shipColor;

    TextMeshProUGUI logText;

    private void Awake()
    {
        logText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Log(string text)
    {
        logText.text = text;
    }

    public void Log_Attack_Success(PlayerBase victim, ShipType type)
    {
        //"Enemy의 {배종류}에 포탄이 명중했습니다."
        
        if(type != ShipType.None)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(shipColor);
            Log($"{victim.gameObject.name}의 <#{hexColor}>{victim.Ships[(int)type - 1].Name}</color>에 포탄이 명중했습니다.");
        }
    }

    public void Log_Attack_Fail()
    {

    }

    public void Log_Ship_Destroy()
    {

    }

    public void Log_Turn_Start()
    {

    }
}
