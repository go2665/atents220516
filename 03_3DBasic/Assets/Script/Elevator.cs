using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, IUseable
{
    Animator anim = null;
    bool moveUp = true;

    List<Rigidbody> passengers = new List<Rigidbody>();

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        foreach( Rigidbody rigid in passengers)
        {
            // 엘리베이터 안에 있는 Rigidbody들의 높이를 엘리베이터 바닥 높이로 고정
            rigid.MovePosition(new Vector3(rigid.position.x, transform.position.y + 0.25f, rigid.position.z));
        }
    }

    public void Use()
    {
        if( moveUp )
        {
            anim.SetTrigger("Up");
            moveUp = false;
        }
        else
        {
            anim.SetTrigger("Down");
            moveUp = true;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        // 중복 발생 가능 이유
        // 이 트리거에 다른 컬라이더가 들어왔을 때.
        // 리지드바디를 가지는 오브젝트의 트리거에 이 오브젝트의 컬러이더가 들어갔을 때.
        // 위 두 경우 모두 실행되기 때문

        if (rigid)
        {
            // 중복으로 리스트에 들어가는 것을 방지하기 위해 같은 rigidbody가 있는지 찾기
            // 람다식 (     (x) => x == rigid          ) 이용
            // x는 passengers안에 있는 엘리먼트들
            // 그 중에서 rigid와 같은 것이 있으면 해당 엘리먼트를 리턴
            Rigidbody find = passengers.Find((x) => x == rigid);  
            if (find == null)
            {
                passengers.Add(rigid);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if (rigid)
        {
            passengers.Remove(rigid);
        }
    }
}
