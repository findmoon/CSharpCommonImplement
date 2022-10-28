using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Compression
{
    /// <summary>
    /// gzip帮助类
    /// </summary>
    public static class GZipHelper
    {
        /// <summary>
        /// gzip压缩字节数组
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> CompressDataAsync(byte[] inputBytes)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (GZipStream gs = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    await gs.WriteAsync(inputBytes, 0, inputBytes.Length);
                }
                return outputStream.ToArray();
            }
        }
        /// <summary>
        /// gzip压缩文本内容为base64格式的文本
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static async Task<string> CompressToBase64Async(string text)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            var outputBytes = await CompressDataAsync(inputBytes);
            
            return Convert.ToBase64String(outputBytes);
        }
        /// <summary>
        /// gzip压缩字节为Base64字符串
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static async Task<string> CompressToBase64Async(byte[] inputBytes)
        {
            var outputBytes = await CompressDataAsync(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }

        /// <summary>
        /// gzip压缩文件，或批量压缩文件夹下的文件，生成指定tar文件或在自定目录下生成压缩文件
        /// </summary>
        /// <param name="sourceDirOrFileName"></param>
        /// <param name="destinationGZipFileOrdirName">压缩目标文件，以.ext.gz结尾，或文件夹</param>
        /// <returns></returns>
        public static async Task CompressFileAsync(string sourceDirOrFileName, string destinationGZipFileOrdirName)
        {
            if (File.Exists(sourceDirOrFileName))
            {
                string destinationGZipFileName;
                if (Directory.Exists(destinationGZipFileOrdirName))
                {
                    destinationGZipFileName=Path.Combine(destinationGZipFileOrdirName,Path.GetFileName(sourceDirOrFileName) +".gz");
                }
                else
                {
                    var endStr = $"{Path.GetExtension(sourceDirOrFileName)}.gz";
                    destinationGZipFileName = destinationGZipFileOrdirName.EndsWith(endStr) ? destinationGZipFileOrdirName : $"{destinationGZipFileOrdirName}.{endStr}";
                }
                if (File.Exists(destinationGZipFileName))
                {
                    throw new Exception("目标压缩文件已存在");
                }
                if (!Directory.Exists(Path.GetDirectoryName(destinationGZipFileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationGZipFileName));
                }

                if (File.Exists(destinationGZipFileName))
                {
                    throw new Exception("压缩的目标文件存在");
                }
                using (var compressedFileStream = File.Create(destinationGZipFileName))
                {
                    using (GZipStream gs = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        using (var sourceFileStream = File.OpenRead(sourceDirOrFileName))
                        {
                            await sourceFileStream.CopyToAsync(gs);
                        }
                    }
                }
            }
            else if (Directory.Exists(sourceDirOrFileName))
            {
                if (File.Exists(destinationGZipFileOrdirName))
                {
                    throw new Exception("从源压缩文件夹批量压缩时，压缩目标不能是文件，因为要批量压缩到一个文件夹中");
                }
                foreach (var sourceFileName in Directory.GetFiles(sourceDirOrFileName))
                {
                    await CompressFileAsync(sourceFileName, destinationGZipFileOrdirName);
                }
            }
            else
            {
                throw new Exception("源文件不是有效的文件(或不存在)");
            }
        }
        /// <summary>
        /// gzip压缩文件，或批量压缩文件夹下的文件，同一目录下生成同名的.gz文件
        /// </summary>
        /// <param name="sourceDirOrFileName"></param>
        /// <returns></returns>
        public static async Task CompressFileAsync(string sourceDirOrFileName)
        {
            if (File.Exists(sourceDirOrFileName))
            {
                var destinationGZipFileName = sourceDirOrFileName + ".gz";
                if (File.Exists(destinationGZipFileName))
                {
                    throw new Exception("压缩的目标文件存在");
                }
                using (var compressedFileStream = File.Create(destinationGZipFileName))
                {
                    using (GZipStream gs = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        using (var sourceFileStream=File.OpenRead(sourceDirOrFileName))
                        {
                            await sourceFileStream.CopyToAsync(gs);
                        }
                    }
                }
            }
            else if (Directory.Exists(sourceDirOrFileName))
            {
                foreach (var sourceFileName in Directory.GetFiles(sourceDirOrFileName))
                {
                    await CompressFileAsync(sourceFileName);
                }
            }
            else
            {
                throw new Exception("源文件不是有效的路径(或不存在)");
            }
        }

        /// <summary>
        /// gzip解压缩字节数组
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> DeCompressDataAsync(byte[] gzBytes)
        {
            using (MemoryStream gzStream = new MemoryStream(gzBytes))
            {
                using (GZipStream gs = new GZipStream(gzStream, CompressionMode.Decompress))
                {
                    using (var outputStream=new MemoryStream())
                    {
                        await gs.CopyToAsync(outputStream);
                        return outputStream.ToArray();
                    }
                }
            }
        }
        /// <summary>
        /// gzip解压缩base64文本内容
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static async Task<string> DeCompressFromBase64Async(string base64Text)
        {
            byte[] inputBytes = Convert.FromBase64String(base64Text);
            var outputBytes = await DeCompressDataAsync(inputBytes);
            return Encoding.UTF8.GetString(outputBytes);
        }
        /// <summary>
        /// gzip解压缩base64文本内容
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> DeCompressBytesFromBase64Async(string base64Text)
        {
            byte[] inputBytes = Convert.FromBase64String(base64Text);
            return await DeCompressDataAsync(inputBytes);
        }

        /// <summary>
        /// gzip解压缩文件，或批量解压缩文件夹下的gz文件，到指定文件或文件夹内
        /// </summary>
        /// <param name="sourceDirOrFileName"></param>
        /// <param name="destinationFileOrdirName"></param>
        /// <returns></returns>
        public static async Task DeCompressFileAsync(string sourceGZipFileOrDirName,string destinationFileOrdirName)
        {
            if (File.Exists(sourceGZipFileOrDirName))
            {
                if (!sourceGZipFileOrDirName.EndsWith(".gz"))
                {
                    throw new Exception("非.gz后缀格式文件");
                }
                string destinationFileName;
                if (Directory.Exists(destinationFileOrdirName))
                {
                    destinationFileName = Path.Combine(destinationFileOrdirName,Path.GetFileName(sourceGZipFileOrDirName.Remove(sourceGZipFileOrDirName.Length - 3)));
                }
                else
                {
                    var endStr = Path.GetExtension(sourceGZipFileOrDirName.Remove(sourceGZipFileOrDirName.Length - 3));
                    destinationFileName = destinationFileOrdirName.EndsWith(endStr) ? destinationFileOrdirName : $"{destinationFileOrdirName}.{endStr}";
                }
                if (File.Exists(destinationFileName))
                {
                    throw new Exception("目标解压缩文件已存在");
                }
                if (!Directory.Exists(Path.GetDirectoryName(destinationFileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFileName));
                }

                using (var compressedFileStream = File.OpenRead(sourceGZipFileOrDirName))
                {
                    using (GZipStream gs = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        using (var destFileStream = File.Create(destinationFileName))
                        {
                            await gs.CopyToAsync(destFileStream);
                        }
                    }
                }
            }
            else if (Directory.Exists(sourceGZipFileOrDirName))
            {
                if (File.Exists(destinationFileOrdirName))
                {
                    throw new Exception("从源压缩文件夹批量解压缩时，解压缩目标不能是文件，因为要批量解压缩到一个文件夹中");
                }
                foreach (var sourceFileName in Directory.GetFiles(sourceGZipFileOrDirName))
                {
                    if (sourceFileName.EndsWith(".gz"))
                    {
                        await DeCompressFileAsync(sourceFileName, destinationFileOrdirName);
                    }
                }
            }
            else
            {
                throw new Exception("源文件不是有效的路径(或不存在)");
            }
        }
        /// <summary>
        /// gzip解压缩文件，或批量解压缩文件夹下的gz文件
        /// </summary>
        /// <param name="sourceDirOrFileName"></param>
        /// <returns></returns>
        public static async Task DeCompressFileAsync(string sourceGZipFileOrDirName)
        {
            if (File.Exists(sourceGZipFileOrDirName))
            {
                if (!sourceGZipFileOrDirName.EndsWith(".gz"))
                {
                    throw new Exception("非.gz后缀格式文件");
                }
                var destinationFileName = sourceGZipFileOrDirName.Remove(sourceGZipFileOrDirName.Length - 3);
                if (File.Exists(destinationFileName))
                {
                    throw new Exception("目标解压缩文件已存在");
                }
                using (var compressedFileStream = File.OpenRead(sourceGZipFileOrDirName))
                {
                    using (GZipStream gs = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        using (var destFileStream = File.Create(destinationFileName))
                        {
                            await gs.CopyToAsync(destFileStream);
                        }
                    }
                }
            }
            else if (Directory.Exists(sourceGZipFileOrDirName))
            {
                foreach (var sourceFileName in Directory.GetFiles(sourceGZipFileOrDirName))
                {
                    if (sourceFileName.EndsWith(".gz"))
                    {
                        await DeCompressFileAsync(sourceFileName);
                    }
                }
            }
            else
            {
                throw new Exception("源文件不是有效的路径(或不存在)");
            }
        }
    
    }
}
