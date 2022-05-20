using System;
using System.Collections.Generic;
using System.Text;

// 접근제한자(Access Modifier, 액세스 한정자)
// public  : (공용.) 누구든지 다 사용 가능. 다른 클래스와 데이터를 주고 받을 일이있을 때 사용. 최소한으로 만들어야 함.
// private : (개인의, 사적인.) 나만 사용 가능
// protected : 나랑 내 자식만 본다.


namespace _01_Console
{
    // 클래스 : 특정한 유형의 물체를 코드로 표현한 것(붕어빵틀)
    // 상속 : 다른 클래스의 변수와 함수를 물려받는 것
    class Human : Character
    {
        // 데이터 -> 맴버 변수(필드)로 표현
        protected int wisdom = 10;
        protected Random random = new Random(DateTime.Now.Millisecond);

        // 프로퍼티 : 특수한 함수(매서드)
        public int Wisdom
        {
            get => wisdom;
        }

        // 생성자(Constructor)
        public Human() : base()
        {
            wisdom = 10;
        }

        public Human(int str, int dex, int wis, string newName = "새휴먼")
        {
            strength = str;
            dexterity = dex;
            wisdom = wis;
            name = newName;
        }

        public override void Attack(Character attackTarget)
        {           
            //30% 확률로 스킬로 공격한다.
            if( random.NextDouble() < 0.3 )
            {
                Skill(attackTarget);
            }
            else
            {
                base.Attack(attackTarget);
            }
        }

        void Skill(Character attackTarget)
        {
            Console.WriteLine($"{name}은(는) {attackTarget.name}를 스킬로 공격합니다.");
            Console.WriteLine($"{name}은(는) {Wisdom * 2} 만큼 피해를 줍니다.");
            attackTarget.TakeDamage(Wisdom * 2);
        }

        // 기능 -> 맴버 함수(매서드)로 표현
        public override void PrintStatus()
        {
            // base.PrintStatus();
            Console.WriteLine($"┌ {name,5} 스테이터스 ───────────────────┐");
            Console.WriteLine($"│   HP : {healthPoint,5}                           │");
            Console.WriteLine($"│   힘 : {strength,5}                           │");
            Console.WriteLine($"│ 민첩 : {dexterity,5}                           │");
            Console.WriteLine($"│ 지능 : {wisdom,5}                           │");
            Console.WriteLine($"└────────────────────────────────────────┘");
        }
    }
}
