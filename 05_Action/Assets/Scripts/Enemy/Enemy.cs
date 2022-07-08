using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public Transform patrolRoute;
    NavMeshAgent agent;
    Animator anim;

    EnemyState state = EnemyState.Idle;

    //Idle 용 --------------------------------------------------------------------------------------
    float waitTime = 3.0f;
    float timeCountDown = 3.0f;

    //Patrol 용 ------------------------------------------------------------------------------------

    int childCount = 0; // patrolRoute의 자식 개수
    int index = 0;      // 다음 목표인 patrolRoute의 자식


    //추적용------------------------------------------------------------------------------------------
    float sightRange = 10.0f;
    Vector3 targetPosition = new();
    WaitForSeconds oneSecond = new WaitForSeconds(1.0f);
    IEnumerator repeatChase = null;

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
        if (SearchPlayer())
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        timeCountDown -= Time.deltaTime;
        if(timeCountDown < 0)
        {
            ChangeState(EnemyState.Patrol);
            return;
        }
    }

    void PatrolUpdate()
    {
        if(SearchPlayer())
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)  // 도착하면
        {
            Debug.Log("도착");
            index++;                // 다음 인덱스 계산해서
            index %= childCount;    // index = index % childCount;
            
            ChangeState(EnemyState.Idle);
            return;
        }
    }

    bool SearchPlayer()
    {
        bool result = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));
        if(colliders.Length > 0)
        {
            targetPosition = colliders[0].transform.position;
            result = true;
        }

        return result;
    }

    void ChaseUpdate()
    {
        if(!SearchPlayer())
        {
            ChangeState(EnemyState.Patrol);
            return;
        }
        
    }

    IEnumerator RepeatChase()
    {
        while (true)
        {
            yield return oneSecond;
            agent.SetDestination(targetPosition);
        }
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
                agent.isStopped = true;
                break;
            case EnemyState.Patrol:
                agent.isStopped = true;
                break;
            case EnemyState.Chase:
                agent.isStopped = true;
                StopCoroutine(repeatChase);
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                break;
            case EnemyState.Dead:
                agent.isStopped = true;
                break;
            default:
                break;
        }

        // 새 상태로 들어가면서 해야할 일들
        switch (newState)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                timeCountDown = waitTime;
                break;
            case EnemyState.Patrol:
                agent.isStopped = false;                
                agent.SetDestination(patrolRoute.GetChild(index).position); // 다음 인덱스로 이동
                break;
            case EnemyState.Chase:
                agent.isStopped = false;
                agent.SetDestination(targetPosition);
                repeatChase = RepeatChase();
                StartCoroutine(repeatChase);
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                break;
            case EnemyState.Dead:
                agent.isStopped = true;
                break;
            default:
                break;
        }

        state = newState;
        anim.SetInteger("EnemyState", (int)state);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);
    }
}
