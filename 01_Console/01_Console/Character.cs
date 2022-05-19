using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    // 추상화 : 공통적 부분을 뽑아 일반화 하는 것
    class Character
    {
        protected int healthPoint = 100;
        protected int healthPointMax = 100;
        protected int strength = 10;
        protected int dexterity = 10;        
        public string name = "이름";

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
            get => strength;
        }

        public int Dexterity
        {
            get => dexterity; 
        }

        // 생성자(Constructor)
        public Character()
        {
            strength = 10;
            dexterity = 10;
            name = "새 캐릭터";
        }

        // 부모 클래스로 된 변수는 자식 클래스도 담을 수 있다.
        public virtual void Attack(Character attackTarget)
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
            if (HealthPoint <= 0)     // 비교를 할때는 최대한 큰 범위로 비교를 해야한다.(나중에 실수나 수정할 일이 대체로 줄어든다)
            {
                Console.WriteLine($"{name}은(는) 죽었다.");
            }
            PrintStatus();
        }

        public virtual void PrintStatus()
        {
            Console.WriteLine($"┌ {name,5} 스테이터스 ───────────────────┐");
            Console.WriteLine($"│   HP : {healthPoint,5}                           │");
            Console.WriteLine($"│   힘 : {strength,5}                           │");
            Console.WriteLine($"│ 민첩 : {dexterity,5}                           │");
            Console.WriteLine($"└────────────────────────────────────────┘");
        }
    }
}
