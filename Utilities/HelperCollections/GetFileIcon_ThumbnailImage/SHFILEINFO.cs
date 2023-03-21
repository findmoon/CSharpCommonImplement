using System;
using System.Runtime.InteropServices;

namespace HelperCollections
{
    /// <summary>
    /// SHGetFileInfo 获取的文件信息 结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SHFILEINFO
    {
        public IntPtr hIcon;//图标句柄
        public int iIcon;//系统图标列表的索引
        public uint dwAttributes; //文件的属性
        [MarshalAs(UnmanagedType.LPStr, SizeConst = 260)] // MarshalAs指示如何在托管代码和非托管代码间传送数据
        public string szDisplayName;//文件的路径等 文件名最长256（ANSI），加上盘符（X:\）3字节，259字节，再加上结束符1字节，共260
        [MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
        public string szTypeName;//文件的类型名 固定80字节
    };
}
