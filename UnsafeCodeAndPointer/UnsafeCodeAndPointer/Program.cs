using System;
using System.Diagnostics;

namespace UnsafeCodeAndPointer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            unsafe
            {
                int myInt;
                int* myIntPtr=&myInt;

                // 间接寻址运算符
                *myIntPtr = 11;

                Console.WriteLine($"myInt值：{myInt}");
                Console.WriteLine($"*myIntPtr取值：{*myIntPtr}");
                Console.WriteLine($"myInt地址：{(uint)myIntPtr}");

                //var num = 10;
                //int* pNum = &num;
                //Debug.WriteLine((int)pNum);
            }

            Console.ReadKey();
        }
    }
}