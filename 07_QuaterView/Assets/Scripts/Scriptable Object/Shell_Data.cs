using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shell Data", menuName = "Sctipable Object/Shell Data", order = 0)]
public class Shell_Data : ScriptableObject
{
    public float initialSpeed = 20.0f;
    public float coolTime = 1.0f;
    public float damage = 50.0f;

    public GameObject explosionPrefab;

    public virtual void Explosion(Vector3 position, Vector3 normal)
    {
        Instantiate(explosionPrefab, position, Quaternion.LookRotation(normal));
    }
}
