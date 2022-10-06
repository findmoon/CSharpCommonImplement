using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTypeRegisterTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 处理打开文件时
            string filePath = "";
            if (args.Length==1 && File.Exists(args[0]))
            {
                filePath = args[0];
            }
            #region 对于路径中间带空格的会自动分割成多个参数传入 但有可能多个空格 最正确的处理是：在注册表关联的程序文件打开的参数通过""确保参数文件路径正确
            //string filePath = "";
            //if ((args != null) && (args.Length > 0))
            //{
            //    for (int i = 0; i < args.Length; i++)
            //    {
            //        // 对于路径中间带空格的会自动分割成多个参数传入 但有可能多个空格 
            //        filePath += " " + args[i];
            //    }

            //    filePath.Trim();
            //} 
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(filePath));
        }
    }
}
