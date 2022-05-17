using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    class Test
    {
        void Test1()
        {
            Human humanInstance = null;
            humanInstance = new Human();    // Human클래스의 인스턴스를 만들었다.            
            humanInstance.healthPoint = 200;
        }
    }
}
