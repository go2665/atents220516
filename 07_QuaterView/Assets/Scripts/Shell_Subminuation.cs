using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Subminuation : Shell
{
    protected override void Start()
    {
        // 아래쪽 방향으로 자탄 쏘기
        Vector2 random = Random.insideUnitCircle;
        rigid.velocity = (Vector3.down + new Vector3(random.x, 0, random.y)).normalized * data.initialSpeed;
    }
}
