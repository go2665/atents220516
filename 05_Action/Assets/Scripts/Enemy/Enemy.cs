using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform patrolRoute;
    NavMeshAgent agent;
    Animator anim;

    EnemyState state = EnemyState.Idle;

    //Idle 용 --------------------------------------------------------------------------------------
    float waitTime = 3.0f;
    float timeCountDown = 3.0f;

    //---------------------------------------------------------------------------------------------

    int childCount = 0; // patrolRoute의 자식 개수
    int index = 0;      // 다음 목표인 patrolRoute의 자식

    //---------------------------------------------------------------------------------------------

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if(patrolRoute)
        {
            childCount = patrolRoute.childCount;    // 자식 개수 설정
        }
    }

    private void Update()
    {
        //if(patrolRoute!=null)
        //{
        //    agent.SetDestination(patrolRoute.position);  // 길찾기는 연산량이 많은 작업. SetDestination을 자주하면 안된다.
        //}        

        switch (state)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.Dead:
            default:
                break;
        }
    }

    void IdleUpdate()
    {
        timeCountDown -= Time.deltaTime;
        if(timeCountDown < 0)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    void PatrolUpdate()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)  // 도착하면
        {
            Debug.Log("도착");
            index++;                // 다음 인덱스 계산해서
            index %= childCount;    // index = index % childCount;
            
            ChangeState(EnemyState.Idle);
        }
    }

    void ChaseUpdate()
    {

    }

    void AttackUpdate()
    {

    }

    void ChangeState( EnemyState newState )
    {
        // 이전 상태를 나가면서 해야할 일들
        switch (state)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Patrol:
                agent.isStopped = true;
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }

        // 새 상태로 들어가면서 해야할 일들
        switch (newState)
        {
            case EnemyState.Idle:
                timeCountDown = waitTime;
                break;
            case EnemyState.Patrol:
                agent.isStopped = false;
                agent.SetDestination(patrolRoute.GetChild(index).position); // 다음 인덱스로 이동
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }

        state = newState;
        anim.SetInteger("EnemyState", (int)state);
    }
}
