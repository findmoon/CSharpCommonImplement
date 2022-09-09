using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CMCode.Call
{
    /// <summary>
    /// 执行外部exe程序
    /// </summary>
    public static class ExecApp
    {
        /// <summary>
        /// 启动一个可执行程序，执行参数为arguments。启动一个关联的子进程，并且等待子进程的处理返回【一种有限的方式运行其他程序】
        /// 注意，如果是等待结束的情况下，必须保证会自动结束，或者最后会关闭/退出程序
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="arguments">启动参数</param>
        /// <param name="waitComplete">是否等待结束，默认false，启动程序后就结束不再管理；若为true，则必须保证会执行结束，或者手动关闭/退出程序，否则一直等待</param>
        /// <param name="getOuput">是否获取输出结果，waitComplete为true时，true获取输出结果才有效，并且要确保程序有输出，否则会一直等待</param>
        /// <returns></returns>
        public static async Task<ExecResult> StartExecutAsync(string appPath,string arguments=null, bool waitComplete= false, bool getOuput=false)
        {
            using (Process p = new Process())
            {
                var result = new ExecResult();
                try
                {
                    p.StartInfo.FileName = appPath;
                    p.StartInfo.Arguments = arguments;
                    p.StartInfo.UseShellExecute = false;        //设置false只能启动一个程序
                    p.StartInfo.RedirectStandardInput = false;  
                    p.StartInfo.RedirectStandardOutput = getOuput;  //由调用程序获取输出信息
                    p.StartInfo.RedirectStandardError = getOuput;   //重定向标准错误输出
                    p.StartInfo.CreateNoWindow = false;          //显示程序窗口

                    //p.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;

                    p.Start();//启动程序执行
                   
                    if (waitComplete)
                    {
                        if (getOuput)
                        {
                            // 获取输出【确保程序有输出，否则会一直等待】
                            result.Output = await p.StandardOutput.ReadToEndAsync();
                            result.Error = await p.StandardError.ReadToEndAsync();
                        }

                        p.WaitForExit();//等待程序执行完退出进程。应在最后调用
                    }
                    p.Close();
                }
                catch (Exception ex)
                {
                    result.ExceptError = ex;
                }
                return result;
            }
        }


        /// <summary>
        /// 执行指定路径appPath的应用程序，执行参数为arguments
        /// 注意，如果是调用执行完不会自动退出的程序，必须指定timeout
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="arguments"></param>
        /// <param name="inputs">启动后的要输入的交互信息</param>
        /// <returns></returns>
        [Obsolete(@"交互输入的信息，有可能是在新开的输入界面中接受，而不是当前执行调用的主线程，这是则需要额外处理新的界面输入，CreateNoWindow=false显示窗口再进一步输入；
                    也有可能当前调用的主线程中输入输出，等待交互信息。因此需要根据实际进行不同处理。可参考PGDBManagement项目中命令行交互输入新开窗口密码的实现", true)]
        public static async Task<ExecResult> RunAsync(string appPath, string arguments, string[] inputs, int timeout = 10000)
        {
            using (Process p = new Process())
            {
                var result = new ExecResult();
                try
                {
                    p.StartInfo.FileName = appPath;
                    p.StartInfo.Arguments = arguments;
                    p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                    p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                    p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                    p.StartInfo.CreateNoWindow = false;          //不显示程序窗口

                    p.Start();//启动程序执行

                    p.StandardInput.AutoFlush = true;

                    // // 等待结果，然后再StandardInput输入的方式不行，程序启动后等待输入，不会输出，`StandardOutput.ReadToEndAsync()`会一直等待读取输出
                    //result.Output = await p.StandardOutput.ReadToEndAsync();

                    //// 直接循环输入的方式也不行，执行到后面StandardOutput.ReadToEndAsync()仍会一直等待
                    //foreach (var input in inputs)
                    //{
                    //    // 写入标准输入
                    //    p.StandardInput.WriteLine(input);
                    //}

                    result.Output = await p.StandardOutput.ReadToEndAsync();
                    result.Error = await p.StandardError.ReadToEndAsync();

                    p.WaitForExit(timeout);//等待程序执行完退出进程
                    p.Close();

                }
                catch (Exception ex)
                {
                    result.ExceptError = ex;
                }
                return result;
            }
        }
    }
}
