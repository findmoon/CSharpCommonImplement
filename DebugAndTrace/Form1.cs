using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugAndTrace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var a = 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Debug.Listeners.Clear();

            Debug.Write("Debug info");
            Debug.WriteIf(true, "Debug info if true");
            Debug.WriteLine("Debug info Line");
            Debug.WriteLineIf(true, "Debug info Line if true");

            Debug.WriteLine("Debug info Line","Info");
            Debug.WriteLine("Warning info Line", "Warning");
            Debug.WriteLine("Error info Line", "Error");
            Debug.WriteLine("Fatal info Line", "Fatal");

            Debug.WriteLine("错误列表:");
            Debug.Indent();
            Debug.WriteLine("错误 1: File not found","Error");
            Debug.WriteLine("错误 2: Directory not found", "Error");
            Debug.Unindent();
            Debug.WriteLine("错误结束");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var listener in Debug.Listeners)
            {
                Console.WriteLine(listener);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var a = 0;
                var b = 10;
                var foo = b / a;
            }
            catch (Exception ex)
            {
                Debug.Fail($"发生错误: {ex.Message}" ,
                   ex.StackTrace);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = -40;

            // 测试[条件]是否有效
            Debug.Assert(index > -1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (var listener in Trace.Listeners)
            {
                Console.WriteLine(listener);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Trace.Write("Trace info");
            Trace.WriteIf(true, "Trace info if true");
            Trace.WriteLine("Trace info Line");
            Trace.WriteLineIf(true, "Trace info Line if true");
            
            Trace.WriteLine("Trace info Line", "Info");
            Trace.WriteLine("Trace Warning info Line", "Warning");
            Trace.WriteLine("Trace Error info Line", "Error");
            Trace.WriteLine("Trace Fatal info Line", "Fatal");
            
            Trace.WriteLine("错误列表:");
            Trace.Indent();
            Trace.WriteLine("错误 1: File not found", "Error");
            Trace.WriteLine("错误 2: Directory not found", "Error");
            Trace.Unindent();
            Trace.WriteLine("错误结束");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Trace.Assert(false, "测试");

            Trace.TraceError("TraceError错误");
            Trace.TraceInformation("TraceError消息");
            Trace.TraceWarning("TraceError警告");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // 输出文件 TestFile.txt.
            //Stream myFileStream = File.Open("TestFile.txt", FileMode.OpenOrCreate);// 记得使用FileMode.Append，否则文件流会覆盖原有内容，而不是添加。
            //FileMode.Append 存在则打开并查找文件尾；不存在则创建新文件
            using (Stream myFileStream = File.Open("TestFile.txt", FileMode.Append))
            {
                // 清除默认的 listener 或 使用 RemoveAt/Remove
                Trace.Listeners.Clear();
               
                // 自动刷新
                Trace.AutoFlush = true;

                /* 使用输出流创建 text writer,并添加到 trace listeners. */
                TextWriterTraceListener myTextListener = new
                   TextWriterTraceListener(myFileStream);
                Trace.Listeners.Add(myTextListener);

                // 写入输出文件
                Trace.WriteLine("Test output ");

                // 记得关闭流，使用using或Trace.close()

                Debug.WriteLine("Debug信息");
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Trace.Listeners.Add(new TextWriterTraceListener("TextWriterOutput.log", "myListener"));
            Trace.TraceInformation("测试消息1.");
            Trace.TraceError("测试消息2.");
            Trace.TraceWarning("测试消息3.");
            // 必须 close or flush trace 以清空输出buffer.
            Trace.Flush();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            TextWriterTraceListener myListener = new TextWriterTraceListener("TextWriterOutput2.log", "myListener");
            myListener.WriteLine("独立使用-测试消息1.");
            myListener.WriteLine("独立使用-测试消息2.");
            myListener.WriteLine("独立使用-测试消息3.");
            
            // 刷新缓存
            myListener.Flush();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            using (var myFileStream = File.Open("TextWriterOutput2.log", FileMode.Append))
            {
                TextWriterTraceListener myListener = new TextWriterTraceListener(myFileStream);

                myListener.WriteLine("独立使用-测试消息1.");
                myListener.WriteLine("独立使用-测试消息2.");
                myListener.WriteLine("独立使用-测试消息3.");
                // 刷新缓存
                myListener.Flush();
            }

            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var consoleTracer = new ConsoleTraceListener();
            var infos = EnvironmentInfo.Infos;
            foreach (var info in infos)
            {
                consoleTracer.WriteLine(info);
            }
            // TextWriterTraceListener 指定Writer输出到控制台
            var consoleListener = new TextWriterTraceListener(System.Console.Out);
            // 或
            var consoleListener2 = new TextWriterTraceListener();
            consoleListener2.Writer = System.Console.Out;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var myConsoleTraceListener = new MyConsoleTraceListener();
            myConsoleTraceListener.WriteLine("测试输出");
        }
    }
}
