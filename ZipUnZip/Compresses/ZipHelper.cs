using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.IO.Compression
{
    /// <summary>
    /// ZipHelper类，zip文件的压缩解压缩(UTF-8编码，如果需要指定其他编码，请使用ZipFile的原生方法)
    /// 直接使用Task.Run()包装的异步方法，并不是推荐的做法
    /// </summary>
    public static class ZipHelper
    {
        #region 同步压缩
        /// <summary>
        /// Zip文件压缩，生成同名的.zip文件
        /// </summary>
        /// <param name="sourceDirOrFileName">源文件或目录</param>
        /// <param name="isWrite">若zip文件存在是否覆盖</param>
        /// <param name="compressionLevel">指定压缩级别，压缩速度还是大小</param>
        /// <returns></returns>
        public static void Compress(string sourceDirOrFileName, bool isWrite = false, CompressionLevel? compressionLevel = null)
        {
            Compress(sourceDirOrFileName, Path.Combine(File.Exists(sourceDirOrFileName) ? Path.GetDirectoryName(sourceDirOrFileName) : Directory.GetParent(sourceDirOrFileName).FullName, $"{Path.GetFileNameWithoutExtension(sourceDirOrFileName)}.zip"), isWrite, compressionLevel);
        }

        /// <summary>
        /// Zip文件压缩，生成指定文件名的.zip文件
        /// </summary>
        /// <param name="sourceDirOrFileName">源文件或目录</param>
        /// <param name="destinationZipFileName">压缩包文件名(含.zip后缀的文件名)</param>
        /// <param name="isWrite">若zip文件存在是否覆盖</param>
        /// <param name="compressionLevel">指定压缩级别，压缩速度还是大小</param>
        /// <returns></returns>
        public static void Compress(string sourceDirOrFileName, string destinationZipFileName, bool isWrite = false, CompressionLevel? compressionLevel = null)
        {
            Compress_Inner(sourceDirOrFileName, destinationZipFileName, isWrite, compressionLevel);
        }

        /// <summary>
        /// 向zip压缩档中添加文件或文件夹。暂未实现，基本思路是：遍历读取要添加的文件，同时在压缩档对象ZipArchive创建对应文件项，通过读取原文件流，写入新创建的ZipArchiveEntry。
        /// 更正确的处理应该是，使用 ZipFileExtensions 的 ZipArchive 扩展方法CreateEntryFromFile()，直接从文件创建
        /// </summary>
        /// <param name="sourceDirOrFileName"></param>
        /// <param name="destinationZipFileName"></param>
        /// <param name="compressionLevel"></param>
        [Obsolete("未实现", true)]
        public static void CompressAddFile(string sourceDirOrFileName, string destinationZipFileName, CompressionLevel? compressionLevel = null)
        {

        }
        #endregion
        #region 异步压缩
        /// <summary>
        /// Zip文件压缩，生成同名的.zip文件
        /// </summary>
        /// <param name="sourceDirOrFileName">源文件或目录</param>
        /// <param name="isWrite">若zip文件存在是否覆盖</param>
        /// <param name="compressionLevel">指定压缩级别，压缩速度还是大小</param>
        /// <returns></returns>
        public static async Task CompressAsync(string sourceDirOrFileName, bool isWrite = false, CompressionLevel? compressionLevel = null)
        {
            await CompressAsync(sourceDirOrFileName, Path.Combine(File.Exists(sourceDirOrFileName) ? Path.GetDirectoryName(sourceDirOrFileName) : Directory.GetParent(sourceDirOrFileName).FullName, $"{Path.GetFileNameWithoutExtension(sourceDirOrFileName)}.zip"), isWrite, compressionLevel);
        }

        /// <summary>
        /// Zip文件压缩，生成指定文件名的.zip文件
        /// </summary>
        /// <param name="sourceDirOrFileName">源文件或目录</param>
        /// <param name="destinationZipFileName">压缩包文件名(含.zip后缀的文件名)</param>
        /// <param name="isWrite">若zip文件存在是否覆盖</param>
        /// <param name="compressionLevel">指定压缩级别，压缩速度还是大小</param>
        /// <returns></returns>
        public static async Task CompressAsync(string sourceDirOrFileName, string destinationZipFileName, bool isWrite = false, CompressionLevel? compressionLevel = null)
        {
            await Task.Run(() =>
            {
                Compress_Inner(sourceDirOrFileName, destinationZipFileName, isWrite, compressionLevel);
            });
        }
        #endregion

        #region 同步解压方法
        /// <summary>
        /// 解压缩zip文件到当前文件夹
        /// </summary>
        /// <param name="sourceZipFileName">源zip文件</param>
        /// <param name="isWrite">解压目标文件存在时是否覆盖，推荐false</param>
        /// <returns></returns>
        public static void Decompress(string sourceZipFileName, bool isWrite = false)
        {
            Decompress(sourceZipFileName, Path.Combine(Path.GetDirectoryName(sourceZipFileName), Path.GetFileNameWithoutExtension(sourceZipFileName)), isWrite);
        }
        /// <summary>
        /// 解压缩zip文件
        /// </summary>
        /// <param name="sourceZipFileName">源zip文件</param>
        /// <param name="destinationDirName">目标目录</param>
        /// <param name="isWrite">解压目标文件存在时是否覆盖，推荐false</param>
        /// <returns></returns>
        public static void Decompress(string sourceZipFileName, string destinationDirName, bool isWrite = false)
        {
            Decompress_Inner(sourceZipFileName, destinationDirName, isWrite);
        }

        /// <summary>
        /// 解压缩zip中指定模式的文件
        /// </summary>
        /// <param name="sourceZipFileName">源zip文件</param>
        /// <param name="destinationDirName">目标目录</param>
        /// <param name="zipEntryRegPattern">要解压缩的zip内文件名和路径符合的正则模式，比如，".txt$"解压缩txt后缀的文件</param>
        /// <param name="isWrite">解压目标文件存在时是否覆盖，推荐false</param>
        /// <returns></returns>
        public static void Decompress(string sourceZipFileName, string destinationDirName, string zipEntryRegPattern, bool isWrite = false)
        {
            Decompress_Inner(sourceZipFileName, destinationDirName, zipEntryRegPattern, isWrite);
        }
        #endregion

        #region 异步解压方法
        /// <summary>
        /// 解压缩zip文件到当前文件夹
        /// </summary>
        /// <param name="sourceZipFileName">源zip文件</param>
        /// <param name="isWrite">解压目标文件存在时是否覆盖，推荐false</param>
        /// <returns></returns>
        public static async Task DecompressAsync(string sourceZipFileName, bool isWrite = false)
        {
            await DecompressAsync(sourceZipFileName, Path.Combine(Path.GetDirectoryName(sourceZipFileName), Path.GetFileNameWithoutExtension(sourceZipFileName)), isWrite);
        }
        /// <summary>
        /// 解压缩zip文件。为防止文件名或路径名过长导致System.IO.PathTooLongException(260/248个字符限制)错误，推荐使用 DecompressAsync2 方法
        /// </summary>
        /// <param name="sourceZipFileName">源zip文件</param>
        /// <param name="destinationDirName">目标目录</param>
        /// <param name="isWrite">解压目标文件存在时是否覆盖，推荐false</param>
        /// <returns></returns>
        public static async Task DecompressAsync(string sourceZipFileName, string destinationDirName, bool isWrite = false)
        {
            await Task.Run(() =>
            {
                Decompress_Inner(sourceZipFileName, destinationDirName, isWrite);
            });
        }

        /// <summary>
        /// 解压缩zip中指定模式的文件
        /// </summary>
        /// <param name="sourceZipFileName">源zip文件</param>
        /// <param name="destinationDirName">目标目录</param>
        /// <param name="zipEntryRegPattern">要解压缩的zip内文件名和路径符合的正则模式，比如，".txt$"解压缩txt后缀的文件</param>
        /// <param name="isWrite">解压目标文件存在时是否覆盖，推荐false</param>
        /// <returns></returns>
        public static async Task DecompressAsync(string sourceZipFileName, string destinationDirName, string zipEntryRegPattern, bool isWrite = false)
        {
            await Task.Run(() =>
            {
                Decompress_Inner(sourceZipFileName, destinationDirName, zipEntryRegPattern, isWrite);
            });
        }
        #endregion

        #region 内部方法，由适应异步提取出来
        private static void Compress_Inner(string sourceDirOrFileName, string destinationZipFileName, bool isWrite = false, CompressionLevel? compressionLevel = null)
        {
            var sourceDirName = sourceDirOrFileName;
            var isDeleteTempDir = false;
            try
            {
                if (File.Exists(sourceDirOrFileName))
                {
                    // 处理文件 创建随机文件夹并
                    var randomDir = Path.Combine(Path.GetDirectoryName(sourceDirOrFileName), Path.GetRandomFileName());
                    while (Directory.Exists(randomDir))
                    {
                        randomDir = Path.Combine(Path.GetDirectoryName(sourceDirOrFileName), Path.GetRandomFileName());
                    }
                    var di = Directory.CreateDirectory(randomDir);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    File.Copy(sourceDirOrFileName, Path.Combine(randomDir, Path.GetFileName(sourceDirOrFileName)));
                    sourceDirName = randomDir;
                    isDeleteTempDir = true;
                }

                var isCompress = true;
                if (File.Exists(destinationZipFileName))
                {
                    if (isWrite)//覆盖并压缩
                    {
                        File.Delete(destinationZipFileName);
                    }
                    else
                    {
                        isCompress = false;
                    }
                }
                if (isCompress)
                {
                    if (!destinationZipFileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        destinationZipFileName += ".zip";
                    }
                    if (compressionLevel.HasValue)
                    {
                        ZipFile.CreateFromDirectory(sourceDirName, destinationZipFileName, compressionLevel.Value, false);
                    }
                    else
                    {
                        ZipFile.CreateFromDirectory(sourceDirName, destinationZipFileName);
                    }
                }
            }
            finally
            {

                if (isDeleteTempDir) // 删除创建的随机文件夹
                {
                    if (Directory.Exists(sourceDirName)) Directory.Delete(sourceDirName, true);
                }
            }
        }


        private static void Decompress_Inner(string sourceZipFileName, string destinationDirName, string zipEntryRegPattern, bool isWrite = false)
        {
            var existsFiles = new List<string>();
            // 判断目标文件是否存在
            using (ZipArchive archive = ZipFile.OpenRead(sourceZipFileName)) // 需要添加System.IO.Compression(.dll)引用
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    var m = Regex.Match(entry.FullName, zipEntryRegPattern);
                    if (m.Success)
                    {
                        if (File.Exists(Path.Combine(destinationDirName, entry.FullName)))
                        {
                            existsFiles.Add(entry.FullName);
                        }
                    }
                }
                if (existsFiles.Count > 0)
                {
                    if (!isWrite)
                    {
                        throw new Exception($"解压目录中存在同名文件，请确认后再解压缩。【{string.Join(";", existsFiles)}】");
                    }
                }

                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    var m = Regex.Match(entry.FullName, zipEntryRegPattern);
                    if (m.Success)
                    {
                        entry.ExtractToFile(Path.Combine(destinationDirName, entry.FullName), true);
                    }
                }
            }
        }

        private static void Decompress_Inner(string sourceZipFileName, string destinationDirName, bool isWrite = false)
        {
            try
            {
                ZipFile.ExtractToDirectory(sourceZipFileName, destinationDirName); // 不支持@"\\?\"长路径处理
            }
            catch (IOException ex)
            {
                var isThrow = true;
                if (isWrite)
                {
                    // 判断目标文件是否存在
                    using (ZipArchive archive = ZipFile.OpenRead(sourceZipFileName)) // 需要添加System.IO.Compression(.dll)引用
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (File.Exists(Path.Combine(destinationDirName, entry.FullName)))
                            {
                                File.Delete(Path.Combine(destinationDirName, entry.FullName));
                                isThrow = false;
                            }
                        }
                    }
                    if (!isThrow) ZipFile.ExtractToDirectory(sourceZipFileName, destinationDirName);
                }
                if (isThrow) throw ex;
            }
        }
        /// <summary>
        /// 支持长路径的解压处理(文件名不小于260字符或路径不小于248字符) 
        /// 参考entry.ExtractToFile()方法的源码实现，写入文件时没有区分二进制或文本，而是直接使用Stream写入到文件
        /// </summary>
        /// <param name="sourceZipFileName"></param>
        /// <param name="destinationDirName"></param>
        /// <param name="zipEntryRegPattern"></param>
        /// <param name="isWrite"></param>
        public static async Task DecompressAsync2(string sourceZipFileName, string destinationDirName, bool isWrite = false)
        {
            await Task.Run(() =>
            {
                var existsFiles = new List<string>();
                // 判断目标文件是否存在
                using (ZipArchive archive = ZipFile.OpenRead(sourceZipFileName)) // 需要添加System.IO.Compression(.dll)引用
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (string.IsNullOrEmpty(entry.Name))  // 文件夹时entry.Name为空，无法提取
                        {
                            continue;
                        }
                        var extraToFileName = Path.Combine(destinationDirName, entry.FullName);
                        var extraToDir = Path.GetDirectoryName(extraToFileName); // 获取时会转为正确的路径分隔符
                        if (!CMCode.IO.Directory.Exists(extraToDir))
                        {
                            CMCode.IO.Directory.CreateDirectory(extraToDir);
                        }


           
                        if (extraToFileName.Length >= 260) // 长路径处理
                        {
                            //entry.ExtractToFile;
                            // 处理window下路径分隔符  entry.FullName中的分隔符为unix风格的/，Win API直接处理会报错格式不正确
                            if (Path.DirectorySeparatorChar=='\\')
                            {
                                extraToFileName = extraToFileName.Replace('/', Path.DirectorySeparatorChar);
                            }
                            

                            FileMode mode = (!isWrite) ? FileMode.CreateNew : FileMode.Create;

                            #region 参考原始使用FileStream，Win API处理长路径
                            //Alphaleonis.Win32.Filesystem.File.Open(extraToFileName,mode, FileAccess.Write, FileShare.None);
                            //File.Open(extraToFileName, mode, FileAccess.Write, FileShare.None);
                            using (var fileStream = CMCode.IO.File.Open(extraToFileName, mode, FileAccess.Write, FileShare.None))
                            {
                                using (Stream stream = entry.Open())
                                {
                                    stream.CopyTo(fileStream);
                                }
                            }
                            CMCode.IO.File.SetLastWriteTime(extraToFileName, entry.LastWriteTime.DateTime);
                            #endregion

                            #region 读取流，最终落实到文件，还是要考虑是二进制还是文本，但同时File.Open()打开或创建文件流的原生处理中还是没有长路径的解决，还是要用上面的方法
                            //using (StreamReader reader = new StreamReader(entry.Open()))
                            //{
                            //    var result=reader.ReadToEnd();
                            //} 
                            #endregion

                        }
                        else
                        {
                            entry.ExtractToFile(extraToFileName, isWrite);
                        }
                    }
                }
            });
        }


        #region ZipArchiveEntry.ExtractToFile到文件源码
        //public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName, bool overwrite)
        //{
        //    if (source == null)
        //    {
        //        throw new ArgumentNullException("source");
        //    }
        //    if (destinationFileName == null)
        //    {
        //        throw new ArgumentNullException("destinationFileName");
        //    }
        //    FileMode mode = (!overwrite) ? FileMode.CreateNew : FileMode.Create;
        //    using (Stream destination = File.Open(destinationFileName, mode, FileAccess.Write, FileShare.None))
        //    {
        //        using (Stream stream = source.Open())
        //        {
        //            stream.CopyTo(destination);
        //        }
        //    }
        //    File.SetLastWriteTime(destinationFileName, source.get_LastWriteTime().DateTime);
        //} 
        #endregion
        #endregion
    }

}
