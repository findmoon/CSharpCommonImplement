namespace TaskRunDirectReturn
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 直接执行一个异步任务
            // 将Task任务排队到线程池上，并返回表示当前工作的任务对象（Task Object）。
            Task.Run(() =>
            {
                Console.WriteLine($"[{DateTime.Now.ToString("yyyMMddHHmmss")}]：Task异步");
            });
            Console.WriteLine("123");
            Thread.Sleep(1500);

            //Console.ReadKey();
        }
    }
}