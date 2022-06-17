using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IUseable
{
    public Door door = null;

    Animator anim = null;
    bool switchOnOff = false;

    void Awake()
    {
        anim = GetComponent<Animator>();

        // 내 부모의 자식 중 이름이 "Door_Switch"인 게임 오브젝트를 찾아 Door 컴포넌트 가져오기
        door = transform.parent.Find("Door_Switch")?.GetComponent<Door>();  
    }

    public void Use()
    {
        switchOnOff = !switchOnOff; // not(불변수 뒤집기)
        anim.SetBool("IsSwitchOn", switchOnOff);

        if( switchOnOff == true )
        {
            door?.Open();
        }
        else
        {
            door?.Close();
        }
    }
}
