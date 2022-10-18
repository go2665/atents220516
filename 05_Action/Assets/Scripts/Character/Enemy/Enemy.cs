using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : MonoBehaviour, IHealth, IBattle
{
    public Transform patrolRoute;
    NavMeshAgent agent;
    Animator anim;

    EnemyState state = EnemyState.Idle;

    public System.Action OnDead;

    //Idle 용 --------------------------------------------------------------------------------------
    float waitTime = 3.0f;
    float timeCountDown = 3.0f;

    //Patrol 용 ------------------------------------------------------------------------------------

    int childCount = 0; // patrolRoute의 자식 개수
    int index = 0;      // 다음 목표인 patrolRoute의 자식


    //추적용------------------------------------------------------------------------------------------
    float sightRange = 10.0f;
    float closeSightRange = 2.5f;
    Vector3 targetPosition = new();
    WaitForSeconds oneSecond = new WaitForSeconds(1.0f);
    IEnumerator repeatChase = null;
    float sightAngle = 150.0f;   //-45 ~ +45 범위

    //공격용 -----------------------------------------------------------------------------------------
    float attackCoolTime = 1.0f;
    float attackSpeed = 1.0f;
    IBattle attackTarget;

    //사망용 -----------------------------------------------------------------------------------------
    bool isDead = false;

    //IHealth -------------------------------------------------------------------------------------
    public float hp = 100.0f;
    float maxHP = 100.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0.0f, maxHP);
            onHealthChange?.Invoke();
        }
    }

    public float MaxHP { get => maxHP; }

    public System.Action onHealthChange { get; set; }


    //IBattle -------------------------------------------------------------------------------------
    public float attackPower = 10.0f;
    public float defencePower = 10.0f;
    float criticalRate = 0.1f;

    public float AttackPower { get => attackPower; }        

    public float DefencePower { get => defencePower; }

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
            //Debug.Log("도착");
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
        if(colliders.Length > 0)    // 시야 범위 안에 있는가?
        {
            Vector3 pos = colliders[0].transform.position;
            if (InSightAngle(pos))  // 시야 각도 안에 있는가?
            {
                if (!BlockByWall(pos))  // 벽에 가렸는가?
                {
                    targetPosition = pos;
                    result = true;
                }
            }
            if(!result && (pos-transform.position).sqrMagnitude < closeSightRange * closeSightRange )
            {
                targetPosition = pos;
                result = true;
            }
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
        attackCoolTime -= Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f);
        if ( attackCoolTime < 0.0f)
        {
            anim.SetTrigger("Attack");
            Attack(attackTarget);
            attackCoolTime = attackSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Inst.MainPlayer.gameObject)
        {
            attackTarget = other.GetComponent<IBattle>();
            ChangeState(EnemyState.Attack);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Inst.MainPlayer.gameObject)
        {
            ChangeState(EnemyState.Chase);
            return;
        }
    }

    void ChangeState(EnemyState newState)
    {
        if (isDead)
        {
            return;
        }

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
                attackTarget = null;
                break;
            case EnemyState.Dead:
                agent.isStopped = true;
                isDead = false;
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
                attackCoolTime = attackSpeed;
                break;
            case EnemyState.Dead:
                DiePresent();
                break;
            default:
                break;
        }

        state = newState;
        anim.SetInteger("EnemyState", (int)state);
    }

    void DiePresent()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");    // 죽었을 때 락온이 다시 되지 않게 하기 위해 설정
        OnDead?.Invoke();               // 죽었을 때 실행되는 델리게이트(락온 해제)
        anim.SetBool("Dead", true);
        anim.SetTrigger("Die");
        isDead = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        HP = 0;
        ItemDrop();
        StartCoroutine(DeadEffect());
    }

    void ItemDrop()
    {
        float randomSelect = Random.Range(0.0f, 1.0f);        
        
        if( randomSelect < 0.001f)
        {
            ItemFactory.MakeItem(ItemIDCode.OneHandSword2);
        }
        else if( randomSelect < 0.1f )
        {
            ItemFactory.MakeItem(ItemIDCode.Coin_Gold, transform.position, true);            
        }
        else if( randomSelect < 0.3f )
        {
            ItemFactory.MakeItem(ItemIDCode.Coin_Silver, transform.position, true);
        }
        else if (randomSelect < 0.4f)
        {
            ItemFactory.MakeItem(ItemIDCode.HealingPotion, transform.position, true);
        }
        else if (randomSelect < 0.5f)
        {
            ItemFactory.MakeItem(ItemIDCode.ManaPotion, transform.position, true);
        }
        else
        {
            ItemFactory.MakeItem(ItemIDCode.Coin_Copper, transform.position, true);
        }
    }

    IEnumerator DeadEffect()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.Play();
        ps.gameObject.transform.parent = null;

        EnemyHP_Bar hpBar = GetComponentInChildren<EnemyHP_Bar>();
        hpBar.gameObject.SetActive(false);

        yield return new WaitForSeconds(3.0f);
        Collider[] colliders = GetComponents<Collider>();
        foreach(var col in colliders)
        {
            col.enabled = false;
        }
        agent.enabled = false;
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = false;
        rigid.drag = 20.0f;
        Destroy(this.gameObject, 5.0f);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);


        Handles.color = Color.green;
        if( state == EnemyState.Chase || state == EnemyState.Attack )
        {
            Handles.color = Color.red;  // 추적이나 공격 중일 때만 빨간색
        }
        Handles.DrawWireDisc(transform.position, transform.up, closeSightRange); // 근접 시야 범위

        Vector3 forward = transform.forward * sightRange;
        Quaternion q1 = Quaternion.Euler(0.5f * sightAngle * transform.up);        
        Quaternion q2 = Quaternion.Euler(-0.5f * sightAngle * transform.up);
        Handles.DrawLine(transform.position, transform.position + q1 * forward);    // 시야각 오른쪽 끝
        Handles.DrawLine(transform.position, transform.position + q2 * forward);    // 시야각 왼쪽 끝

        Handles.DrawWireArc(transform.position, transform.up, q2 * transform.forward, sightAngle, sightRange, 5.0f);// 전체 시야범위
    }
