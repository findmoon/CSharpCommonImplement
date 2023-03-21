using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace HelperCollections
{
    /// <summary>
    /// 获取指定文件内的图标【可执行文件、DLL 或 图标文件】
    /// </summary>
    public class FileIconHelper
    {
        #region GDI+ Icon
        /// <summary>
        /// 获取指定文件中的ICON图标[一般是Size(32,32)]
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static System.Drawing.Icon GetIconFromFile(string fileName)
        {
            if (System.IO.File.Exists(fileName) == false)
                return null;
            return System.Drawing.Icon.ExtractAssociatedIcon(fileName);
        }
        #endregion

        #region ExtractIconEx  win32 API
        /// <summary>
        /// 方法2：使用【ExtractIconEx】从可执行文件、DLL 或 图标文件 中提取不同大小的图标
        /// </summary>
        /// <param name="fileName">应用程序文件名</param>
        /// <returns>返回获取到的Icon图标集，顺序为图标A Size(32,32)、图标A Size(16,16)、图标B Size(32,32)、图标B Size(16,16)....</returns>
        public static System.Drawing.Icon[] GetIconFromFile2(string fileName)
        {
            int count = Win32Extern.ExtractIconEx(fileName, -1, null, null, 0);

            IntPtr[] largeIcons = new IntPtr[count];

            IntPtr[] smallIcons = new IntPtr[count];

            Win32Extern.ExtractIconEx(fileName, 0, largeIcons, smallIcons, count);
            System.Drawing.Icon[] icons = new System.Drawing.Icon[count * 2];
            for (int i = 0; i < count; i++)
            {
                icons[i * 2] = System.Drawing.Icon.FromHandle(largeIcons[i]);
                icons[i * 2 + 1] = System.Drawing.Icon.FromHandle(smallIcons[i]);

                // Icon 无法使用
                //Win32Extern.DestroyIcon(largeIcons[i]);
                //Win32Extern.DestroyIcon(smallIcons[i]);
            }
            return icons;
        }

        #endregion
    }
}
