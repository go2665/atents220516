using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shell_Cluster : Shell
{
    Shell_Subminuation[] subminuations;


    protected override void Awake()
    {
        base.Awake();

        subminuations = GetComponentsInChildren<Shell_Subminuation>();
        foreach (var sub in subminuations)
        {
            sub.gameObject.SetActive(false);    // 더 이상 Update 함수가 실행되지 않는다.
        }
    }

    // 첫번째 업데이트가 실행되기 직전에 호출
    //protected override void Start()
    //{
    //    base.Start();
    //    StartCoroutine(TimeOut());
    //}

    //private void FixedUpdate()
    //{
    //    rigid.AddForce(Vector3.up * upPower);
    //    //Quaternion.LookRotation(rigid.velocity);    //rigid.velocity가 forward가 되는 회전만들기
    //    rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));
    //}

    //IEnumerator TimeOut()
    //{
    //    yield return new WaitForSeconds(lifeTime);

    //    Instantiate(explosionPrefab, transform.position, Quaternion.LookRotation(Vector3.up));
    //    foreach (var sub in subminuations)
    //    {
    //        sub.gameObject.SetActive(true); // Update 함수들이 실행 됨.(다음 프레임부터)
    //        sub.transform.parent = null;            
    //    }

    //    Destroy(this.gameObject);
    //}
}
