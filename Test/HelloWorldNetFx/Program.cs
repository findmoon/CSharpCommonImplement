using System;

namespace HelloWorldNetFx
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 10;
            new MyTest();
            Console.WriteLine("Hello, World");
            Console.Read();

            var m = 0b101;

            var d1 = 3D;
            var d2 = 5.1;
            var d3 = 3.934_001d;

            float f1 = 3_000.5F;
            var f2 = 5.4f;

            decimal myMoney1 = 1_000.5m;
            var myMoney2 = 200.75M;
        }

        void Main()
        {
            Console.WriteLine("Hello, World");
        }
    }
    class MyTest
    {
        void Main(string[] args)
        {
            Console.WriteLine("Hello, World");
            Console.Read();
        }
    }
    enum MyTestEnum
    {

    }

    delegate int MyDelegate(int a,string b);


}
