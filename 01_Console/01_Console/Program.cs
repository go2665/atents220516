using System;

namespace _01_Console
{
    class Program
    {
        /// <summary>
        /// 이 프로그램의 실행 시작 지점
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.Write("당신의 이름을 입력해 주세요 : ");
            string name = Console.ReadLine();
            Player player = new Player(name);

            do
            {
                player.RestStatus();
                player.PrintStatus();                
            }
            while ( !Util.ChoiceYesNo("이대로 진행할까요?") );    // do-while : 일단 실행하고 조건을 확인하는 반복문

            // 비교 연산자
            // ==(같다), !=(다르다), < , > , <=, >=

            Orc enemy = new Orc();
            Console.WriteLine($"{enemy.name}이 나타났다.");
            enemy.PrintStatus();

            Console.WriteLine("\n\n----------------------전투 시작----------------------\n\n");

            while (true)   
            {
                player.Attack(enemy);
                if (enemy.HealthPoint <= 0)
                {
                    Console.WriteLine("\n\n승리!\n\n");
                    break;
                }
                enemy.Attack(player);
                if (player.HealthPoint <= 0)
                {
                    Console.WriteLine("\n\n패배.....\n\n");
                    break;
                }
                Console.WriteLine("\n\n");
            }
        }

        static void Test()
        {
            //int i = 10;
            Test aaa = new Test();    // Test클래스의 인스턴스를 만들어서, aaa라는 Test 타입 변수에 저장했다.

            //aaa.Test1_VariableFunctionContol();
            //aaa.Test2_ClassTest();
            //aaa.Test3_ClassInstance();            
            //aaa.Test4_HumanVSOrc();
            aaa.Test5_Player();
        }
    }
}
