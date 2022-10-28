using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Compression
{
    #region 测试压、解正常
    ///// <summary>
    ///// 简单的压缩
    ///// </summary>
    //public static class DeflateHelper
    //{
    //    /// <summary>
    //    /// 压缩字符串
    //    /// </summary>
    //    /// <param name="str"></param>
    //    /// <returns></returns>
    //    public static byte[] CompressString(string str)
    //    {
    //        return CompressBytes(Encoding.UTF8.GetBytes(str));
    //    }

    //    /// <summary>
    //    /// 压缩二进制
    //    /// </summary>
    //    /// <param name="str"></param>
    //    /// <returns></returns>
    //    public static byte[] CompressBytes(byte[] str)
    //    {
    //        var ms = new MemoryStream(str) { Position = 0 };
    //        var outms = new MemoryStream();
    //        using (var deflateStream = new DeflateStream(outms, CompressionMode.Compress, true))
    //        {
    //            var buf = new byte[1024];
    //            int len;
    //            while ((len = ms.Read(buf, 0, buf.Length)) > 0)
    //                deflateStream.Write(buf, 0, len);
    //        }
    //        return outms.ToArray();
    //    }
    //    /// <summary>
    //    /// 解压字符串
    //    /// </summary>
    //    /// <param name="str"></param>
    //    /// <returns></returns>
    //    public static string DecompressString(byte[] str)
    //    {
    //        return Encoding.UTF8.GetString(DecompressBytes(str));
    //    }
    //    /// <summary>
    //    /// 解压二进制
    //    /// </summary>
    //    /// <param name="str"></param>
    //    /// <returns></returns>
    //    public static byte[] DecompressBytes(byte[] str)
    //    {
    //        var ms = new MemoryStream(str) { Position = 0 };
    //        var outms = new MemoryStream();
    //        using (var deflateStream = new DeflateStream(ms, CompressionMode.Decompress, true))
    //        {
    //            var buf = new byte[1024];
    //            int len;
    //            while ((len = deflateStream.Read(buf, 0, buf.Length)) > 0)
    //                outms.Write(buf, 0, len);
    //        }
    //        return outms.ToArray();
    //    }
    //} 
    #endregion
    #region 帮助类
    /// <summary>
    /// Deflate压缩算法帮助类
    /// </summary>
    public static class DeflateHelper
    {
        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static async Task<byte[]> CompressStringAsync(string str)
        {
            return await CompressBytesAsync(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// 压缩二进制
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> CompressBytesAsync(byte[] inputBytes)
        {
            using (var inMs = new MemoryStream(inputBytes))
            //{
            // { Position = 0 };
            using (var outMs = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(outMs, CompressionMode.Compress, true))
                {
                    #region 重新读取字节。。。多余
                    //var buf = new byte[1024];
                    //int len;
                    //while ((len = inMs.Read(buf, 0, buf.Length)) > 0)
                    //    deflateStream.Write(buf, 0, len); 
                    #endregion
                    await deflateStream.WriteAsync(inputBytes, 0, inputBytes.Length);
                    //deflateStream.Write(inputBytes, 0, inputBytes.Length); 
                }
                // 必须放在DeflateStream后放回，否则回去的字节数组为0。返回空
                return outMs.ToArray();
            }
            //}
        }
        /// <summary>
        /// 解压字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static async Task<string> DecompressStringAsync(byte[] strBytes)
        {
            return Encoding.UTF8.GetString(await DecompressBytesAsync(strBytes));
        }
        /// <summary>
        /// 解压二进制
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static async Task<byte[]> DecompressBytesAsync(byte[] inputBytes)
        {
            //var ms = new MemoryStream(inputBytes) { Position = 0 };
            //var outms = new MemoryStream();
            //using (var deflateStream = new DeflateStream(ms, CompressionMode.Decompress, true))
            //{
            //    var buf = new byte[1024];
            //    int len;
            //    while ((len = deflateStream.Read(buf, 0, buf.Length)) > 0)
            //        outms.Write(buf, 0, len);
            //}
            //return outms.ToArray();
            using (var inputStream = new MemoryStream(inputBytes))
            {
                using (var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress, true))
                {
                    using (var outStream = new MemoryStream())
                    {
                        await deflateStream.CopyToAsync(outStream);
                        return outStream.ToArray();
                    }
                }
            }
        }


        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="OriginalFileName"></param>
        /// <param name="CompressedFileName"></param>
        public static async Task CompressFileAsync(string OriginalFileName, string CompressedFileName)
        {
            if (File.Exists(CompressedFileName))
            {
                throw new Exception("压缩目标文件已存在");
            }
            using (FileStream originalFileStream = File.Open(OriginalFileName, FileMode.Open))
            {
                using (FileStream compressedFileStream = File.Create(CompressedFileName))
                {
                    using (var compressor = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                    {
                        await originalFileStream.CopyToAsync(compressor);
                    }
                }
            }
        }
        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="CompressedFileName"></param>
        /// <param name="DecompressedFileName"></param>
        public static async Task DeCompressFileAsync(string CompressedFileName, string DecompressedFileName)
        {
            if (File.Exists(DecompressedFileName))
            {
                throw new Exception("解压缩目标文件已存在");
            }
            using (FileStream compressedFileStream = File.Open(CompressedFileName, FileMode.Open))
            {
                using (FileStream outputFileStream = File.Create(DecompressedFileName))
                {
                    using (var decompressor = new DeflateStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        await decompressor.CopyToAsync(outputFileStream);
                    }
                }
            }
        }
    }
    #endregion

}
