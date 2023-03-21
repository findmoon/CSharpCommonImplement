using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace HelperCollections
{
    /// <summary>
    /// 从 SHGetImageList 图标列表中，根据 SHGetFileInfo 获取文件、文件夹图标 的帮助类
    /// </summary>
    public class FileDirIconHelper_ImageList
    {
        #region win32 API SHGetFileInfo  SHGetImageList

        /// <summary>
        /// 获取文件、文件夹的图标
        /// </summary>
        /// <param name="fileDirName">文件、文件夹名</param>
        /// <param name="flag">图标尺寸标识，默认获取16x16</param>
        /// <returns></returns>
        public static System.Drawing.Icon GetFileDirIcon(string fileDirName, IMAGELIST_SIZE_FLAG flag= IMAGELIST_SIZE_FLAG.SHIL_SMALL| IMAGELIST_SIZE_FLAG.SHIL_SYSSMALL)
        {
            return GetIcon(GetIconIndex(fileDirName), flag);
        }

        /// <summary>
        /// SHGetImageList 获取所有文件/系统图标列表
        /// </summary>
        /// <param name="flag">图标尺寸标识</param>
        /// <returns></returns>
        public static System.Drawing.Icon[] GetImageList(IMAGELIST_SIZE_FLAG flag = IMAGELIST_SIZE_FLAG.SHIL_SMALL | IMAGELIST_SIZE_FLAG.SHIL_SYSSMALL)
        {
            IImageList list = null;
            
            Win32Extern.SHGetImageList(flag, ref theGuid, ref list);//获取系统图标列表
            int count = 0;
            int r = list.GetImageCount(ref count);//获取图标索引总数

            var icons = new Icon[count];

            IntPtr hIcon = IntPtr.Zero;
            for (int i = 0; i < count; i++)
            {
                hIcon = IntPtr.Zero;
                list.GetIcon(i, ILD_TRANSPARENT | ILD_IMAGE, ref hIcon);//获取指定索引号的图标句柄
                if (hIcon== IntPtr.Zero)
                {
                    continue;
                }
                var icon = Icon.FromHandle(hIcon);
                // 会出现这种情况
                if (icon.Width==0 || icon.Height==0)
                {
                    continue;
                }
                icons[i]= icon;
                // Win32Extern.DestroyIcon(hIcon);
            }
            return icons;
        }


        private const int ILD_TRANSPARENT = 0x00000001;
        private const int ILD_IMAGE = 0x00000020;

        private const string IID_IImageList2 = "192B9D83-50FC-457B-90A0-2B82A8B5DAE1";//GUID的两个com标识中的一个，底层固定
        private const string IID_IImageList = "46EB5926-582E-4017-9FDF-E8998DAA0950";//GUID的两个com标识中的一个，底层固定

        private static Guid theGuid = new Guid(IID_IImageList);//目前所知用IID_IImageList2也是一样的

        /// <summary>
        /// 获取文件的图标索引号
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>图标索引号</returns>
        private static int GetIconIndex(string fileName)
        {
            SHFILEINFO info = new SHFILEINFO();
            IntPtr iconIntPtr = Win32Extern.SHGetFileInfo_ref(fileName, 0, ref info, (uint)Marshal.SizeOf(info), (uint)(SHGFI.SysIconIndex | SHGFI.OpenIcon));
            if (iconIntPtr == IntPtr.Zero)
                return -1;
            return info.iIcon;
        }

        /// <summary>
        /// 根据图标索引号获取图标
        /// </summary>
        /// <param name="iIcon">图标索引号</param>
        /// <param name="flag">图标尺寸标识</param>
        /// <returns></returns>
        private static System.Drawing.Icon GetIcon(int iIcon, IMAGELIST_SIZE_FLAG flag)
        {
            IImageList list = null;
            //Guid theGuid = new Guid(IID_IImageList);//目前所知用IID_IImageList2也是一样的
            Win32Extern.SHGetImageList(flag, ref theGuid, ref list);//获取系统图标列表
            IntPtr hIcon = IntPtr.Zero;
            int r = list.GetIcon(iIcon, ILD_TRANSPARENT | ILD_IMAGE, ref hIcon);//获取指定索引号的图标句柄
            var icon= System.Drawing.Icon.FromHandle(hIcon);

            //// 会出现这种情况
            //if (icon.Width == 0 || icon.Height == 0)
            //{
            //    return null;
            //}
            // Win32Extern.DestroyIcon(hIcon);
            return icon;
        }

        #endregion
    }
}
