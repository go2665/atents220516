using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTank : Tank
{
    public float fireAngle = 15.0f;     // 발사각 (-15~+15)
    public float attackRange = 20.0f;   // 발사 거리

    private PlayerTank player;          // 추적할 플레이어

    private NavMeshAgent agent;         // 길찾기용 컴포넌트

    protected override void Awake()
    {
        base.Awake();        
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<PlayerTank>();    // 플레이어 찾기
        if (player != null)
        {
            player.onDead += PlayerDead;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!isDead && player != null )
        {
            Vector3 playerPos = player.transform.position;
            Vector3 dir = playerPos - transform.position;

            // 연산량 비교
            // ( dir.sqrMagnitude < attackRange * attackRange ) 
            // (dir.x * dir.x + dir.y * dir.y + dir.z * dir.z) < attackRange * attackRange 
            // * 4번, + 2번, < 1번 

            // ( Vector3.Angle( dir, transform.forward ) < fireAngle )
            // acos(dir.x * transform.forward.x + dir.y * transform.forward.y + dir.z * transform.forward.z) / root(dir.x * dir.x + dir.y * dir.y + dir.z * dir.z) * root(transform.forward.x * transform.forward.x + transform.forward.y * transform.forward.y + transform.forward.z * transform.forward.z)
            // * 9번, + 6번, / 1번, root 2번, acos 1번, < 1번

            if (fireDatas[0].IsFireReady                                    // 발사 쿨타임이 다 되었고
                && (dir.sqrMagnitude < attackRange * attackRange)           // 발사 거리안에 플레이어가 있고
                && (Vector3.Angle(dir, transform.forward) < fireAngle) )    // 발사 각도안에 플레이어가 있다.
            {
                Instantiate(shellPrefabs[0], firePosition.position, firePosition.rotation); // 포탄 발사
                fireDatas[0].ResetCoolTime();                               // 쿨타임 다시 돌리기
            }

            agent.SetDestination(playerPos);    // 살아있으면 무조건 플레이어쪽으로 추적
        }
    }    

    public override void Dead()
    {
        if (!isDead)
        {
            player.onDead -= PlayerDead;
            agent.isStopped = true;     //길찾기 중단 후 컴포넌트 비활성화
            agent.enabled = false;
        }

        base.Dead();
    }

    private void PlayerDead()
    {
        // 플레이어 사망시 호출될 함수
        player = null;        
        agent.isStopped = true;     // 길찾기 중단.
    }
}
