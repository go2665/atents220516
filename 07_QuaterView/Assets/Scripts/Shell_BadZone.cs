using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_BadZone : Shell
{
    public float effectDuration = 5.0f;

    //protected override void OnCollisionEnter(Collision collision)
    //{
    //    Ray ray = new Ray(collision.contacts[0].point, Vector3.down);
    //    Vector3 position;
    //    Quaternion rotation = Quaternion.LookRotation(Vector3.up);
    //    if( Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")))
    //    {
    //        position = hit.point;
    //    }
    //    else
    //    {
    //        position = collision.contacts[0].point;
    //    }
    //    position += new Vector3(0, 0.1f, 0);

    //    GameObject obj = Instantiate(explosionPrefab, position, rotation);
    //    ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>();
    //    foreach(var ps in pss)
    //    {
    //        var main = ps.main;
    //        main.duration = effectDuration;            
    //    }
    //    pss[0].Play();

    //    BadEffect bedEffect = obj.GetComponent<BadEffect>();
    //    bedEffect.damagePerSecond = damage / effectDuration;    // 전체 데미지 / 전체시간

    //    Destroy(this.gameObject);
    //}
}
