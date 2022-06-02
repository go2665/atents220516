using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform[] bgSlots = null;
    public float scrollingSpeed = 2.5f;

    public Transform[] bgStars = null;
    public float starSpeed = 5.0f;
    private SpriteRenderer[] starsRenderer = null;

    public Transform bgPlanet = null;
    public float planetSpeed = 10.0f;

    const float BG_WIDTH = 13.6f;

    private void Awake()
    {
        starsRenderer = new SpriteRenderer[bgStars.Length];
        for(int i=0; i<bgStars.Length; i++)
        {
            starsRenderer[i] = bgStars[i].GetComponent<SpriteRenderer>();   // 같은 인덱스로 매칭시키기
        }
    }

    private void Update()
    {
        float minusX = transform.position.x - BG_WIDTH; // 백그라운드의 x위치에서 왼쪽으로 BG_WIDTH(그림 한장의 폭) 만큼 이동한 위치
        foreach (Transform bgSlot in bgSlots)
        {
            bgSlot.Translate(-transform.right * scrollingSpeed * Time.deltaTime);
            if (bgSlot.position.x < minusX)
            {
                // 충분히 왼쪽으로 이동한 위치
                //Debug.Log("충분히 왼쪽이다.");
                bgSlot.Translate(transform.right * BG_WIDTH * 3.0f);    //오른쪽으로 BG_WIDTH의 3배 만큼 이동
            }
        }

        for(int i=0; i<bgStars.Length; i++)
        {
            bgStars[i].transform.Translate(-transform.right * starSpeed * Time.deltaTime);
            if( bgStars[i].position.x < minusX)
            {
                bgStars[i].transform.Translate(transform.right * BG_WIDTH * 3.0f);
                int rand = Random.Range(0, 4);  // 0~3을 랜덤으로 구하기
                // 0x01 : 16진수 1 ( 0x00_00_00_01 )
                // 0b_01 : 2진수 1 ( 0b_0000_0000_0000_0000_0000_0000_0000_0001)
                // rand의 2진수 결과 = 0일때 00, 1일때 01, 2일때 10, 3일때 11
                // & 연산자 : 양변이 각 자리수별로 둘 다 1일 때 1. 특정 자리수가 1로 되어있는지 확인할 때 좋음.
                                
                starsRenderer[i].flipX = ((rand & 0b_01) != 0);
                starsRenderer[i].flipY = ((rand & 0b_10) != 0);
                // 58~65번 까지의 코드가 56번 라인과 결과가 똑같음.
                //if ((rand & 0b_10) != 0)  // rand의 제일 오른쪽에서 두번째 비트가 1로 되어있는지 아닌지 확인(1로 되어있으면 결괴는 true. 아니면 false)
                //{
                //    starsRenderer[i].flipY = true;
                //}
                //else
                //{
                //    starsRenderer[i].flipY = false;
                //}
            }
        }

        bgPlanet.Translate(-transform.right * planetSpeed * Time.deltaTime);
        if(bgPlanet.position.x < minusX)
        {
            // 행성의 현재 위치에서 오른쪽으로 BG_WIDTH의 3배~5배 사이의 위치만큼 이동
            Vector3 newPos = bgPlanet.position + transform.right * (BG_WIDTH * 3.0f + Random.Range(0.0f, BG_WIDTH * 10));
            newPos.y = transform.position.y + Random.Range(2.0f, 4.5f); // 높이도 랜덤으로 설정 (2~4.5)
            bgPlanet.position = newPos;
        }
    }
}
