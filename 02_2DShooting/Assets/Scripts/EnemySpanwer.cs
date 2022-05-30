using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    public GameObject enemy = null;
    public float spawnInterval = 1.0f;
    public float randomRange = 8.0f;

    WaitForSeconds waitSecond = null;

    private void Start()
    {
        waitSecond = new WaitForSeconds(spawnInterval);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            yield return waitSecond;
            GameObject obj = Instantiate(enemy);
            obj.transform.position = this.transform.position;
            obj.transform.Translate(Vector3.up * Random.Range(0.0f, randomRange));
        }
    }
}
