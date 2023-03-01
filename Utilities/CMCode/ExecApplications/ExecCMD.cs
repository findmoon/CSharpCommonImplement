using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CMCode.Call
{
    /// <summary>
    /// 执行cmd命令
    /// </summary>
    public static class ExecCMD
    {
        #region 异步方法
        /// <summary>
        /// 执行cmd命令 返回cmd窗口显示的信息
        /// 多命令请使用批处理命令连接符：
        /// <![CDATA[
        /// &:同时执行两个命令
        /// |:将上一个命令的输出,作为下一个命令的输入
        /// &&：当&&前的命令成功时,才执行&&后的命令
        /// ||：当||前的命令失败时,才执行||后的命令]]>
        /// </summary>
        ///<param name="command">执行的命令</param>
        ///<param name="workDirectory">工作目录</param>
        /// <returns>cmd命令执行窗口的输出</returns>
        public static async Task<ExecResult> RunAsync(string command,string workDirectory=null)
        {
            command = command.Trim().TrimEnd('&') + "&exit";  //说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态

            string cmdFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "cmd.exe");// @"C:\Windows\System32\cmd.exe";
            using (Process p = new Process())
            {
                var result = new ExecResult();
                try
                {
                    p.StartInfo.FileName = cmdFileName;
                    p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                    p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                    p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                    p.StartInfo.CreateNoWindow = true;          //不显示程序窗口

                    if (!string.IsNullOrWhiteSpace(workDirectory))
                    {
                        p.StartInfo.WorkingDirectory = workDirectory;
                    }

                    p.Start();//启动程序

                    //向cmd窗口写入命令
                    p.StandardInput.WriteLine(command);
                    p.StandardInput.AutoFlush = true;

                    // 若要使用StandardError，必须设置ProcessStartInfo.UseShellExecute为false，并且必须设置 ProcessStartInfo.RedirectStandardError 为 true。 否则，从 StandardError 流中读取将引发异常。
                    //获取cmd的输出信息
                    result.Output = await p.StandardOutput.ReadToEndAsync();
                    result.Error = await p.StandardError.ReadToEndAsync();

                    p.WaitForExit();//等待程序执行完退出进程。应在最后调用
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
        /// 执行多个cmd命令 返回cmd窗口显示的信息
        /// 此处执行的多条命令并不是交互执行的信息，是多条独立的命令。也可以使用&连接多条命令为一句执行
        /// </summary>
        ///<param name="command">执行的命令</param>
        /// <returns>cmd命令执行窗口的输出</returns>
        /// <returns>工作目录</returns>
        public static async Task<ExecResult> RunAsync(string[] commands,string workDirectory=null)
        {
            if (commands == null)
            {
                throw new ArgumentNullException();
            }
            if (commands.Length == 0)
            {
                return default(ExecResult);
            }
            return await Task.Run(() =>
            {
                commands[commands.Length - 1] = commands[commands.Length - 1].Trim().TrimEnd('&') + "&exit";  //说明：不管命令是否成功均执行exit命令

                string cmdFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "cmd.exe");// @"C:\Windows\System32\cmd.exe";
                using (Process p = new Process())
                {
                    var result = new ExecResult();
                    try
                    {
                        p.StartInfo.FileName = cmdFileName;
                        p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                        p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                        p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                        p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                        p.StartInfo.CreateNoWindow = true;          //不显示程序窗口

                        if (!string.IsNullOrWhiteSpace(workDirectory))
                        {
                            p.StartInfo.WorkingDirectory = workDirectory;
                        }

                        // 接受输出的方式逐次执行每条
                        //var output = string.Empty;
                        var inputI = 1;
                        p.OutputDataReceived += (sender, e) =>
                        {
                            // cmd中的输出会包含换行；其他应用可以考虑接收数据是添加换行 Environment.NewLine
                            result.Output +=$"{ e.Data}{Environment.NewLine}" ;// 获取输出 
                            if (inputI >= commands.Length)
                            {
                                return;
                            }
                            if (e.Data.Contains(commands[inputI - 1]))
                            {
                                p.StandardInput.WriteLine(commands[inputI]);
                            }
                            inputI++;
                        };
                        
                        p.ErrorDataReceived+= (sender, e) =>
                        {
                            result.Error += $"{ e.Data}{Environment.NewLine}";// 获取输出 
                            if (inputI>= commands.Length)
                            {
                                return;
                            }
                            if (e.Data.Contains(commands[inputI - 1]))
                            {
                                p.StandardInput.WriteLine(commands[inputI]);
                            }
                            inputI++;
                        };

                        p.Start();//启动程序

                        // 开始异步读取输出流
                        p.BeginOutputReadLine();
                        p.BeginErrorReadLine();
                        //向cmd窗口写入命令
                        p.StandardInput.WriteLine(commands[0]);
                        p.StandardInput.AutoFlush = true;

                        p.WaitForExit();//等待程序执行完退出进程。应在最后调用
                        p.Close();
                    }
                    catch (Exception ex)
                    {
                        result.ExceptError = ex;
                    }
                    return result;
                }
            });
        }
        #endregion
        /// <summary>
        /// 执行cmd命令 返回cmd窗口显示的信息
        /// 多命令请使用批处理命令连接符：
        /// <![CDATA[
        /// &:同时执行两个命令
        /// |:将上一个命令的输出,作为下一个命令的输入
        /// &&：当&&前的命令成功时,才执行&&后的命令
        /// ||：当||前的命令失败时,才执行||后的命令]]>
        /// </summary>
        ///<param name="command">执行的命令</param>
        ///<param name="workDirectory">工作目录</param>
        public static ExecResult Run(string command, string workDirectory = null)
        {
            command = command.Trim().TrimEnd('&') + "&exit";  //说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态

            string cmdFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "cmd.exe");// @"C:\Windows\System32\cmd.exe";
            using (Process p = new Process())
            {
                var result = new ExecResult();
                try
                {
                    p.StartInfo.FileName = cmdFileName;
                    p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动，设置为false可以重定向输入输出错误流；同时会影响WorkingDirectory的值
                    p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                    p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                    p.StartInfo.CreateNoWindow = true;          //不显示程序窗口

                    if (!string.IsNullOrWhiteSpace(workDirectory))
                    {
                        p.StartInfo.WorkingDirectory = workDirectory;
                    }

                    p.Start();//启动程序

                    //向cmd窗口写入命令
                    p.StandardInput.WriteLine(command);
                    p.StandardInput.AutoFlush = true;

                    //获取cmd的输出信息
                    result.Output = p.StandardOutput.ReadToEnd();
                    result.Error = p.StandardError.ReadToEnd();
                    p.WaitForExit();//等待程序执行完退出进程。应在最后调用
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
