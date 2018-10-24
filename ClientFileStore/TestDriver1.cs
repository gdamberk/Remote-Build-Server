using System;

namespace ConsoleApp1
{
    class TestDriver1 : ITest
    {
        private CodeToTest1 code1;
        private CodeToTest2 code2;

        public TestDriver1()
        {
            code1 = new CodeToTest1();
            code2 = new CodeToTest2();

        }
        public static ITest create()
        {
            return new TestDriver1();
        }
        public bool test()
        {
            if (code1.add(1, 2) == 3 && code2.multiply(3, 4) == 12)
                return true;
            return false;

        }
        
        static void Main(string[] args)
        {
            ITest t = TestDriver1.create();
            //TestDriver1 t = new TestDriver1();
            Console.Write(t.test());
            Console.Write("\n\n");
        }
    }

    internal interface ITest
    {
        bool test();
    }
}
