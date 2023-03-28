using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonEffect_Elements.TreeViewEffect
{
    /// <summary>
    /// 编码、解码器
    /// </summary>
    public static class EncoderAndDecoder
    {
        /// <summary>
        /// 从(二进制)文件中读取字符串
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> ReadStringFromFileAsync(string fileName)
        {
            return await ReadStringFromFileAsync(fileName, Encoding.UTF8);
        }
        /// <summary>
        /// 从(二进制)文件中读取字符串
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> ReadStringFromFileAsync(string fileName, Encoding encoding)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            using (var sr = new StreamReader(fileName, encoding))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}
