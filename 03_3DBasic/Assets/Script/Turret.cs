using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target = null;
    public GameObject bullet = null;
    public float range = 5.0f;
    public float angle = 15.0f;
    public float fireInterval = 1.0f;
        
    Transform turretHead = null;
    Transform firePostion = null;
    float lookSpeed = 2.0f;
    float halfAngle = 0.0f;
    float fireCooltime = 0.0f;

    private void Awake()
    {
        turretHead = transform.Find("Head");
        firePostion = turretHead.Find("FirePosition");
        halfAngle = angle * 0.5f;   // 나누기를 하는 것보다 곱하기가 좋다.
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position; // 포탑에서 플레이어로 향하는 방향 백터
        dir.y = 0.0f;   // 지형 높이차가 생기면 반드시 해야 한다.
        //dir의 길이 = root(dir.x*dir.x + dir.y*dir.y + dir.z*dir.z)
        //dir의 길이 = dir.magnitude;

        fireCooltime -= Time.deltaTime;     // 항상 쿨타임 감소

        if ( dir.sqrMagnitude < range * range )
        {
            // 포탑의 range 안이다.
            //Vector3 targetPos = target.position;
            //targetPos.y = turretHead.position.y;
            //turretHead.LookAt(targetPos);
            turretHead.rotation = Quaternion.Lerp(  // 보간함수. (시작값, 도착값, 시간) 3가지를 받아서 계산된 결과를 돌려준다.
                                                    // 시간이 0이면 시작값, 시간이 1이면 도착값, 시간이 0~1사이면 비율에 맞춰서
                turretHead.rotation,            // 시작값. (현재 포탑머리의 회전)
                Quaternion.LookRotation(dir),   // 도착값. (dir방향으로 바라보는 회전)
                lookSpeed * Time.deltaTime);    // 1초동안 모으면 2가 된다. 0 -> 1로 가는데 걸리는 시간은 0.5초가된다. => 시작에서 도착까지 가는데 0.5초가 걸린다.

            float angleBetween = Vector3.Angle(turretHead.forward, dir);
            if( angleBetween < halfAngle )
            {
                //Debug.Log($"Fire : {angleBetween}");
                //Fire();
                if(fireCooltime < 0.0f) // 쿨타임이 0초 미만일 때만 발사 가능
                {
                    Fire();
                    fireCooltime = fireInterval;    // 쿨타임 초기화
                }
            }
        }
    }

    void Fire()
    {
        Instantiate(bullet, firePostion.position, firePostion.rotation);
    }
}
