using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    class Orc : Character
    {
        Random rand = new Random(DateTime.Now.Millisecond); // 현재 시간을 랜덤시드로 설정

        // 데이터 -> 맴버 변수(필드)로 표현
        // 프로퍼티 : 특수한 함수(매서드)
        // 생성자(Constructor) : 클래스이름과 같은 이름이어야 한다.
        public Orc()
        {
            name = "새 오크";
            strength = rand.Next(10,31);   // 10~30 중 하나가 나온다.
            dexterity = 10;
        }

        // 기능 -> 맴버 함수(매서드)로 표현
        public override void Attack(Character attackTarget)
        {
            // 공격할 수 있다.
            Console.WriteLine($"{name}은(는) {attackTarget.name}를 공격합니다.");            
            
            int damage = strength + (int)(strength * (rand.NextDouble() * 0.2));    // 최대 20% 만큼의 랜덤 데미지를 주고 싶다. 다만 소수점 아래는 버려진다.
            Console.WriteLine($"{name}은(는) {damage} 만큼 피해를 줍니다.");
            attackTarget.TakeDamage(damage);  
            //base.Attack(attackTarget);
            //attackTarget.TakeDamage((int)(strength * 0.2f));
        }
    }
}
