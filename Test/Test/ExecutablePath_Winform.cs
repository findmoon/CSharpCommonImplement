using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    /// <summary>
    /// 可执行程序的路径
    /// </summary>
    internal class ExecutablePath_Winform
    {
        public ExecutablePath_Winform() {

            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine("获取程序集或可执行程序路径 - 开始");

            // 完整路径
            Debug.WriteLine($"System.Windows.Forms.Application.ExecutablePath：{System.Windows.Forms.Application.ExecutablePath}");

            Debug.WriteLine(Environment.NewLine);

            // 启动路径
            Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase：{System.Windows.Forms.Application.StartupPath}");

            Debug.WriteLine(Environment.NewLine);
            
            // 产品名 
            Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().Location：{System.Windows.Forms.Application.ProductName}");

            Debug.WriteLine(Environment.NewLine);

            
            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine("获取程序集或可执行程序路径 - 结束");
        }
    }
}
