using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugAndTrace
{
    /// <summary>
    /// 自定义的ConsoleTraceListener
    /// </summary>
    public class MyConsoleTraceListener : System.Diagnostics.TraceListener
    {
        public override void Write(string message)
        {
            Console.Write(message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
