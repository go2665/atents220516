using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequest : MonoBehaviour
{
    readonly string url = "http://go2665.dothome.co.kr/HTTP_Data/TestData.txt";

    void Start()
    {
        StartCoroutine(GetWebData());
    }


    IEnumerator GetWebData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url); // 이 url에 데이터 요청
        yield return www.SendWebRequest();              // 요청한 데이터가 도착할 때까지 대기

        if( www.result != UnityWebRequest.Result.Success )
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log($"Data get : {www.downloadHandler.text}");
            TestData test = JsonUtility.FromJson<TestData>(www.downloadHandler.text);
            int i = 0;
        }
    }
}
