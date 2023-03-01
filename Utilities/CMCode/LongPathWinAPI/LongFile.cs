using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CMCode.IO
{
    /// <summary>
    /// 原名LongFile。可处理LongPath的File操作
    /// </summary>
    public static class File
    {
        private const int MAX_PATH = 260;

        public static bool Exists(string path)
        {
            if (path.Length < MAX_PATH) return System.IO.File.Exists(path);
            var attr = NativeMethods.GetFileAttributesW(GetWin32LongPath(path));
            return (attr != NativeMethods.INVALID_FILE_ATTRIBUTES && ((attr & NativeMethods.FILE_ATTRIBUTE_ARCHIVE) == NativeMethods.FILE_ATTRIBUTE_ARCHIVE));
        }

        public static void Delete(string path)
        {
            if (path.Length < MAX_PATH) System.IO.File.Delete(path);
            else
            {
                bool ok = NativeMethods.DeleteFileW(GetWin32LongPath(path));
                if (!ok) ThrowWin32Exception();
            }
        }

        public static void AppendAllText(string path, string contents)
        {
            AppendAllText(path, contents, Encoding.Default);
        }

        public static void AppendAllText(string path, string contents, Encoding encoding)
        {
            if (path.Length < MAX_PATH)
            {
                System.IO.File.AppendAllText(path, contents, encoding);
            }
            else
            {
                var fileHandle = CreateFileForAppend(GetWin32LongPath(path));
                using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Write))
                {
                    var bytes = encoding.GetBytes(contents);
                    fs.Position = fs.Length;
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static void WriteAllText(string path, string contents)
        {
            WriteAllText(path, contents, Encoding.Default);
        }

        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            if (path.Length < MAX_PATH)
            {
                System.IO.File.WriteAllText(path, contents, encoding);
            }
            else
            {
                var fileHandle = CreateFileForWrite(GetWin32LongPath(path));

                using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Write))
                {
                    var bytes = encoding.GetBytes(contents);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (path.Length < MAX_PATH)
            {
                System.IO.File.WriteAllBytes(path, bytes);
            }
            else
            {
                var fileHandle = CreateFileForWrite(GetWin32LongPath(path));

                using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, false);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (sourceFileName.Length < MAX_PATH && (destFileName.Length < MAX_PATH)) System.IO.File.Copy(sourceFileName, destFileName, overwrite);
            else
            {
                var ok = NativeMethods.CopyFileW(GetWin32LongPath(sourceFileName), GetWin32LongPath(destFileName), !overwrite);
                if (!ok) ThrowWin32Exception();
            }
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            if (sourceFileName.Length < MAX_PATH && (destFileName.Length < MAX_PATH)) System.IO.File.Move(sourceFileName, destFileName);
            else
            {
                var ok = NativeMethods.MoveFileW(GetWin32LongPath(sourceFileName), GetWin32LongPath(destFileName));
                if (!ok) ThrowWin32Exception();
            }
        }

        public static string ReadAllText(string path)
        {
            return ReadAllText(path, Encoding.Default);
        }

        public static string ReadAllText(string path, Encoding encoding)
        {
            if (path.Length < MAX_PATH) { return System.IO.File.ReadAllText(path, encoding); }
            var fileHandle = GetFileHandle(GetWin32LongPath(path));

            using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Read))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                return encoding.GetString(data);
            }
        }

        public static string[] ReadAllLines(string path)
        {
            return ReadAllLines(path, Encoding.Default);
        }

        public static string[] ReadAllLines(string path, Encoding encoding)
        {
            if (path.Length < MAX_PATH) { return System.IO.File.ReadAllLines(path, encoding); }
            var fileHandle = GetFileHandle(GetWin32LongPath(path));

            using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Read))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                var str = encoding.GetString(data);
                if (str.Contains("\r")) return str.Split(new[] { "\r\n" }, StringSplitOptions.None);
                return str.Split('\n');
            }
        }
        public static byte[] ReadAllBytes(string path)
        {
            if (path.Length < MAX_PATH) return System.IO.File.ReadAllBytes(path);
            var fileHandle = GetFileHandle(GetWin32LongPath(path));

            using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Read))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                return data;
            }
        }


        public static void SetAttributes(string path, FileAttributes attributes)
        {
            if (path.Length < MAX_PATH)
            {
                System.IO.File.SetAttributes(path, attributes);
            }
            else
            {
                var longFilename = GetWin32LongPath(path);
                NativeMethods.SetFileAttributesW(longFilename, (int)attributes);
            }
        }

        #region Helper methods

        private static SafeFileHandle CreateFileForWrite(string filename)
        {
            if (filename.Length >= MAX_PATH) filename = GetWin32LongPath(filename);
            SafeFileHandle hfile = NativeMethods.CreateFile(filename, (int)NativeMethods.FILE_GENERIC_WRITE, NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.CREATE_ALWAYS, 0, IntPtr.Zero);
            if (hfile.IsInvalid) ThrowWin32Exception();
            return hfile;
        }

        private static SafeFileHandle CreateFileForAppend(string filename)
        {
            if (filename.Length >= MAX_PATH) filename = GetWin32LongPath(filename);
            SafeFileHandle hfile = NativeMethods.CreateFile(filename, (int)NativeMethods.FILE_GENERIC_WRITE, NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.CREATE_NEW, 0, IntPtr.Zero);
            if (hfile.IsInvalid)
            {
                hfile = NativeMethods.CreateFile(filename, (int)NativeMethods.FILE_GENERIC_WRITE, NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
                if (hfile.IsInvalid) ThrowWin32Exception();
            }
            return hfile;
        }

        internal static SafeFileHandle GetFileHandle(string filename)
        {
            if (filename.Length >= MAX_PATH) filename = GetWin32LongPath(filename);
            SafeFileHandle hfile = NativeMethods.CreateFile(filename, (int)NativeMethods.FILE_GENERIC_READ, NativeMethods.FILE_SHARE_READ, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
            if (hfile.IsInvalid) ThrowWin32Exception();
            return hfile;
        }

        internal static SafeFileHandle GetFileHandleWithWrite(string filename)
        {
            if (filename.Length >= MAX_PATH) filename = GetWin32LongPath(filename);
            SafeFileHandle hfile = NativeMethods.CreateFile(filename, (int)(NativeMethods.FILE_GENERIC_READ | NativeMethods.FILE_GENERIC_WRITE | NativeMethods.FILE_WRITE_ATTRIBUTES), NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
            if (hfile.IsInvalid) ThrowWin32Exception();
            return hfile;
        }

        [SecurityCritical]
        public static System.IO.FileStream GetFileStream(string filename, FileAccess access = FileAccess.Read)
        {
            var longFilename = GetWin32LongPath(filename);
            SafeFileHandle hfile=null;
            try
            {
                if (access == FileAccess.Write)
                {
                    hfile = NativeMethods.CreateFile(longFilename, (int)(NativeMethods.FILE_GENERIC_READ | NativeMethods.FILE_GENERIC_WRITE | NativeMethods.FILE_WRITE_ATTRIBUTES), NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
                }
                else
                {
                    hfile = NativeMethods.CreateFile(longFilename, (int)NativeMethods.FILE_GENERIC_READ, NativeMethods.FILE_SHARE_READ, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
                }

                if (hfile.IsInvalid) ThrowWin32Exception();

                return new System.IO.FileStream(hfile, access);
            }
            catch
            {
                NativeMethods.CloseFileHandle(hfile);
                throw;
            }
        }
        /// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path with read/write access.</summary>
		/// <param name="filename">The file to open.</param>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <returns>A <see cref="T:System.IO.FileStream" /> opened in the specified mode and path, with read/write access and not shared.</returns>
		[SecurityCritical]
        public static FileStream Open(string filename, FileMode mode)
        {
            return Open(filename, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite);
        }
        /// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path, with the specified mode and access.</summary>
		/// <param name="filename">The file to open.</param>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <param name="access">A <see cref="T:System.IO.FileAccess" /> value that specifies the operations that can be performed on the file.</param>
		/// <returns>An unshared <see cref="T:System.IO.FileStream" /> that provides access to the specified file, with the specified mode and access.</returns>
		[SecurityCritical]
        public static FileStream Open(string filename, FileMode mode, FileAccess access)
        {
            return Open(filename, mode, access, FileShare.None);
        }
        /// <summary>Opens a <see cref="T:System.IO.FileStream" /> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
		/// <param name="path">The file to open.</param>
		/// <param name="mode">A <see cref="T:System.IO.FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
		/// <param name="access">A <see cref="T:System.IO.FileAccess" /> value that specifies the operations that can be performed on the file.</param>
		/// <param name="share">A <see cref="T:System.IO.FileShare" /> value specifying the type of access other threads have to the file.</param>
		/// <returns>A <see cref="T:System.IO.FileStream" /> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
		[SecurityCritical]
        public static FileStream Open(string filename, FileMode mode, FileAccess access, FileShare share)
        {
            int num;
            switch (access)
            {
                default:
                    num = 131487;
                    break;
                case FileAccess.Write:
                    num = 278;
                    break;
                case FileAccess.Read:
                    num = 131209;
                    break;
            }
            SafeFileHandle safeFileHandle = null;

            try
            {
                FileAccess access_finel = (FileAccess)(((((FileSystemRights)num & FileSystemRights.ReadData) != 0) ? 1 : 0) | ((((FileSystemRights)num & FileSystemRights.WriteData) != 0 || ((FileSystemRights)num & FileSystemRights.AppendData) != 0) ? 2 : 0));

                safeFileHandle = CreateFileCore(filename, null, mode, num, share, true);

                if (safeFileHandle.IsInvalid) ThrowWin32Exception();

                return new FileStream(safeFileHandle, access_finel);
            }
            catch
            {
                NativeMethods.CloseFileHandle(safeFileHandle);
                throw;
            }

        }


        internal static SafeFileHandle CreateFileCore(string filename, FileSecurity fileSecurity, FileMode fileMode, int fileSystemRights, FileShare fileShare, bool checkPath)
        {
            var longFilename = GetWin32LongPath(filename);
            bool flag = fileMode == FileMode.Append;
            if (flag)
            {
                fileMode = FileMode.OpenOrCreate;
                fileSystemRights |= 0x4;
            }
            if (fileSecurity != null)
            {
                fileSystemRights |= 268435456;
            }


            SafeFileHandle safeFileHandle = NativeMethods.CreateFile(longFilename, fileSystemRights, (int)fileShare, IntPtr.Zero, (int)fileMode, 0x80, IntPtr.Zero);

            if (flag)
            {
                bool num = NativeMethods.SetFilePointerEx(safeFileHandle, 0uL, IntPtr.Zero, SeekOrigin.End);
                var lastWin32Error = Marshal.GetLastWin32Error();
                if (!num)
                {
                    if (safeFileHandle != null && !safeFileHandle.IsClosed) // if (safeFileHandle != null && !safeFileHandle.IsClosed && !handle.IsInvalid)
                    {
                        safeFileHandle.Close();
                    }
                    return null;
                }
            }
            return safeFileHandle;


        }



        [DebuggerStepThrough]
        public static void ThrowWin32Exception()
        {
            int code = Marshal.GetLastWin32Error();
            if (code != 0)
            {
                throw new System.ComponentModel.Win32Exception(code);
            }
        }

        public static string GetWin32LongPath(string path)
        {
            if (path.StartsWith(@"\\?\")) return path;

            if (path.StartsWith("\\"))
            {
                path = @"\\?\UNC\" + path.Substring(2);
            }
            else if (path.Contains(":"))
            {
                path = @"\\?\" + path;
            }
            else
            {
                var currdir = Environment.CurrentDirectory;
                path = Combine(currdir, path);
                while (path.Contains("\\.\\")) path = path.Replace("\\.\\", "\\");
                path = @"\\?\" + path;
            }
            return path.TrimEnd('.'); ;
        }

        private static string Combine(string path1, string path2)
        {
            return path1.TrimEnd('\\') + "\\" + path2.TrimStart('\\').TrimEnd('.'); ;
        }


        #endregion

        public static void SetCreationTime(string path, DateTime creationTime)
        {
            long cTime = 0;
            long aTime = 0;
            long wTime = 0;

            using (var handle = GetFileHandleWithWrite(path))
            {
                NativeMethods.GetFileTime(handle, ref cTime, ref aTime, ref wTime);
                var fileTime = creationTime.ToFileTimeUtc();
                if (!NativeMethods.SetFileTime(handle, ref fileTime, ref aTime, ref wTime))
                {
                    throw new Win32Exception();
                }
            }
        }

        public static void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            long cTime = 0;
            long aTime = 0;
            long wTime = 0;

            using (var handle = GetFileHandleWithWrite(path))
            {
                NativeMethods.GetFileTime(handle, ref cTime, ref aTime, ref wTime);

                var fileTime = lastAccessTime.ToFileTimeUtc();
                if (!NativeMethods.SetFileTime(handle, ref cTime, ref fileTime, ref wTime))
                {
                    throw new Win32Exception();
                }
            }
        }

        public static void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            long cTime = 0;
            long aTime = 0;
            long wTime = 0;

            using (var handle = GetFileHandleWithWrite(path))
            {
                NativeMethods.GetFileTime(handle, ref cTime, ref aTime, ref wTime);

                var fileTime = lastWriteTime.ToFileTimeUtc();
                if (!NativeMethods.SetFileTime(handle, ref cTime, ref aTime, ref fileTime))
                {
                    throw new Win32Exception();
                }
            }
        }

        public static DateTime GetLastWriteTime(string path)
        {
            long cTime = 0;
            long aTime = 0;
            long wTime = 0;

            using (var handle = GetFileHandleWithWrite(path))
            {
                NativeMethods.GetFileTime(handle, ref cTime, ref aTime, ref wTime);

                return DateTime.FromFileTimeUtc(wTime);
            }
        }

    }
}
