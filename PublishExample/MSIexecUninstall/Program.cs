using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MSIexecUninstall
{
    /// <summary>
    /// 调用 msiexec.exe 卸载程序。参数为要卸载的软件的ProductCode
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            var currProductCode = "{F929A188-FE65-463B-B362-7DBA0C19D619}";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "msiexec.exe";
            startInfo.Arguments = $"/X {currProductCode}";
            startInfo.Verb = "runas";
            Process.Start(startInfo);
            return;

            var productCode = "";
            if (args.Length == 0) {
                Console.WriteLine("Please Enter ProductCode that will be uninstalled: ");
                productCode = Console.ReadLine().Trim();
            }
            else
            {
                productCode = args[0].Trim();
            }
            if (string.IsNullOrEmpty(productCode))
            {
                return;
            }
            if (productCode.StartsWith("/X"))
            {
                productCode= productCode.Substring(2).TrimStart();
            }
            if (!productCode.StartsWith("{"))
            {
                productCode = "{" + productCode;
            }
            if (!productCode.EndsWith("}"))
            {
                productCode += "}";
            }
            Process.Start("msiexec.exe", $"/X {productCode}");
        }
    }
}
