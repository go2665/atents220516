using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger == false)
        {
            //Debug.Log("OnTriggerEnter : " + other.name);
            IBattle battle = other.GetComponent<IBattle>();
            if(battle!=null)
            {
                GameManager.Inst.MainPlayer.Attack(battle);
                //battle.TakeDamage(30.0f);
            }
        }
    }
}
