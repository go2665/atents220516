using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float attackPower = 30.0f;
    float defencePower = 10.0f;
    float criticalRate = 0.3f;
    public float AttackPower { get => attackPower; }

    public float DefencePower { get => defencePower; }

    private void Awake()
    {
        anim = GetComponent<Animator>();

        weapon = GetComponentInChildren<FindWeapon>().gameObject;
        sheild = GetComponentInChildren<FindShield>().gameObject;

        ps = weapon.GetComponentInChildren<ParticleSystem>();
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("OnTriggerEnter : " + other.name);
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("OnCollisionEnter : " + collision.gameObject.name);
    //}
}
