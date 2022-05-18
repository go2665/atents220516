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
        public string name = "이름";

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

        public Human(int str, int dex, int wis, string newName = "새이름")
        {
            strength = str;
            dexterity = dex;
            wisdom = wis;
            name = newName;
        }        

        // 기능 -> 맴버 함수(매서드)로 표현
        public void Attack(Human attackTarget)
        {
            // 공격할 수 있다.
            Console.WriteLine($"{name}은(는) {attackTarget.name}를 공격합니다.");
            Console.WriteLine($"{name}은(는) {strength} 만큼 피해를 줍니다.");
            attackTarget.TakeDamage(strength);
        }

        public void TakeDamage(int damage)
        {
            // 데미지를 받을 수 있다.
            Console.WriteLine($"{name}은(는) {damage} 만큼 피해를 입었습니다.");            
            HealthPoint -= damage;
            if ( HealthPoint <= 0 )     // 비교를 할때는 최대한 큰 범위로 비교를 해야한다.(나중에 실수나 수정할 일이 대체로 줄어든다)
            {
                Console.WriteLine($"{name}은(는) 죽었다.");
            }
            PrintStatus();
        }

        public void PrintStatus()
        {
            Console.WriteLine($"┌ {name,5} 스테이터스 ───────────────────┐");
            Console.WriteLine($"│   HP : {healthPoint,5}                           │");
            Console.WriteLine($"│   힘 : {strength,5}                           │");
            Console.WriteLine($"│ 민첩 : {dexterity,5}                           │");
            Console.WriteLine($"│ 지능 : {wisdom,5}                           │");
            Console.WriteLine($"└────────────────────────────────────────┘");
        }
    }
}
