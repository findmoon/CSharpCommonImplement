using System;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;

namespace GCCollectIntExample
{
    class MyGCCollectClass
    {
        private const long maxGarbage = 1000;

        public static void TestMethod()
        {
            MyGCCollectClass myGCCol = new MyGCCollectClass();

            // 系统垃圾收集器当前支持生成的最大数量
            Debug.WriteLine("垃圾收集器支持生成的最大数量 {0}", GC.MaxGeneration);

            myGCCol.MakeSomeGarbage();

            // myGCCol对象存储在哪个 垃圾收集器生成 中 which generation. 
            Debug.WriteLine("对象所在的垃圾收集器号(generation number): {0}", GC.GetGeneration(myGCCol));

            // 当前在托管内存中分配的字节数的最佳可用近似值。
            // Determine the best available approximation of the number
            // of bytes currently allocated in managed memory.
            Debug.WriteLine("(.NET托管对象可用的)Total Memory: {0}", GC.GetTotalMemory(false));

            // 在0号垃圾收集器上执行收集
            // Perform a collection of generation 0 only.
            GC.Collect(0);

            // 再次查看 myGCCol对象位于哪个 垃圾收集器生成 中
            // Determine which generation myGCCol object is stored in.
            Debug.WriteLine("对象所在的垃圾收集器号(generation number): {0}", GC.GetGeneration(myGCCol));

            Debug.WriteLine("(.NET托管对象可用的)Total Memory: {0}", GC.GetTotalMemory(false));

            // GC.Collect(number)   Forces an immediate garbage collection from generation 0 through a specified generation.
            // Perform a collection of all generations up to and including 2.
            // 执行从0到指定垃圾收集器号的所有的垃圾收集
            GC.Collect(2);

            // 再次查看 myGCCol对象位于哪个 垃圾收集器生成 中
            Debug.WriteLine("对象所在的垃圾收集器号(generation number): {0}", GC.GetGeneration(myGCCol));
            Debug.WriteLine("(.NET托管对象可用的)Total Memory: {0}", GC.GetTotalMemory(false));
            //Console.Read();
        }

        void MakeSomeGarbage()
        {
            Version vt;

            for (int i = 0; i < maxGarbage; i++)
            {
                // Create objects and release them to fill up memory
                // with unused objects.
                vt = new Version();
            }
        }
    }
}