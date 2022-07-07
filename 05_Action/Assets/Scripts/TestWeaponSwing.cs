using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestWeaponSwing : MonoBehaviour
{
    // polling : 데이터를 당겨오는 것. 계속 데이터의 변화를 확인하다가 원하는 상태가 되면 당겨오는 처리.

    bool movingStart = false;
    float speed = 180.0f;
    float angle = 0.0f;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //if( Input.GetKeyDown(KeyCode.Space) )
        if( Keyboard.current.spaceKey.wasPressedThisFrame )
        {
            Debug.Log("Space!");
            //movingStart = true;
            anim.SetTrigger("Swing");
        }

        if(movingStart)
        {
            angle += (speed * Time.deltaTime);
            if(angle > 360.0f)
            {
                movingStart = false;
                angle = 0.0f;
            }
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        //Debug.Log(angle);
    }
}
