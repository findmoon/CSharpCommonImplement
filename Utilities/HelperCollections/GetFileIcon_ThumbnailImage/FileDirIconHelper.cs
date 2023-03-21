using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace HelperCollections
{
    /// <summary>
    /// 获取文件图标、文件夹图标 的帮助类
    /// System.Drawing.dll 引用
    /// </summary>
    public class FileDirIconHelper
    {
        /// <summary>
        /// 获取文件、文件夹的图标 【推荐】
        /// System.Drawing.dll 引用
        /// </summary>
        /// <param name="fileDirName">文件、文件夹名</param>
        /// <param name="largeIcon">图标的大小，默认 false，获取16x16大小图标；否则32x32</param>
        /// <returns></returns>
        public static Icon GetFileDirIcon(string fileDirName, bool largeIcon = false)
        {
            SHFILEINFO info = new SHFILEINFO();
            int size = Marshal.SizeOf(info);
            SHGFI flags;
            if (largeIcon)
                flags = SHGFI.Icon | SHGFI.LargeIcon;//| SHGFI.UseFileAttributes;网上都有加这项导致只对文件有效，去掉后文件夹也可以。
            else
                flags = SHGFI.Icon | SHGFI.SmallIcon;//| SHGFI.UseFileAttributes;网上都有加这项导致只对文件有效，去掉后文件夹也可以。
            IntPtr iconIntPtr = Win32Extern.SHGetFileInfo(fileDirName, 0, out info, (uint)size, flags);
            if (iconIntPtr.Equals(IntPtr.Zero))
                return null;
            var icon= System.Drawing.Icon.FromHandle(info.hIcon);
            // Win32Extern.DestroyIcon(info.hIcon);
            return icon;
        }

        // SHGFI.UseFileAttributes | SHGFI.TypeName | SHGFI.DisplayName 实现获取扩展名的图标
    }
}
