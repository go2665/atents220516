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
