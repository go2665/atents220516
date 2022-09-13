using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shell_Cluster : Shell
{
    Shell_Subminuation[] subminuations;
    ShellData_Cluster data_cluster;

    protected override void Awake()
    {
        base.Awake();

        subminuations = GetComponentsInChildren<Shell_Subminuation>();
        foreach (var sub in subminuations)
        {
            sub.gameObject.SetActive(false);    // 더 이상 Update 함수가 실행되지 않는다.
        }

        data_cluster = data as ShellData_Cluster;
    }

    // 첫번째 업데이트가 실행되기 직전에 호출
    protected override void Start()
    {
        base.Start();
        StartCoroutine(TimeOut());
    }

    private void FixedUpdate()
    {
        rigid.AddForce(Vector3.up * data_cluster.upPower);
        rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        ActiveSubminuation();
        base.OnCollisionEnter(collision);
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(data_cluster.lifeTime);

        data.Explosion(transform.position, Vector3.up);
        ActiveSubminuation();

        Destroy(this.gameObject);
    }

    private void ActiveSubminuation()
    {
        foreach (var sub in subminuations)
        {
            sub.gameObject.SetActive(true); // Update 함수들이 실행 됨.(다음 프레임부터)
            sub.transform.parent = null;
        }
    }
}
