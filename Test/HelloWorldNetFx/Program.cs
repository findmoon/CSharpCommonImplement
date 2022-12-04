using System;

namespace HelloWorldNetFx
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new MyTest();
            Console.WriteLine("Hello, World");
            Console.Read();
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
