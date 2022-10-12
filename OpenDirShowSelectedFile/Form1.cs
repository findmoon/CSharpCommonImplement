using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDirShowSelectedFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string fileName1 = @"..\..\obj\新建文本文档.txt";
        string fileName2 = @"..\..\obj\我是有着特殊符号😁的文件名，路径名有😒也是一样的.txt";

        private void button1_Click(object sender, EventArgs e)
        {
            OpenDirSelectedFile_Process(fileName1);
            OpenDirSelectedFile_Process(fileName2);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            OpenDirSelectedFile_Win32(fileName1);
            OpenDirSelectedFile_Win32(fileName2);
            OpenDirSelectedFile_Win32(@"..\..\obj");
        }

        #region 方法1. explorer进程打开文件夹并选择文件(夹)
        /// <summary>
        /// 方法1. explorer进程打开文件夹并选择文件(夹)。只能选择一个文件打开
        /// </summary>
        /// <param name="path">文件或文件夹路径，不存在将打开"我的文档"</param>
        public static void OpenDirSelectedFile_Process(string path)
        {
            Process.Start("explorer", "/select,\"" + path + "\"");
        } 
        #endregion

        #region 方法2 SHOpenFolderAndSelectItems API      // Win32
        /// <summary>
        /// 释放命令行管理程序分配的ITEMIDLIST结构
        /// Frees an ITEMIDLIST structure allocated by the Shell.
        /// </summary>
        /// <param name="pidlList"></param>
        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern void ILFree(IntPtr pidlList);
        /// <summary>
        /// 返回与指定文件路径关联的ITEMIDLIST结构。
        /// Returns the ITEMIDLIST structure associated with a specified file path.
        /// </summary>
        /// <param name="pszPath"></param>
        /// <returns></returns>
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern IntPtr ILCreateFromPathW(string pszPath);
        /// <summary>
        /// 打开一个Windows资源管理器窗口，其中选择了特定文件夹中的指定项目。
        /// Opens a Windows Explorer window with specified items in a particular folder selected.
        /// </summary>
        /// <param name="pidlList"></param>
        /// <param name="cild"></param>
        /// <param name="children"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cild, IntPtr children, uint dwFlags);


        /// <summary>
        /// 方法2 SHOpenFolderAndSelectItems API 打开路径并定位文件【如果文件夹已打开则置前再选中，而不是再打开一个新的文件夹】
        /// </summary>
        /// <param name="fullPath">文件或文件夹绝对路径</param>
        public static void OpenDirSelectedFile_Win32(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
                throw new ArgumentNullException(nameof(fullPath));

            fullPath = Path.GetFullPath(fullPath);
            // 如何获取多个pidlList即列表
            var pidlList = ILCreateFromPathW(fullPath);
            if (pidlList == IntPtr.Zero) return;

            try
            {
                Marshal.ThrowExceptionForHR(SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0));
            }
            finally
            {
                ILFree(pidlList);
            }
        }
        #endregion


    }
}
