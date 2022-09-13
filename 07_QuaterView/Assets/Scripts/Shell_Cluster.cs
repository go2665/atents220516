using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shell_Cluster : Shell
{
    Shell_Subminuation[] subminuations;     // 자탄들
    ShellData_Cluster data_cluster;         // 데이터 캐스팅 한 것 미리 저장

    protected override void Awake()
    {
        base.Awake();

        // 자탄들을 전부 찾아 놓는다.
        subminuations = GetComponentsInChildren<Shell_Subminuation>();
        foreach (var sub in subminuations)
        {
            sub.gameObject.SetActive(false);        // 모든 자탄의 Update 함수를 실행시키지 않는다.
        }

        data_cluster = data as ShellData_Cluster;   // 캐스팅을 통해 클러스터용 데이터를 미리 저장해 놓는다.
    }

    // 첫번째 업데이트가 실행되기 직전에 호출
    protected override void Start()
    {
        base.Start();
        StartCoroutine(TimeOut());  // 자폭을 위해 코루틴 실행
    }

    private void FixedUpdate()
    {
        rigid.AddForce(Vector3.up * data_cluster.upPower);              // 계속 위로 올라가게 만든다.
        rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));    // 진행방향으로 바라보게 만든다.
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        ActiveSubminuation();               // 자탄 활성화
        base.OnCollisionEnter(collision);
    }

    /// <summary>
    /// 자폭용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(data_cluster.lifeTime); // 클러스트탄이 자폭할 시간이 지나면

        data.Explosion(transform.position, Vector3.up); // 폭팔 이팩트 만들고
        ActiveSubminuation();       // 자탄들 활성화

        Destroy(this.gameObject);   // 게임 오브젝트 삭제
    }

    /// <summary>
    /// 자탄 활성화 함수
    /// </summary>
    private void ActiveSubminuation()
    {
        foreach (var sub in subminuations)  // 모든 자탄들을
        {
            sub.gameObject.SetActive(true); // 활성화 시킴(Update 함수들이 실행 됨.(다음 프레임부터))
            sub.transform.parent = null;    // 부모 제거하기
        }
    }
}
