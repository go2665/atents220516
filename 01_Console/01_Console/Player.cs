using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    class Player : Human
    {
        // 생성자
        public Player(string _name = "플레이어")
        {
            this.strength = 10;
            this.dexterity = 10;
            this.wisdom = 10;
            this.healthPointMax = 10;
            this.healthPoint = 10;
            this.name = _name;
        }

        public void RestStatus()
        {
            this.strength = random.Next(5, 21);    // 5~20 사이의 랜덤값
            this.dexterity = random.Next(5, 21);
            this.wisdom = random.Next(16) + 5;     // 5~20 사이의 랜덤값
            this.healthPointMax = random.Next(100, 251);   // 100~250 사이의 랜덤값
            this.healthPoint = this.healthPointMax;
        }

        public override void Attack(Character attackTarget)
        {
            int op = Util.WRONG_OPTION;
            do
            {
                op = Util.Choice123("근접공격", "원거리공격", "마법공격", "어떻게 공격할까요?");
            }
            while (op == Util.WRONG_OPTION);

            int damage = 0;
            switch(op)
            {
                case 1: // 근접 공격을 선택했다.
                    {
                        damage = strength;
                    }
                    break;
                case 2: // 원거리 공격을 선택했다.
                    {
                        damage = dexterity;
                    }
                    break;
                case 3: // 마법 공격을 선택했다.
                    {
                        damage = wisdom;
                    }
                    break;
                default:// 절대로 들어오면 안되는 것
                    break;
            }
            attackTarget.TakeDamage(damage);
        }

        public override void PrintStatus()
        {
            // base.PrintStatus();
            Console.WriteLine($"┌ {name,5} 스테이터스 ───────────────────┐");
            Console.WriteLine($"│   HP : {healthPoint,3}/{healthPointMax,3}                         │");
            Console.WriteLine($"│   힘 : {strength,5}                           │");
            Console.WriteLine($"│ 민첩 : {dexterity,5}                           │");
            Console.WriteLine($"│ 지능 : {wisdom,5}                           │");
            Console.WriteLine($"└────────────────────────────────────────┘");
        }
    }
}
