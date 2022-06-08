using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpark : MonoBehaviour
{
    private void Awake()
    {
        Animator anim = GetComponent<Animator>();
        Destroy(this.gameObject, anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);    // 미봉책. 버그를 안보이게만 해 놓은 것
    }
}
