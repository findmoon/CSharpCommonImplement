**C#中的垃圾回收和GC类保持对象不被内存回收**

[toc]

[GC Class](https://learn.microsoft.com/en-us/dotnet/api/system.gc?view=net-6.0)

[GC.KeepAlive(Object) Method](https://learn.microsoft.com/en-us/dotnet/api/system.gc.keepalive?view=net-6.0#system-gc-keepalive(system-object))


[GC：.net framework中的自动内存管理--part 1 （翻译）](https://www.cnblogs.com/coolkiss/archive/2010/08/27/1810095.html)
[GC：.net framework中的自动内存管理--part 2 （翻译）](https://www.cnblogs.com/coolkiss/archive/2010/08/31/1813665.html#!comments)
[使用finalize/dispose 模式提高GC性能（翻译）](https://www.cnblogs.com/coolkiss/archive/2010/08/23/1806382.html)

[《你不常用的c#之二》:略谈GCHandle](https://www.cnblogs.com/zhaox583132460/p/3402243.html)

[深入了解.net垃圾回收机制之代龄与算法详解](https://blog.csdn.net/superhoy/article/details/8553470)

[【C# .Net GC】垃圾回收算法 应用程序线程运行时，](https://www.cnblogs.com/cdaniu/p/15927757.html)

[C#【必备技能篇】垃圾回收机制(GC)的理解（资源清理+内存管理）](https://blog.csdn.net/sinat_40003796/article/details/128041837)

[C#内存泄漏--event内存泄漏](https://www.chinacion.cn/article/2907.html)

[温故之.NET垃圾回收](https://zhuanlan.zhihu.com/p/38292761)




[【C# .Net GC】强制垃圾回收 和System GC](https://www.cnblogs.com/cdaniu/p/15935837.html)

[C# 垃圾回收机制GC详解](https://zhuanlan.zhihu.com/p/484572963)

# GC 实现禁止垃圾回收

## GC

`GC`静态类用于控制系统垃圾收集器，该服务会自动回收未使用的内存。

下面的示例来自官方文档，演示了使用 GC 类的几个方法获取未使用对象的生成和内存信息、内存总数等。

```C#
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
```

## .KeepAlive
