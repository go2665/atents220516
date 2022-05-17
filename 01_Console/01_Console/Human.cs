using System;
using System.Collections.Generic;
using System.Text;

// 접근제한자(Access Modifier, 액세스 한정자)
// public  : (공용.) 누구든지 다 사용 가능. 다른 클래스와 데이터를 주고 받을 일이있을 때 사용. 최소한으로 만들어야 함.
// private : (개인의, 사적인.) 나만 사용 가능


namespace _01_Console
{
    class Human
    {
        // 데이터 -> 맴버 변수(필드)로 표현
        private int healthPoint = 100;
        private int strength = 10;
        private int dexterity = 10;
        private int wisdom = 10;

        // 프로퍼티

        // 기능 -> 맴버 함수(매서드)로 표현
        public void Attack()
        {
            // 공격할 수 있다.
        }

        public void TakeDamage()
        {
            // 데미지를 받을 수 있다.
        }
       
    }
}
