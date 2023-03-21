using System;
using System.Runtime.InteropServices;


namespace HelperCollections
{
    /// <summary>
    /// Win32 API 的外部方法
    /// </summary>
    internal partial class Win32Extern
    {
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="pszPath">一个包含要取得信息的文件相对或绝对路径的缓冲。它可以处理长或短文件名。（也就是指定的文件路径）</param>
        /// <param name="dwFileAttributes">此参数仅用于uFlags中包含SHGFI_USEFILEATTRIBUTES标志的情况(一般不使用)。如此，它应该是文件属性的组合：存档，只读，目录，系统等。</param>
        /// <param name="psfi">指向 SHFILEINFO 结构的指针，用于接收文件信息。</param>
        /// <param name="cbfileInfo">指向的 SHFILEINFO 结构的大小</param>
        /// <param name="uFlags">[要检索的文件信息的标志] 函数的核心变量，通过所有可能的标志，指定函数的行为和实际得到的信息</param>
        /// <returns></returns>
        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo
        (
            string pszPath,
            uint dwFileAttributes,
            out SHFILEINFO psfi,
            uint cbfileInfo,
            SHGFI uFlags
        );

        [DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo")]
        public static extern IntPtr SHGetFileInfo_ref(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        #region SHGetImageList 相关 获取文件或目录图标

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static int SHGetImageList(IMAGELIST_SIZE_FLAG iImageList, ref Guid riid, ref IImageList ppv);
        /// <summary>
        /// 清除图标
        /// </summary>
        /// <param name="hIcon">图标句柄</param>
        /// <returns>返回非零表示成功，零表示失败</returns>
        [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("shell32.dll")]
        public static extern uint SHGetIDListFromObject([MarshalAs(UnmanagedType.IUnknown)] object iUnknown, out IntPtr ppidl);

        #endregion


        /// <summary>
        /// 从文件中提取icon
        /// </summary>
        /// <param name="lpszFile"></param>
        /// <param name="nIconIndex"></param>
        /// <param name="phiconLarge"></param>
        /// <param name="phiconSmall"></param>
        /// <param name="nIcons"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, int nIcons);
    }
}
