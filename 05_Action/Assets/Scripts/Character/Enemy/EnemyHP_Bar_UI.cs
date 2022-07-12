using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP_Bar_UI : MonoBehaviour
{
    IHealth target;
    Image fill;

    private void Awake()
    {
        target = GetComponentInParent<IHealth>();
        target.onHealthChange += SetHP_Value;
        fill = transform.Find("Fill").GetComponent<Image>();        
    }

    void SetHP_Value()
    {
        if( target != null )
        {
            float ratio = target.HP / target.MaxHP;
            fill.fillAmount = ratio;
        }
    }

    // 빌보드 -> 항상 카메라를 정면으로 마주보는 오브젝트    
    private void LateUpdate()
    {
        //transform.LookAt(transform.position + Camera.main.transform.position - transform.position);
        //transform.forward = -Camera.main.transform.forward;
        transform.rotation = Camera.main.transform.rotation;
    }
}
