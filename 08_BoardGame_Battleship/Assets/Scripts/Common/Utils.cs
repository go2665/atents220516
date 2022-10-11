using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utils
{
    public static void Shuffle<T>(T[] source)
    {
        for (int i = source.Length - 1; i > -1; i--) // 랜덤으로 선택한 숫자를 오른쪽에 모으면서 셔플
        {
            int randIndex = Random.Range(0, i);
            (source[randIndex], source[i]) = (source[i], source[randIndex]);
        }
        //for (int i = 0; i < temp.Length - 1; i++) // 왼쪽 기준으로 셔플
        //{
        //    int index = Random.Range(i + 1, temp.Length);
        //    (temp[i], temp[index]) = (temp[index], temp[i]);
        //}
    }
}
