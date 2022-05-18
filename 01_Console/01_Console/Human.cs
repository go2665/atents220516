using System;
using System.Collections.Generic;
using System.Text;

// 접근제한자(Access Modifier, 액세스 한정자)
// public  : (공용.) 누구든지 다 사용 가능. 다른 클래스와 데이터를 주고 받을 일이있을 때 사용. 최소한으로 만들어야 함.
// private : (개인의, 사적인.) 나만 사용 가능


namespace _01_Console
{
    // 클래스 : 특정한 유형의 물체를 코드로 표현한 것(붕어빵틀)
    class Human
    {
        // 데이터 -> 맴버 변수(필드)로 표현
        private int healthPoint = 100;
        private int healthPointMax = 100;
        private int strength = 10;
        private int dexterity = 10;
        private int wisdom = 10;

        // 프로퍼티 : 특수한 함수(매서드)
        public int HealthPoint
        {
            get // HealthPoint를 읽을 때 실행되는 함수.
            {
                return healthPoint;
            }
            set // HealthPoint 프로퍼티에 값을 넣을 때 실행되는 함수. 설정되는 값은 value라는 키워드에 들어있다.
            {
                //strength = value;
                healthPoint = value;


                //if(healthPoint > healthPointMax)
                //{
                //    healthPoint = healthPointMax;
                //}
                //else if( healthPoint < 0 )
                //{
                //    healthPoint = 0;
                //}                
                healthPoint = Math.Clamp(healthPoint, 0, healthPointMax);
            }
        }

        public int Strength
        {
            get
            {
                return strength;
            }
        }

        public int Dexterity
        {
            get => dexterity;   // 아래 주석과 같은 내용
            //get
            //{
            //    return dexterity;
            //}
        }

        public int Wisdom
        {
            get => wisdom;
        }

        // 생성자(Constructor)
        public Human()
        {
            strength = 10;
            dexterity = 10;
            wisdom = 10;
        }
        public Human(int str, int dex, int wis)
        {
            strength = str;
            dexterity = dex;
            wisdom = wis;
        }
        

        // 기능 -> 맴버 함수(매서드)로 표현
        public void Attack()
        {
            // 공격할 수 있다.
        }

        public void TakeDamage()
        {
            // 데미지를 받을 수 있다.
        }

        public void PrintStatus()
        {
            Console.WriteLine($"┌ 스테이터스 ────────────────────────────┐");
            Console.WriteLine($"│   HP : {healthPoint,5}                           │");
            Console.WriteLine($"│   힘 : {strength,5}                           │");
            Console.WriteLine($"│ 민첩 : {dexterity,5}                           │");
            Console.WriteLine($"│ 지능 : {wisdom,5}                           │");
            Console.WriteLine($"└────────────────────────────────────────┘");
        }
    }
}
