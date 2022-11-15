using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Encoding.Default          的值为：{Encoding.Default}");
            Console.WriteLine($"Encoding.ASCII            的值为：{Encoding.ASCII}");
            Console.WriteLine($"Encoding.BigEndianUnicode 的值为：{Encoding.BigEndianUnicode}");
            Console.WriteLine($"Encoding.Unicode          的值为：{Encoding.Unicode}");
            Console.WriteLine($"Encoding.UTF8             的值为：{Encoding.UTF8}");

            var enc_936 = Encoding.GetEncoding(936);
            var enc_gb2312 = Encoding.GetEncoding("gb2312");
            var enc_gb18030 = Encoding.GetEncoding("gb18030");
            Console.WriteLine($"Encoding.GetEncoding(936)        的值为：{enc_936}");
            Console.WriteLine($"Encoding.GetEncoding(\"gb2312\")   的值为：{enc_gb2312}");
            Console.WriteLine($"Encoding.GetEncoding(\"gb18030\")  的值为：{enc_gb18030}");

            Console.WriteLine($"Encoding.Default.EncodingName          的值为：{Encoding.Default.EncodingName}");
            Console.WriteLine($"Encoding.ASCII.EncodingName            的值为：{Encoding.ASCII.EncodingName}");
            Console.WriteLine($"Encoding.BigEndianUnicode.EncodingName 的值为：{Encoding.BigEndianUnicode.EncodingName}");
            Console.WriteLine($"Encoding.Unicode.EncodingName          的值为：{Encoding.Unicode.EncodingName}");
            Console.WriteLine($"Encoding.UTF8.EncodingName             的值为：{Encoding.UTF8.EncodingName}");

            var allEncodings= Encoding.GetEncodings();

            Console.ReadKey();
        }
    }
}
