using System;
using System.Runtime.InteropServices;

namespace HelloWorldNetFx
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 10;
            new MyTest();
            Console.WriteLine("Hello, World");
            Console.ReadKey();

            var m = 0b101;

            var d1 = 3D;
            var d2 = 5.1;
            var d3 = 3.934_001d;

            float f1 = 3_000.5F;
            var f2 = 5.4f;

            decimal myMoney1 = 1_000.5m;
            var myMoney2 = 200.75M;

            Console.WriteLine(getMemory(new object()));

  

            Console.ReadKey();
        }

        void Main()
        {
            Console.WriteLine("Hello, World");
        }

        // 获取引用类型的内存地址方法
        public static string getMemory(object obj)
        {
            // GCHandle.Alloc(obj, GCHandleType.Pinned) // System.ArgumentException:“Object 包含非基元或非直接复制到本机结构中的数据。”
            GCHandle handle = GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
            IntPtr addr = GCHandle.ToIntPtr(handle);
            return $"0x{addr.ToString("X")}";
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
