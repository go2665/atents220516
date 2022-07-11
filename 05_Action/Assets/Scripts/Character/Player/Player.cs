using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject weapon;
    GameObject sheild;

    ParticleSystem ps;

    private void Awake()
    {
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

}
