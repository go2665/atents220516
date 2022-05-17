using System;

namespace _01_Console
{
    class Program
    {
        static void Main(string[] args)
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

            string userName;
            string userLevel;
            string userExp;
            string userStr;
            string userDex;
            string userInt;

            Console.Write("이름을 입력해 주세요 : ");
            userName = Console.ReadLine();
            Console.Write("레벨을 입력해 주세요 : ");
            userLevel = Console.ReadLine();
            Console.Write("경험치를 입력해 주세요 : ");
            userExp = Console.ReadLine();
            Console.Write("힘을 입력해 주세요 : ");
            userStr = Console.ReadLine();
            Console.Write("민첩을 입력해 주세요 : ");
            userDex = Console.ReadLine();
            Console.Write("지능을 입력해 주세요 : ");
            userInt = Console.ReadLine();

            Console.WriteLine(
                $"나의 이름은 {userName}이고 레벨은 {userLevel}이며 경험치는 {userExp}%이다.\n힘: {userStr}, 민첩: {userDex}, 지능: {userInt}");
        }
    }
}
