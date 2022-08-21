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
    /// </summary>
    public static class ZipHelper
    {
        /// <summary>
        /// Zip文件压缩，生成同名的.zip文件
        /// </summary>
        /// <param name="sourceDirOrFileName">源文件或目录</param>
        /// <param name="isWrite">若zip文件存在是否覆盖</param>
        /// <param name="compressionLevel">指定压缩级别，压缩速度还是大小</param>
        /// <returns></returns>
        public static void Compress(string sourceDirOrFileName,bool isWrite=false, CompressionLevel? compressionLevel = null)
        {
            Compress(sourceDirOrFileName, Path.Combine(File.Exists(sourceDirOrFileName) ? Path.GetDirectoryName(sourceDirOrFileName): Directory.GetParent(sourceDirOrFileName).FullName, $"{Path.GetFileNameWithoutExtension(sourceDirOrFileName)}.zip"), isWrite, compressionLevel);
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
            var sourceDirName = sourceDirOrFileName;
            var isDeleteTempDir = false;
            try
            {
                if (File.Exists(sourceDirOrFileName))
                {
                    // 处理文件 创建随机文件夹并
                    var randomDir = Path.Combine( Path.GetDirectoryName(sourceDirOrFileName), Path.GetRandomFileName());
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
                        ZipFile.CreateFromDirectory(sourceDirName, destinationZipFileName, compressionLevel.Value,false);
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
                    if (Directory.Exists(sourceDirName))  Directory.Delete(sourceDirName, true);
                }
            }
        }

        /// <summary>
        /// 向zip压缩档中添加文件或文件夹。暂未实现，基本思路是：遍历读取要添加的文件，同时在压缩档对象ZipArchive创建对应文件项，通过读取原文件流，写入新创建的ZipArchiveEntry。
        /// </summary>
        /// <param name="sourceDirOrFileName"></param>
        /// <param name="destinationZipFileName"></param>
        /// <param name="compressionLevel"></param>
        [Obsolete("未实现",true)]
        public static void CompressAddFile(string sourceDirOrFileName, string destinationZipFileName, CompressionLevel? compressionLevel = null)
        {

        }


        /// <summary>
        /// 解压缩zip文件到当前文件夹
        /// </summary>
        /// <param name="sourceZipFileName">源zip文件</param>
        /// <param name="isWrite">解压目标文件存在时是否覆盖，推荐false</param>
        /// <returns></returns>
        public static void Decompress(string sourceZipFileName,bool isWrite=false)
        {
             Decompress(sourceZipFileName,Path.Combine(Path.GetDirectoryName(sourceZipFileName),Path.GetFileNameWithoutExtension(sourceZipFileName)), isWrite);
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
            try
            {
                ZipFile.ExtractToDirectory(sourceZipFileName, destinationDirName);
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
        /// 解压缩zip中指定模式的文件
        /// </summary>
        /// <param name="sourceZipFileName">源zip文件</param>
        /// <param name="destinationDirName">目标目录</param>
        /// <param name="zipEntryRegPattern">要解压缩的zip内文件名和路径符合的正则模式，比如，".txt$"解压缩txt后缀的文件</param>
        /// <param name="isWrite">解压目标文件存在时是否覆盖，推荐false</param>
        /// <returns></returns>
        public static void Decompress(string sourceZipFileName, string destinationDirName,string zipEntryRegPattern, bool isWrite = false)
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
                    if (existsFiles.Count>0)
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
    }

}
