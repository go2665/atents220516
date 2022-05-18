using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    class Orc
    {
        // 데이터
        // HP 있어야 함
        // 그외 스텟은 원하는대로

        // 기능
        // 공격할 수 있다. Attack
        // 피해를 입을 수 있다. TakeDamage
        // 상태 출력 PrintStatus

        // 데이터 -> 맴버 변수(필드)로 표현
        private int healthPoint = 100;
        private int healthPointMax = 100;
        private int strength = 10;
        private int dexterity = 10;
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
                healthPoint = value;           
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
        }

        // 생성자(Constructor) : 클래스이름과 같은 이름이어야 한다.
        public Orc()
        {
            name = "새 오크";
            strength = 30;
            dexterity = 10;
        }

        // 기능 -> 맴버 함수(매서드)로 표현
        public void Attack(Human attackTarget)
        {
            // 공격할 수 있다.
            Console.WriteLine($"{name}은(는) 인간 {attackTarget.name}를 공격합니다.");
            Console.WriteLine($"{name}은(는) {strength} 만큼 피해를 줍니다.");
            attackTarget.TakeDamage((int)(strength * 1.2f));  // 20% 만큼 더 데미지를 주고 싶다. 다만 소수점 아래는 버려진다.
        }

        public void Attack(Orc attackTarget)
        {
            // 공격할 수 있다.
            Console.WriteLine($"{name}은(는) 오크 {attackTarget.name}를 공격합니다.");
            Console.WriteLine($"{name}은(는) {strength} 만큼 피해를 줍니다.");
            attackTarget.TakeDamage((int)(strength * 1.2f));
        }

        public void TakeDamage(int damage)
        {
            // 데미지를 받을 수 있다.
            Console.WriteLine($"{name}은(는) {damage} 만큼 피해를 입었습니다.");
            HealthPoint -= damage;
            if (HealthPoint <= 0)     // 비교를 할때는 최대한 큰 범위로 비교를 해야한다.(나중에 실수나 수정할 일이 대체로 줄어든다)
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
            Console.WriteLine($"└────────────────────────────────────────┘");
        }
    }
}
