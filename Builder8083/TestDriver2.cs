using System;

namespace ConsoleApp2
{
    class TestDriver2 : ITest
    {
        private Test1 code1;
        private Test2 code2;

        public TestDriver2()
        {
            code1 = new Test1();
            code2 = new Test2();

        }
        public static ITest create()
        {
            return new TestDriver2();
        }
        public bool test()
        {
            if (code1.minus(4, 2) == 3 && code2.compare(2,2) != false)
                return true;
            return false;

        }
        static void Main(string[] args)
        {
            ITest t = TestDriver2.create();
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