#endif

    /// <summary>
    /// 플레이어가 시야각도(sightAngle) 안에 있으면 true를 리턴
    /// </summary>
    /// <returns></returns>
    bool InSightAngle(Vector3 targetPosition)
    {
        // 두 백터의 사이각
        float angle = Vector3.Angle(transform.forward, targetPosition - transform.position);
        // 몬스터의 시야범위 각도사이에 있는지 없는지
        return (sightAngle * 0.5f) > angle;
    }

    /// <summary>
    /// 벽에 대상이 숨어서 안보이는지 확인하는 함수
    /// </summary>
    /// <param name="targetPosition">확인할 대상의 위치</param>
    /// <returns>true면 벽에 가려져 있는 것. false면 가려져 있지않다.</returns>
    bool BlockByWall(Vector3 targetPosition)
    {
        bool result = true;
        Ray ray = new(transform.position, targetPosition - transform.position); // 레이 만들기(시작점, 방향)
        ray.origin += Vector3.up * 0.5f;    // 몬스터의 눈높이로 레이 시작점을 높임
        if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
        {
            if( hit.collider.CompareTag("Player") )     // 레이에 무언가가 걸렸는데 "Player"태그를 가지고 있으면
            {
                result = false; // 바로 보인 것이니 벽이 가리고 있지 않다.
            }
        }

        return result;  // true면 벽이 가렸거나 아무것도 충돌하지 않았거나
    }

    public void Attack(IBattle target)
    {
        if(target != null)
        {
            float damage = AttackPower;
            if(Random.Range(0.0f,1.0f) < criticalRate)
            {
                damage *= 2.0f;
            }
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defencePower;
        if(finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }
        HP -= finalDamage;

        if( HP > 0.0f)
        {
            //살아있다.
            anim.SetTrigger("Hit");
            attackCoolTime = attackSpeed;
        }
        else
        {
            //죽었다.
            Die();
        }
    }

    void Die()
    {
        if (isDead == false)
        {
            ChangeState(EnemyState.Dead);
        }
    }
}
