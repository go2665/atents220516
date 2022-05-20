using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    class Util
    {
        /// <summary>
        /// 양자택일한 결과를 리턴하는 함수
        /// </summary>
        /// <param name="question">양자택일용 질문</param>
        /// <returns>yes면 true, 아니면 false</returns>
        static public bool ChoiceYesNo(string question) // static은 같은 종류의 클래스가 완전히 공유하는 것
        {
            bool result = false;

            Console.Write($"{question} (yes/no) : ");   // 질문 출력하고
            string answer = Console.ReadLine();
            if( answer == "yes" || answer == "Yes" || answer == "y" || answer == "Y" )  // yes 4종류일 때만 true 리턴
            {
                result = true;
            }

            //switch(answer)
            //{
            //    case "yes":
            //    case "Yes":
            //    case "y":
            //    case "Y":
            //        result = true;
            //        break;
            //    default:
            //        break;
            //}

            return result;
        }

        public const int WRONG_OPTION = 0; // 상수(항상 같은 수)
        static public int Choice123(string op1, string op2, string op3, string question = "번호를 선택해 주세요")
        {
            int result = WRONG_OPTION;

            Console.WriteLine($"1. {op1}     2. {op2}     3. {op3}");
            Console.Write($"{question} : ");

            string number = Console.ReadLine();
            int.TryParse(number, out result);   // 우선 문자열을 숫자로 변환 시도. 실패시 result에는 0이 들어간다.

            if( result > 3 )
            {
                result = WRONG_OPTION;
            }

            return result;
        }

        //static public int ChiceSelection(params string[] ops)
        //{
        //    ops.Length;
        //    ops[0]
        //}
    }
}
