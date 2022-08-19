using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime : MonoBehaviour
{
    Material mainMat;
    private void Start()
    {
        Renderer renderer = GetComponent<SpriteRenderer>();
        mainMat = renderer.material;
        mainMat.SetColor("_Color", Color.red * 5);
        mainMat.SetFloat("_Tickness", 0);
    }

    public void OutlineOnOff(bool on)
    {
        if( on )
            mainMat.SetFloat("_Tickness", 0.005f);
        else
            mainMat.SetFloat("_Tickness", 0.0f);
    }

    public void Move(Vector2Int target)
    {
        // 이 함수가 실행되면 이 슬라임은 target으로 가는 경로를 계산한다.
        // 경로가 존재하면 해당 위치로 경로에 따라 이동한다.
        // 경로가 없으면 가만히 있는다.
    }

    //private void Update()
    //{
    //    if( Keyboard.current.digit1Key.wasPressedThisFrame)
    //    {
    //        OutlineOnOff(true);
    //    }
    //    if (Keyboard.current.digit2Key.wasPressedThisFrame)
    //    {
    //        OutlineOnOff(false);
    //    }
    //}
}
