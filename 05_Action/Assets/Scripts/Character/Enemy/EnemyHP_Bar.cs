using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP_Bar : MonoBehaviour
{
    IHealth target;
    Transform fillPivot;

    private void Awake()
    {
        target = GetComponentInParent<IHealth>();
        target.onHealthChange += SetHP_Value;
        fillPivot = transform.Find("FillPivot");        
    }

    void SetHP_Value()
    {
        if( target != null )
        {
            float ratio = target.HP / target.MaxHP;
            fillPivot.localScale = new Vector3(ratio, 1, 1);
        }
    }

    // 빌보드 -> 항상 카메라를 정면으로 마주보는 오브젝트    
    private void LateUpdate()
    {
        //transform.LookAt(transform.position + Camera.main.transform.position - transform.position);
        transform.forward = -Camera.main.transform.forward;
    }
}
