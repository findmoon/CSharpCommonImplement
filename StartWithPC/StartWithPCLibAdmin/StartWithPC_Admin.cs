using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// (管理员权限)实现 设置或取消开机自启动、判断是否开机自启【读写注册表】
    /// 
    /// SOFTWARE\Microsoft\Windows\CurrentVersion\Run 注册表下未找到对应的项
    /// </summary>
    public static class StartWithPC_Admin
    {
        /// <summary>
        /// 是否已经开机启动
        /// </summary>
        /// <returns></returns>
        public static bool IsRunWithPC
        {
            get
            {
                string fileName = Process.GetCurrentProcess().MainModule.FileName;//获取当前执行的含exe的文件名
                                                                                  //获取指定子项
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
                {
                    if (registryKey != null)
                    {
                        string ExeNameWithExt = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                        //获取指定定项值
                        object key = registryKey.GetValue(ExeNameWithExt);
                        registryKey.Close();
                        if (key == null || key.ToString() != fileName)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        #region //开机启动、自动运行
        /// <summary>
        /// 设置开机自启动
        /// </summary>
        public static void RunWithPC()
        {
            #region 其他无关测试
            ////获取当前应用程序路径(.exe文件所在的目录）
            ////string DirPath = System.Environment.CurrentDirectory;//当前目录的完全限定名

            ////string str = this.GetType().Assembly.Location;//获取当前进程的完整路径，包含文件名(进程名)

            ////string fileNameWithoutExt = Process.GetCurrentProcess().ProcessName;//程序名(不含扩展名.exe)

            ////获取当前 Thread 的当前应用程序域的基目录，它由程序集冲突解决程序用来探测程序集。
            //string str = System.AppDomain.CurrentDomain.BaseDirectory;
            ////result: X:\xxx\xxx\ (.exe文件所在的目录 + "\")

            ////获取和设置包含该应用程序的目录的名称。(推荐)
            //string str1 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //// result: X:\xxx\xxx\ (.exe文件所在的目录 + "\") 
            #endregion

            string fileName = Process.GetCurrentProcess().MainModule.FileName;//获取当前执行的含exe的文件名

            #region 打开注册表，不存在再创建
            ////获取指定子项
            //RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            //if (registryKey == null)//不存在则创建
            //{
            //    registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            //}
            #endregion

            // 新建 或 打开一个现有 的注册表子项 以进行写访问 【之前的版本，记得似乎创建时存在不会打开】
            using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
            {
                string ExeNameWithExt = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                object key = registryKey.GetValue(ExeNameWithExt);
                if (key != null && key.ToString() == fileName)
                {
                    return;
                }
                //设置该项新的键值对,自动运行
                registryKey.SetValue(ExeNameWithExt, fileName);
                registryKey.Close();//关闭该项，更改的内容会刷新到磁盘
                SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);

            }
        }
        /// <summary>
        /// 取消开机自启动
        /// </summary>
        public static void CancleRunWithPC()
        {
            string fileName = Process.GetCurrentProcess().MainModule.FileName;//获取当前执行的含exe的文件名
            //获取指定子项
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (registryKey == null) return;
                try
                {
                    string ExeNameWithExt = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    //删除指定定项值
                    registryKey.DeleteValue(ExeNameWithExt, true);//false找不到值不引发异常

                    //刷新资源管理器
                    SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);

                }
                catch (Exception)//不是开机自动启动
                {
                }
                finally
                {
                    registryKey.Close();
                }
            }            
        }


        /// <summary>
        /// 引入刷新资源管理器的方法并声明
        /// </summary>
        /// <param name="wEventId"></param>
        /// <param name="uFlags"></param>
        /// <param name="dwItem1"></param>
        /// <param name="dwItem2"></param>
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        #endregion

    }
}
