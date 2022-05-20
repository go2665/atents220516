using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    class Test
    {
        public void Test5_Player()
        {
            // 컴퓨터는 시킨것만 한다.(절대적)
            // 프로그래머가 설정하지 않은 상황에 컴퓨터가 도달하면 죽는다.(가벼우면 프로그램만 죽음. 무거우면 컴퓨터 자체가 다운(블루스크린))
            // 프로그래머가 설정하지 않은 상황 = Exception
            // throw new NotImplementedException();    // 어디가 문제인지 쉽게 알아보기 위해 설정한 것

            Player player = new Player("너굴맨");
            player.PrintStatus();
            player.RestStatus();
            player.PrintStatus();
        }

        public void Test4_HumanVSOrc()
        {
            Human human = new Human(30, 20, 20, "인간A");
            Orc orc = new Orc();

            // 산술연산자 : + - * /

            // 논리 연산자 : 참이냐 거짓이냐를 계산하는 연산자.
            // && (and, 앤드, 그리고, 논리곱) : && 양쪽의 값이 둘 다 참이면 참, 아니면 거짓 ( true && true == true,  true && false == false, false && true == false, false && false == false )
            // || (or, 오어, 또는, 논리합) : || 양쪽의 값이 하나라도 참이면 참, 둘다 거짓이면 거짓 ( true || true == true, true || false == true, false || true = true, false || false == false)
            while (true)    // 둘 중 한명이라도 hp가 0보다 작거나 같이지면 while 종료
            {
                human.Attack(orc);
                if (orc.HealthPoint <= 0)
                {
                    break;
                }
                orc.Attack(human);
                if (human.HealthPoint <= 0)
                {
                    break;
                }
                Console.WriteLine("\n\n");
            }
        }

        public void Test3_ClassInstance()
        {
            Human human1 = new Human(20,10,10, "개구리");
            Human human2 = new Human(15,15,15, "너구리");

            human1.HealthPoint = 50;
            Console.WriteLine($"Human1");
            human1.PrintStatus();
            Console.WriteLine($"Human2");
            human2.PrintStatus();
            human1.Attack(human2);
            human1.Attack(human2);
            human1.Attack(human2);
            human1.Attack(human2);
            human1.Attack(human2);
            human1.Attack(human2);
        }

        public void Test2_ClassTest()
        {
            // 주석 설정 단축키
            // - 주석잡을 영역을 선택하고 Ctrl + K + C 를 누르면 해당 영역이 주석처리가 됨
            // - 주석을 풀고 싶을 때는 Ctrl + K + U

            Human humanInstance = null;
            humanInstance = new Human();    // Human클래스의 인스턴스를 만들었다.

            //humanInstance.healthPoint = 200;
            //Console.WriteLine($"Health Point : {humanInstance.healthPoint}");

            humanInstance.HealthPoint = 90;    // HealthPoint 프로퍼티를 이용해 값을 변경했다.
            Console.WriteLine($"Health Point : {humanInstance.HealthPoint}");

            // 힘과 민첩과 지능을 출력하기
            Console.WriteLine($"힘 : {humanInstance.Strength}");
            Console.WriteLine($"민첩 : {humanInstance.Dexterity}");
            Console.WriteLine($"지혜 : {humanInstance.Wisdom}\n");
            
            // Human이 힐을 받아서 HealthPoint가 50 증가했다.
            Console.WriteLine("Human이 힐을 받아서 HealthPoint가 50 증가했다.");   // 최대 HP는 100이다.
            humanInstance.HealthPoint += 50;            // hp = 140. humanInstance.HealthPoint = humanInstance.HealthPoint + 50;            
            Console.WriteLine($"Health Point : {humanInstance.HealthPoint}");
            // HealthPoint의 set 프로퍼티 때문에 이제 필요없음
            //if (humanInstance.HealthPoint > 100)      // humanInstance.HealthPoint가 100을 초과하면 
            //{
            //    humanInstance.HealthPoint = 100;
            //}        

            // Human이 공격을 당해서 HeathPoint가 200 감소했다.
            Console.WriteLine("Human이 공격을 당해서 HeathPoint가 200 감소했다.");
            humanInstance.HealthPoint -= 200;
            Console.WriteLine($"Health Point : {humanInstance.HealthPoint}");

        }

        public void Test1_VariableFunctionContol()
        {
            Console.WriteLine("Hello World!");      // Console.WriteLine은 파라메터로 받은 문자열을 출력하는 함수
            Console.WriteLine("헬로 월드 고병조");

            // 변수 -> 변하는 숫자
            // 변수의 종류 -> data type
            // int, float, bool, string
            // int 정수 -> 소수점이 없는 숫자(0, 1, 5, -7, 10)
            // float 실수 -> 소수점이 있는 숫자( 0.0, 3.14, -55.9 )
            // bool 불 -> true 아니면 false만 가지는 변수
            // string 문자열 -> ""안에 있는 문장을 저장하는 변수( "개구리", "Hello World!" )

            // 변수의 이름은 알파벳으로 시작한다. _와 숫자를 붙일 수 있다.
            // 변수의 이름은 단어의 의미만으로도 알아볼 수 있도록 작성하는 것을 권장.

            // 변수 작성법
            // (변수의 종류) (변수의 이름) = (초기값);
            int strength;
            int dex = 20;
            int intelligence = 10;

            float exp1 = 35;    // 운이 좋았다.... C#이 int타입을 자동으로 float으로 변경한 경우
            float exp2 = 35f;   // 정답
            float exp3 = 35.0f; // 정답

            bool a = true;
            bool b = false;

            string myName = "고병조";

            // 변수를 만드는 것을 "선언"이라고 함.
            int level = 5;
            float exp = 95.12345f;
            string name = "인간";

            // "나는 인간이고 레벨은 5고 경험치는 95%이다."

            // Console.WriteLine("나는 인간이고 레벨은 5고 경험치는 95%이다.");  // 무식한 방법

            // 가능은 하지만 안좋은 방법(메모리를 낭비하는 방법)            
            Console.WriteLine("나는 " + name + "이고 레벨은 " + level + "고 경험치는 " + exp + "%이다.");

            // 적당히 쓸만한 방법
            Console.WriteLine("나는 {0}이고 레벨은 {1}고 경험치는 {2}%이다", name, level, exp);

            // 가장 최신 방법
            Console.WriteLine($"나는 {name}이고 레벨은 {level}고 경험치는 {exp:f2}%이다.");


            // 스코프(scope)
            // 변수가 살아있는 범위
            //  int i = 0;
            //  {
            //      int j = 20;
            //  }
            //  j = 30; //j의 스코프 밖이라 접근이 불가능하다. 에러 발생

            // 개행문자(줄바꿈용 문자)
            // "개굴개굴 개구리 노래를 한다."
            // "아들 손자 며느리 다 모여서."
            string song = "개굴개굴 개구리 노래를 한다.\n아들 손자 며느리 다 모여서.";
            Console.WriteLine(song);

            //Console.Write("입력해 주세요 : ");
            //string inputline = Console.ReadLine();  //입력 받기
            //Console.WriteLine(inputline);

            string userName = "고";
            int userLevel = 15;
            float userExp = 31.0f;
            int userStr = 15;
            int userDex = 5;
            int userInt = 10;

            //Console.Write("이름을 입력해 주세요 : ");
            //userName = Console.ReadLine();
            //Console.Write("레벨을 입력해 주세요 : ");
            //userLevel = int.Parse(Console.ReadLine());
            //Console.Write("경험치를 입력해 주세요 : ");
            //userExp = float.Parse(Console.ReadLine());
            //Console.Write("힘을 입력해 주세요 : ");
            //userStr = int.Parse(Console.ReadLine());
            //Console.Write("민첩을 입력해 주세요 : ");
            //userDex = int.Parse(Console.ReadLine());
            //Console.Write("지능을 입력해 주세요 : ");
            //userInt = int.Parse(Console.ReadLine());

            Console.WriteLine(
                $"나의 이름은 {userName}이고 레벨은 {userLevel}이며 경험치는 {userExp}%이다.\n힘: {userStr}, 민첩: {userDex}, 지능: {userInt}");

            // 제어문
            // 조건에 따라 다른 코드를 실행하거나 특정 횟수만큼 반복하도록 하게 하는 문구
            //  조건문 - if elseif else
            //  반복문 - for, while, do-while


            // if문의 기본 형태
            // if( 조건 )
            // {
            //     조건이 맞으면 실행될 코드
            // }

            // 레벨이 10보다 크면 "고랩이다"를 출력. 10보다 작거나 같으면 "저랩이다"를 출력.
            // 레벨이 10보다 크면 "고랩이다"를 출력. 아니면 "저랩이다"를 출력.
            //if ( userLevel > 10 )   // 15 > 10
            //{
            //    //if문은 ()사이의 조건이 맞는 말이면 {} 사이의 코드가 실행됨
            //    Console.WriteLine("고랩이다.");
            //}

            //if ( userLevel <= 10 )
            //{
            //    Console.WriteLine("저랩이다.");
            //}

            // if-else문의 기본 형태
            // if( 조건 )
            // {
            //     조건이 맞으면 실행될 코드
            // }
            // else
            // {
            //     조건이 틀리면 실행될 코드
            // }
            if (userLevel > 10)
            {
                Console.WriteLine("고랩이다.");
            }
            else
            {
                Console.WriteLine("저랩이다.");
            }

            // if-else if문의 기본 형태
            // if( 조건1 )
            // {
            //     조건1이 맞으면 실행될 코드
            // }
            // else if( 조건2 )
            // {
            //     조건2가 맞으면 실행될 코드
            // }
            // else
            // {
            //     모든 조건이 틀리면 실행될 코드
            // }

            // 레벨이 10 이하면 저랩, 11~20 이면 중랩, 21~30 이면 고랩
            if (userLevel > 20)
            {
                Console.WriteLine("고랩");
            }
            else if (userLevel > 10)
            {
                Console.WriteLine("중랩");
            }
            else
            {
                Console.WriteLine("저랩");
            }

            // 반복문 
            // 반복하는 문장. 특정 회수만큼 반복하고 싶을 때 사용. 특정 조건이 참이면 계속 반복되는 문장
            // while, for

            // while (조건)
            // {
            //     조건이 만족하면 반복해서 실행될 코드
            //     코드를 실행한 후 다시 조건을 확인
            // }

            // 레벨업 10번찍기
            int count = 0;
            while (count < 10)
            {
                Console.WriteLine("Level up!!!");
                count++;    //count = count + 1;
            }

            for (int countFor = 0; countFor < 10; countFor++)
            {
                Console.WriteLine("Level up!!!!!");
            }


            /// 실습 2번
            Console.Write("얼마를 랩업할까요? : ");
            string temp = Console.ReadLine();       // 입력 받기
            int levelUpCount = int.Parse(temp);     // 입력 받은 문자열을 숫자로 바꾸기
            PrintStatus(userStr, userDex, userInt); // PrintStatus 함수 호출
            for (int i = 0; i < levelUpCount; i++)       // 입력받은 횟수만큼 반복
            {
                userStr++;              // 1 증가 시키기
                userDex = userDex + 1;  // 1 증가 시키기
                userInt += 1;           // 1 증가 시키기
                Console.WriteLine("Level UP!!!!!!!!");
            }
            PrintStatus(userStr, userDex, userInt); // PrintStatus 함수 호출
        }

        // 함수 : 특정한 기능을 수행하기 위해 코드를 모아 놓은 것
        // 함수의 구성요소
        //  1) 이름 (ex)PrintStatus)
        //  2) 함수 리턴타입 (void는 리턴값이 없는 경우)
        //  3) 파라메터 (이름 뒤에 있는 () 사이에 있는 변수들, 0개 이상 존재)
        //  4) 함수 바디( {} 사이 )

        // 함수 프로토타입 : 리턴타입 함수이름(파라메터), 다른 함수와의 구분에 사용됨
        //                  void PrintStatus(int, int, int);    //아래의 모든 함수들은 다 다른 함수
        //                  void PrintStatus2(int, int, int)
        //                  int PrintStatus(int, int, int)
        //                  void PrintStatus(int, int)
        //                  void PrintStatus(int, int, float)

        void PrintStatus(int str, int dex, int intelligence)
        {
            Console.WriteLine($"┌ 스테이터스 ────────────────────────────┐");
            Console.WriteLine($"│   힘 : {str,5}                           │");
            Console.WriteLine($"│ 민첩 : {dex,5}                           │");
            Console.WriteLine($"│ 지능 : {intelligence,5}                           │");
            Console.WriteLine($"└────────────────────────────────────────┘");
        }
    }
}
