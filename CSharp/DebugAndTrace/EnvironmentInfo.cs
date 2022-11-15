using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugAndTrace
{
    /// <summary>
    /// 获取系统环境信息
    /// </summary>
    public static class EnvironmentInfo
    {
        public static string[] Infos { get {
                var infos = new List<string>();
                infos.Add("Operating system: " + System.Environment.OSVersion.ToString());
                infos.Add("64 bit Operating System: " + System.Environment.Is64BitOperatingSystem);
                infos.Add("Processor Count: " + System.Environment.ProcessorCount);
                infos.Add("Computer name: " + System.Environment.MachineName);
                infos.Add("User name: " + System.Environment.UserName);
                infos.Add("CLR runtime version: " + System.Environment.Version.ToString());
                infos.Add("Command line: " + System.Environment.CommandLine);

                return infos.ToArray();
            } }
    }
}
