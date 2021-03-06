using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour, IHealth, IBattle
{
    GameObject weapon;
    GameObject sheild;

    ParticleSystem ps;
    Animator anim;

    // IHealth ------------------------------------------------------------------------------------
    public float hp = 100.0f;
    float maxHP = 100.0f;

    public float HP
    {
        get => hp;
        set
        {
            if(hp != value)
            {
                hp = value;
                onHealthChange?.Invoke();
            }
        }
    }

    public float MaxHP
    {
        get => maxHP;
    }

    public System.Action onHealthChange { get; set; }

    // IBattle ------------------------------------------------------------------------------------
    public float attackPower = 30.0f;
    public float defencePower = 10.0f;
    public float criticalRate = 0.3f;
    public float AttackPower { get => attackPower; }

    public float DefencePower { get => defencePower; }

    // 락온 용 ---------------------------------------------------------------------------------------
    public GameObject lockOnEffect;
    Transform lockOnTarget;
    float lockOnRange = 5.0f;
    public Transform LockOnTarget { get => lockOnTarget; }  // 락온 대상의 트랜스폼. 읽기 전용 프로퍼티 추가

    // 아이템 용 ---------------------------------------------------------------------------------------
    int money = 0;      // 플레이어의 소지 금액
    public int Money
    {
        get => money;
        private set
        {
            if (money != value) // 실제로 금액이 변경되었을 때만 실행
            {
                money = value;
                OnMoneyChange?.Invoke(money);   // 돈이 변경되면 실행되는 델리게이트
            }
        }
    }
    float itemPickupRange = 2.0f;           // 아이템을 줍는 범위(반지름)
    public System.Action<int> OnMoneyChange;// 돈이 변경되면 실행되는 델리게이트


    private void Awake()
    {
        anim = GetComponent<Animator>();

        weapon = GetComponentInChildren<FindWeapon>().gameObject;
        sheild = GetComponentInChildren<FindShield>().gameObject;

        ps = weapon.GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        if (lockOnEffect == null)
        {
            lockOnEffect = GameObject.Find("LockOnEffect");
        }
    }

    public void ShowWeapons(bool isShow)
    {
        weapon.SetActive(isShow);
        sheild.SetActive(isShow);
    }

    public void TurnOnAura(bool turnOn)
    {
        if (turnOn)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }

    public void Attack(IBattle target)
    {
        if (target != null)
        {
            float damage = AttackPower;
            if (Random.Range(0.0f, 1.0f) < criticalRate)
            {
                damage *= 2.0f;
            }
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defencePower;
        if (finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }
        HP -= finalDamage;

        if (HP > 0.0f)
        {
            //살아있다.
            anim.SetTrigger("Hit");
        }
        else
        {
            //죽었다.
            //Die();
        }
    }

    public void LockOnToggle()
    {
        if(lockOnTarget == null)
        {
            // 락온 시도
            LockOn();
        }
        else
        {
            // 락온된 타겟이 있음
            if (!LockOn())  // 다시 락온을 시도
            {
                // 새롭게 락온이 안되면 락온 풀기
                LockOff();
            }
        }
    }

    bool LockOn()
    {
        bool result = false;

        // transform.position지점에서 반경 lockOnRange 범위 안에 있는 Enemy레이어를 가진 컬라이더를 전부 찾기
        Collider[] cols = Physics.OverlapSphere(transform.position, lockOnRange, LayerMask.GetMask("Enemy"));

        // 하나라도 오버랩된것이 있을 때만 실행
        if (cols.Length > 0)
        {
            // 가장 가까운 컬라이더를 찾기
            Collider nearest = null;
            float nearestDistance = float.MaxValue;
            foreach (Collider col in cols)
            {
                float distanceSqr = (col.transform.position - transform.position).sqrMagnitude; // 거리의 제곱으로 체크
                if (distanceSqr < nearestDistance)
                {
                    nearestDistance = distanceSqr;
                    nearest = col;
                }
            }

            lockOnTarget = nearest.transform;   // 가장 가까이 있는 대상을 락온 대상으로 설정
            //Debug.Log($"Lock on : {lockOnTarget.name}");

            lockOnTarget.gameObject.GetComponent<Enemy>().OnDead += LockOff;

            lockOnEffect.transform.position = lockOnTarget.position;    // 락온 이팩트를 락온 대상의 위치로 이동
            lockOnEffect.transform.parent = lockOnTarget;               // 락온 이팩트의 부모를 락온 대상으로 설정
            lockOnEffect.SetActive(true);                               // 락온 이팩트 보여주기

            result = true;
        }

        return result;
    }

    void LockOff()
    {
        lockOnTarget = null;                    // 락온 대상 null
        lockOnEffect.transform.parent = null;   // 락온 이팩트의 부모 제거
        lockOnEffect.SetActive(false);          // 락온 이팩트 보이지 않게 하기
    }

    public void ItemPickup()
    {
        // 주변에 Item레이어에 있는 컬라이더 전부 가져오기
        Collider[] cols = Physics.OverlapSphere(transform.position, itemPickupRange, LayerMask.GetMask("Item"));
        foreach(var col in cols)
        {
            Item item = col.GetComponent<Item>();
            Money += (int)item.data.value;  // 종류별로 돈 더하기
            Destroy(col.gameObject);
        }

        //Debug.Log($"플레이어의 돈 : {money}");
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.up, lockOnRange);
    }
}
