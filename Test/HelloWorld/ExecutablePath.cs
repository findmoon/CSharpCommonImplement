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

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("获取程序集或可执行程序路径 - 开始");

            // 完整路径
            Console.WriteLine($"System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName：{System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName}");

            Console.WriteLine();

            // URL格式的程序集路径
            Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase：{System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase}");

            Console.WriteLine();
            
            // 完整路径
            Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().Location：{System.Reflection.Assembly.GetExecutingAssembly().Location}");

            Console.WriteLine();

            Console.WriteLine();

            // 路径
            Console.WriteLine($"System.AppDomain.CurrentDomain.BaseDirectory：{System.AppDomain.CurrentDomain.BaseDirectory}");

            Console.WriteLine();

            // 路径
            Console.WriteLine($"System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase：{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}");

            Console.WriteLine();
            
            // 路径
            Console.WriteLine($"System.IO.Directory.GetCurrentDirectory()：{System.IO.Directory.GetCurrentDirectory()}");

            Console.WriteLine();

            // 路径
            Console.WriteLine($"System.Environment.CurrentDirectory：{System.Environment.CurrentDirectory}");

            Console.WriteLine();




            Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().FullName：{System.Reflection.Assembly.GetExecutingAssembly().FullName}");

            Console.WriteLine();

            Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName：{System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName}");

            Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().Name：{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}");

            Console.WriteLine();

            Console.WriteLine($"System.Diagnostics.Process.GetCurrentProcess().ProcessName：{System.Diagnostics.Process.GetCurrentProcess().ProcessName}");

            Console.WriteLine();

            Console.WriteLine($"System.Diagnostics.Process.GetCurrentProcess().MainModule?.ModuleName：{System.Diagnostics.Process.GetCurrentProcess().MainModule?.ModuleName}");

            Console.WriteLine();

            Console.WriteLine($"System.AppDomain.CurrentDomain.FriendlyName：{System.AppDomain.CurrentDomain.FriendlyName}");


















            Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().EntryPoint：{System.Reflection.Assembly.GetExecutingAssembly().EntryPoint}");

            Console.WriteLine();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("获取程序集或可执行程序路径 - 结束");
        }
    }
}
