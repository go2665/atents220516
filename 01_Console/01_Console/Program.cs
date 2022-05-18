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
            int i = 10;
            Test aaa = new Test();    // Test클래스의 인스턴스를 만들어서, aaa라는 Test 타입 변수에 저장했다.

            //aaa.Test1_VariableFunctionContol();
            aaa.Test2_ClassTest();
        }
    }
}
