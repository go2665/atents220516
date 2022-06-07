using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anim = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        // 애니메이션 클립정보 가져오기 GetCurrentAnimatorClipInfo        
        Destroy(this.gameObject, anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);

    }

    //private void Update()
    //{
    //    // normalizedTime : 애니메이션 클립이 한번 100% 재생되면 1. 2번 재생되면 2.
    //    if ( anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f )
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}
}
