using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_BadZone : Shell
{
    [ColorUsage(true, true)]
    public Color effectColor;

    public float effectDuration = 5.0f;

    Material material;

    protected override void Awake()
    {
        base.Awake();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        material.SetColor("_EffectColor", effectColor);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Ray ray = new Ray(collision.contacts[0].point, Vector3.down);
        Vector3 position;
        Quaternion rotation = Quaternion.LookRotation(Vector3.up);
        if( Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")))
        {
            position = hit.point;
        }
        else
        {
            position = collision.contacts[0].point;
        }
        position += new Vector3(0, 0.1f, 0);

        GameObject obj = Instantiate(explosionPrefab, position, rotation);
        ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>();
        foreach(var ps in pss)
        {
            var main = ps.main;
            main.duration = effectDuration;            
        }
        pss[0].Play();

        Destroy(this.gameObject);
    }
}
