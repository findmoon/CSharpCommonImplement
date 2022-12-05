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
                int* myIntPtr = &myInt;

                // 间接寻址运算符
                *myIntPtr = 11;

                Console.WriteLine($"myInt值：{myInt}");
                Console.WriteLine($"*myIntPtr取值：{*myIntPtr}");
                Console.WriteLine($"myInt地址：{(uint)myIntPtr}");

                //var num = 10;
                //int* pNum = &num;
                //Debug.WriteLine((int)pNum);


                FunctionPointer functionPtr = new FunctionPointer();
                functionPtr.InvokeFuncPtr();
            }

            Console.ReadKey();
        }



        /// <summary>
        /// 使用fixed pin对象，然后使用其指针
        /// </summary>
        void PinObjectUsePointer()
        {
            // Normal pointer to an object.
            int[] a = new int[5] { 10, 20, 30, 40, 50 };
            // Must be in unsafe code to use interior pointers.
            unsafe
            {
                // Must pin object on heap so that it doesn't move while using interior pointers.
                fixed (int* p = &a[0])
                {
                    // p is pinned as well as object, so create another pointer to show incrementing it.
                    int* p2 = p;
                    Console.WriteLine(*p2);
                    // Incrementing p2 bumps the pointer by four bytes due to its type ...
                    p2 += 1;
                    Console.WriteLine(*p2);
                    p2 += 1;
                    Console.WriteLine(*p2);
                    Console.WriteLine("--------");
                    Console.WriteLine(*p);
                    // Dereferencing p and incrementing changes the value of a[0] ...
                    *p += 1;
                    Console.WriteLine(*p);
                    *p += 1;
                    Console.WriteLine(*p);
                }
            }

            Console.WriteLine("--------");
            Console.WriteLine(a[0]);

            /*
            Output:
            10
            20
            30
            --------
            10
            11
            12
            --------
            12
            */
        }
    }


    unsafe class FunctionPointer
    {
        /// <summary>
        /// 调用函数指针
        /// </summary>
        internal void InvokeFuncPtr()
        {
            int product = UnsafeCombine(&localMultiply, 3, 4);
            Console.WriteLine("函数指针调用的结果：" + product);
        }
        static int localMultiply(int x, int y) => x * y;

        public static T Combine<T>(Func<T, T, T> combinator, T left, T right) =>
    combinator(left, right);

        public static T1 Combine2<T,T1>(Func<T, T, T1> combinator, T left, T right) =>
    combinator(left, right);


        public static T UnsafeCombine<T>(delegate*<T, T, T> combinator, T left, T right) =>
            combinator(left, right);

        #region calling convention
        public static T ManagedCombine<T>(delegate* managed<T, T, T> combinator, T left, T right) =>
    combinator(left, right);

        public static T CDeclCombine<T>(delegate* unmanaged[Cdecl]<T, T, T> combinator, T left, T right) =>
            combinator(left, right);
        public static T StdcallCombine<T>(delegate* unmanaged[Stdcall]<T, T, T> combinator, T left, T right) =>
            combinator(left, right);
        public static T FastcallCombine<T>(delegate* unmanaged[Fastcall]<T, T, T> combinator, T left, T right) =>
            combinator(left, right);
        public static T ThiscallCombine<T>(delegate* unmanaged[Thiscall]<T, T, T> combinator, T left, T right) =>
            combinator(left, right);

        public static T UnmanagedCombine<T>(delegate* unmanaged<T, T, T> combinator, T left, T right) =>
            combinator(left, right);
        #endregion
    }
}