using CMCode.Register;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            #region 检查文件类型注册，并以管理员权限重新运行 【不太好提取为单一方法】
            var extendName = ".mytxt";

            // 判断注册表数据
            if (!FileTypeRegister.FileTypeRegistered(extendName,true))
            {
                //获得当前登录的Windows用户标示 
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                //创建Windows用户主题 
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                // 判断当前主体是否为管理员权限
                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    // 注册文件
                    #region 关联文件
                    var filetypeRegInfo = new FileTypeRegInfo(extendName, Application.ExecutablePath);
                    filetypeRegInfo.Description = Application.ExecutablePath + "打开程序";
                    FileTypeRegister.RegisterFileType(filetypeRegInfo,false);
                    #endregion
                }
                else
                {
                    //创建启动对象 
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    // .netfx 默认为true；.net core 默认为false
                    //startInfo.UseShellExecute = true;
                    //设置运行文件 
                    startInfo.FileName = Application.ExecutablePath;
                    //设置启动参数 
                    startInfo.Arguments = String.Join(" ", args.Select(a => "\"" + args + "\""));
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    //设置启动动作,确保以管理员身份运行 
                    startInfo.Verb = "runas";
                    //如果不是管理员，则启动UAC 
                    Process.Start(startInfo);
                    //退出  // Application.Exit方法不会阻止后面代码的执行，也不会退出，后面代码执行并打开窗体，正常运行。
                    Application.Exit();
                    return;
                }
            }
            #endregion

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
