using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MonsterSpanwer : MonoBehaviour
{
    public GameObject monster;
    public int maxSpawn = 2;
    public float spawnRange = 5.0f;

    [Header("Debug Info")]
    public bool showSpawnRange = true;

    private void Update()
    {
        if( transform.childCount <= maxSpawn )
        {
            GameObject obj = Instantiate(monster, this.transform);
            Vector2 randPos = Random.insideUnitCircle * spawnRange; // 랜덤한 위치에 생성
            obj.transform.localPosition = randPos;
            obj.transform.localRotation = Quaternion.identity;

            obj.GetComponent<Enemy>().patrolRoute = transform.GetChild(0);  // 기본 패트롤 루트 지정
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(showSpawnRange)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, spawnRange);
        }
    }
#endif
}
