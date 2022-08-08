using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, IHealth, IMana, IBattle, IEquipTarget
{
    GameObject weapon;
    GameObject sheild;

    ParticleSystem ps;
    CapsuleCollider weaponCollider;
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
                hp = Mathf.Clamp(value, 0, maxHP);
                onHealthChange?.Invoke();
            }
        }
    }

    public float MaxHP
    {
        get => maxHP;
    }

    public System.Action onHealthChange { get; set; }

    // IMana --------------------------------------------------------------------------------------
    public float mp = 150.0f;
    float maxMP = 150.0f;

    public float MP
    {
        get => mp;
        set
        {
            if (mp != value)
            {
                mp = Mathf.Clamp(value, 0, maxMP);
                onManaChange?.Invoke();
            }
        }
    }

    public float MaxMP => maxMP;

    public System.Action onManaChange { get ; set; }


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
        set
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
    float dropRange = 2.0f;

    // 인벤토리 용 ---------------------------------------------------------------------------------
    Inventory inven;

    ItemSlot equipItemSlot;

    public ItemSlot EquipItemSlot => equipItemSlot;

    //---------------------------------------------------------------------------------------------


    private void Awake()
    {
        anim = GetComponent<Animator>();

        weapon = GetComponentInChildren<FindWeapon>().gameObject;
        sheild = GetComponentInChildren<FindShield>().gameObject;

        ps = weapon.GetComponentInChildren<ParticleSystem>();

        inven = new Inventory();
    }

    void Start()
    {
        if (lockOnEffect == null)
        {
            lockOnEffect = GameObject.Find("LockOnEffect");
            lockOnEffect.SetActive(false);
        }
        GameManager.Inst.InvenUI.InitializeInventory(inven);
    }

    public void ShowWeapons(bool isShow)
    {
        weapon.SetActive(isShow);
        sheild.SetActive(isShow);
    }

    public void TurnOnAura(bool turnOn)
    {
        if (ps != null)
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
    }

    public void Attack(IBattle target)
    {
        if (target != null)
        {
            float damage = AttackPower;

            if(EquipItemSlot != null && EquipItemSlot.ItemEquiped)
            {
                ItemData_Weapon weapon = EquipItemSlot.SlotItemData as ItemData_Weapon;
                damage += weapon.attackPower;
            }

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

            if (lockOnTarget?.gameObject != nearest.gameObject) // 다른 대상을 락온 할 때만 실생
            {
                if( LockOnTarget != null )
                {
                    LockOff();  // LockOff 델리게이트 연결 해제용
                }

                lockOnTarget = nearest.transform;   // 가장 가까이 있는 대상을 락온 대상으로 설정
                                                    //Debug.Log($"Lock on : {lockOnTarget.name}");

                lockOnTarget.gameObject.GetComponent<Enemy>().OnDead += LockOff;

                lockOnEffect.transform.position = lockOnTarget.position;    // 락온 이팩트를 락온 대상의 위치로 이동
                lockOnEffect.transform.parent = lockOnTarget;               // 락온 이팩트의 부모를 락온 대상으로 설정
                lockOnEffect.SetActive(true);                               // 락온 이팩트 보여주기

                result = true;
            }
        }

        return result;
    }

    void LockOff()
    {
        lockOnTarget.gameObject.GetComponent<Enemy>().OnDead -= LockOff;
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

            // as : as 앞의 변수가 as 뒤의 타입으로 캐스팅이 되면 캐스팅 된 결과를 주고 안되면 null을 준다.
            // is : is 앞의 변수가 is 뒤의 타입으로 캐스팅이 되면 true, 아니면 false
            IConsumable consumable = item.data as IConsumable;  
            if (consumable != null)
            {
                consumable.Consume(this);   // 먹자마자 소비하는 형태의 아이템은 각자의 효과에 맞게 사용됨                
                Destroy(col.gameObject);
            }
            else
            {
                if( inven.AddItem(item.data) )
                {
                    Destroy(col.gameObject);
                }
            }            
        }

        //Debug.Log($"플레이어의 돈 : {money}");
    }

    public Vector3 ItemDropPosition(Vector3 inputPos)
    {
        Vector3 result = Vector3.zero;
        Vector3 toInputPos = inputPos - transform.position;
        if(toInputPos.sqrMagnitude > dropRange * dropRange)
        {
            // inputPos가 dropRange 밖에 있다.
            result = transform.position + toInputPos.normalized * dropRange;
        }
        else
        {
            // inputPos가 dropRange 안에 있다.
            result = inputPos;
        }

        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.up, lockOnRange);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, itemPickupRange);
    }
#endif

    /// <summary>
    /// 아이템 장비
    /// </summary>
    /// <param name="weaponSlot">장비하는 무기 아이템 데이터</param>
    public void EquipWeapon(ItemSlot weaponSlot)
    {
        ShowWeapons(true);  // 장비하면 무조건 보이도록
        GameObject obj = Instantiate(weaponSlot.SlotItemData.prefab, weapon.transform);  // 새로 장비할 아이템 생성하기
        obj.transform.localPosition = new(0, 0, 0);             // 부모에게 정확히 붙도록 로컬을 0,0,0으로 설정
        ps = obj.GetComponent<ParticleSystem>();                // 파티클 시스템 갱신
        weaponCollider = obj.GetComponent<CapsuleCollider>();
        equipItemSlot = weaponSlot;                             // 장비한 아이템 표시
        equipItemSlot.ItemEquiped = true;
    }

    /// <summary>
    /// 아이템 해제
    /// </summary>
    public void UnEquipWeapon()
    {
        equipItemSlot.ItemEquiped = false;
        equipItemSlot = null;   // 장비가 해재됬다는 것을 표시하기 위함(IsWeaponEquiped 변경용)
        weaponCollider = null;
        ps = null;          // 파티클 시스템 비우기
        Transform weaponChild = weapon.transform.GetChild(0);   
        weaponChild.parent = null;          // 무기가 붙는 장소에 있는 자식 지우기
        Destroy(weaponChild.gameObject);    // 무기 디스트로이
    }

    public void WeaponColliderOn()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }

    public void WeaponColliderOff()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }


    public void Test()
    {
        inven.AddItem(ItemIDCode.OneHandSword1);
        EquipWeapon(inven[0]);        
    }
}
