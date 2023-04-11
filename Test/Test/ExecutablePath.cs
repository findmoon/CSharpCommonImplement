using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    internal class ExecutablePath
    {
        public ExecutablePath() {

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine("获取程序集或可执行程序路径 - 开始");

            // 完整路径
            Debug.WriteLine($"System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName：{System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName}");

            Debug.WriteLine(Environment.NewLine);

            // URL格式的程序集路径
            Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase：{System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase}");

            Debug.WriteLine(Environment.NewLine);
            
            // 完整路径
            Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().Location：{System.Reflection.Assembly.GetExecutingAssembly().Location}");

            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine(Environment.NewLine);

            // 路径
            Debug.WriteLine($"System.AppDomain.CurrentDomain.BaseDirectory：{System.AppDomain.CurrentDomain.BaseDirectory}");

            Debug.WriteLine(Environment.NewLine);

            // 路径
            Debug.WriteLine($"System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase：{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}");

            Debug.WriteLine(Environment.NewLine);
            
            // 路径
            Debug.WriteLine($"System.IO.Directory.GetCurrentDirectory()：{System.IO.Directory.GetCurrentDirectory()}");

            Debug.WriteLine(Environment.NewLine);

            // 路径
            Debug.WriteLine($"System.Environment.CurrentDirectory：{System.Environment.CurrentDirectory}");

            Debug.WriteLine(Environment.NewLine);




            Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().FullName：{System.Reflection.Assembly.GetExecutingAssembly().FullName}");

            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName：{System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName}");

            Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().Name：{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}");

            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine($"System.Diagnostics.Process.GetCurrentProcess().ProcessName：{System.Diagnostics.Process.GetCurrentProcess().ProcessName}");

            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine($"System.Diagnostics.Process.GetCurrentProcess().MainModule?.ModuleName：{System.Diagnostics.Process.GetCurrentProcess().MainModule?.ModuleName}");

            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine($"System.AppDomain.CurrentDomain.FriendlyName：{System.AppDomain.CurrentDomain.FriendlyName}");







            Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().EntryPoint：{System.Reflection.Assembly.GetExecutingAssembly().EntryPoint}");

            Debug.WriteLine(Environment.NewLine);

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine("获取程序集或可执行程序路径 - 结束");
        }
    }
}
