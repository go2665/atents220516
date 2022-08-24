using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Spawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int maxSpawn = 1;        // 최대 몬스터 생성 수
    public float spawnDelay = 1.0f; // 몬스터 생성 간격

    public Vector2Int spawnAreaMin; // grid 기준
    public Vector2Int spawnAreaMax;

    int currentSpawn = 0;           // 현재 몬스터 생성 수

    SceneMonsterManager monsterManager;

    private void Start()
    {
        monsterManager = transform.GetComponentInParent<SceneMonsterManager>();
    }

    private void OnDrawGizmos()
    {
        if(monsterManager !=null)
        {
            Vector3 min = monsterManager.GridToWorld(spawnAreaMin) - new Vector2(0.5f,0.5f);
            Vector3 max = monsterManager.GridToWorld(spawnAreaMax) + new Vector2(0.5f, 0.5f);
            Vector3 p2 = new(max.x, min.y);
            Vector3 p3 = new(min.x, max.y);
                        
            Handles.color = Color.red;
            Handles.DrawLine(min, p2, 5.0f);
            Handles.DrawLine(p2, max, 5.0f);
            Handles.DrawLine(max, p3, 5.0f);
            Handles.DrawLine(p3, min, 5.0f);
        }
    }
}
