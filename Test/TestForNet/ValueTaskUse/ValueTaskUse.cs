using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiscellaneousTestForNet
{
    internal class ValueTaskUse
    {
        public static void TestUse1()
        {
            var valueTaskUse = new ValueTaskUse();
            var taskResult=valueTaskUse.GetCustomerIdAsync();
            Debug.WriteLine($"Task 立即执行 是否结束：{taskResult.IsCompleted}");

            var valrutaskResult=valueTaskUse.GetCustomerIdAsync_ValueTask();
            Debug.WriteLine($"ValueTask 立即执行 是否结束：{valrutaskResult.IsCompleted}");

        }
        public static void TestUse2()
        {
            IRepository<int> repository = new Repository<int>();
            var result = repository.GetData();
            if (result.IsCompleted)
                Debug.WriteLine("Operation complete...");
            else
                Debug.WriteLine("Operation incomplete...");
            Console.ReadKey();
        }


        /// <summary>
        /// 测试 异步任务 的线程Id (是否切换线程)
        /// </summary>
        /// <returns></returns>
        public static async Task TestTaskThreadIdAsync()
        {
            Debug.WriteLine($"await 外的线程Id：{Thread.CurrentThread.ManagedThreadId}");
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Debug.WriteLine($"await 中的线程Id：{Thread.CurrentThread.ManagedThreadId}");
            });
        }


        /// <summary>
        /// 测试 异步任务 的线程Id (是否切换线程)
        /// </summary>
        /// <returns></returns>
        public static async Task TestTaskThreadIdAsync_ConfigureAwaitTrue()
        {
            Debug.WriteLine($"await 外的线程Id：{Thread.CurrentThread.ManagedThreadId}");
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Debug.WriteLine($"await 中的线程Id：{Thread.CurrentThread.ManagedThreadId}");
            }).ConfigureAwait(true);
        }


        public Task<int> GetCustomerIdAsync()
        {
            Debug.WriteLine($"Task 执行");
            return Task.FromResult(1);
        }

        public ValueTask<int> GetCustomerIdAsync_ValueTask()
        {
            Debug.WriteLine($"ValueTask 执行");
            return new ValueTask<int>(1);
        }
    }

    public interface IRepository<T>
    {
        ValueTask<T> GetData();
        ValueTask<T> GetDataAsync();
    }

    public class Repository<T> : IRepository<T>
    {
        public ValueTask<T> GetData()
        {
            var value = default(T);
            return new ValueTask<T>(value!); // default(T) 可能为null
        }

        // 或者 改为 可空类型T?
        //public ValueTask<T?> GetData()
        //{
        //    var value = default(T);
        //    return new ValueTask<T?>(value);
        //}

        public async ValueTask<T> GetDataAsync()
        {
            var value = default(T);
            await Task.Delay(100);
            return value!;
        }
    }
}
