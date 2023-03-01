using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace CMCode.IO
{
    /// <summary>
    /// 使用Window API实现的对长路径的处理 Win32 API function with P/Invoke  出自 https://stackoverflow.com/questions/5188527/how-to-deal-with-files-with-a-name-longer-than-259-characters#answer-39534444
    /// </summary>
    internal static class NativeMethods
    {
        internal const int FILE_ATTRIBUTE_ARCHIVE = 0x20;
        internal const int INVALID_FILE_ATTRIBUTES = -1;

        internal const int FILE_READ_DATA = 0x0001;
        internal const int FILE_WRITE_DATA = 0x0002;
        internal const int FILE_APPEND_DATA = 0x0004;
        internal const int FILE_READ_EA = 0x0008;
        internal const int FILE_WRITE_EA = 0x0010;

        internal const int FILE_READ_ATTRIBUTES = 0x0080;
        internal const int FILE_WRITE_ATTRIBUTES = 0x0100;

        internal const int FILE_SHARE_NONE = 0x00000000;
        internal const int FILE_SHARE_READ = 0x00000001;

        internal const int FILE_ATTRIBUTE_DIRECTORY = 0x10;

        internal const long FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE |
                                                    FILE_WRITE_DATA |
                                                    FILE_WRITE_ATTRIBUTES |
                                                    FILE_WRITE_EA |
                                                    FILE_APPEND_DATA |
                                                    SYNCHRONIZE;

        internal const long FILE_GENERIC_READ = STANDARD_RIGHTS_READ |
                                                FILE_READ_DATA |
                                                FILE_READ_ATTRIBUTES |
                                                FILE_READ_EA |
                                                SYNCHRONIZE;



        internal const long READ_CONTROL = 0x00020000L;
        internal const long STANDARD_RIGHTS_READ = READ_CONTROL;
        internal const long STANDARD_RIGHTS_WRITE = READ_CONTROL;

        internal const long SYNCHRONIZE = 0x00100000L;

        internal const int CREATE_NEW = 1;
        internal const int CREATE_ALWAYS = 2;
        internal const int OPEN_EXISTING = 3;

        internal const int MAX_PATH = 260;
        internal const int MAX_ALTERNATE = 14;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WIN32_FIND_DATA
        {
            public System.IO.FileAttributes dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh; //changed all to uint, otherwise you run into unexpected overflow
            public uint nFileSizeLow;  //|
            public uint dwReserved0;   //|
            public uint dwReserved1;   //v
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
            public string cAlternate;
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        internal static void CloseFileHandle(SafeFileHandle safeFileHandle)
        {
            if (safeFileHandle != null && !safeFileHandle.IsClosed)
            {
                safeFileHandle.Close();
            }
        }

        /// <summary>Moves the file pointer of the specified file.</summary>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>Minimum supported client: Windows XP [desktop apps | UWP apps]</remarks>
        /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | UWP apps]</remarks>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetFilePointerEx(SafeFileHandle hFile, [MarshalAs(UnmanagedType.U8)] ulong liDistanceToMove, IntPtr lpNewFilePointer, [MarshalAs(UnmanagedType.U4)] SeekOrigin dwMoveMethod);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CopyFileW(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetFileAttributesW(string lpFileName);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool DeleteFileW(string lpFileName);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool MoveFileW(string lpExistingFileName, string lpNewFileName);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetFileTime(SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetFileTime(SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);


        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);


        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool FindClose(IntPtr hFindFile);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool RemoveDirectory(string path);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CreateDirectory(string lpPathName, IntPtr lpSecurityAttributes);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SetFileAttributesW(string lpFileName, int fileAttributes);
    }
}
