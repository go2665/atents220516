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

            return result;
        }
    }
}
