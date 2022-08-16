using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Text
{
    public static class RandomStr
    {
        /// <summary>
        /// 从字符串中获取随机内容
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomContent(this string str, int length)
        {
            return str.GetRandomContent(length, length);
        }
        /// <summary>
        /// 从字符串中获取随机内容
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomContent(this string str, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            DealMinMaxLength(ref minLength, ref maxLength);
            var random = new Random();
            var strLength = random.Next(minLength, maxLength + 1);
            var strBuilder = new StringBuilder();
            for (int i = 0; i < strLength; i++)
            {
                strBuilder.Append(str[random.Next(str.Length)]);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 获取固定长度的随机大写字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomUpperCase(int length)
        {
            return GetRandomUpperCase(length, length);
        }
        /// <summary>
        /// 获取随机大写字母
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomUpperCase(int minLength, int maxLength)
        {
            DealMinMaxLength(ref minLength, ref maxLength);
            var random = new Random();
            var strLength = random.Next(minLength, maxLength + 1);
            var strBuilder = new StringBuilder();
            for (int i = 0; i < strLength; i++)
            {
                // 生成随机字符
                //random.Next(0,26);
                var letter = Convert.ToChar(random.Next(26) + 65);
                strBuilder.Append(letter);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 获取固定长度的随机大写字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomUpperCase2(int length)
        {
            return GetRandomUpperCase2(length, length);
        }
        /// <summary>
        /// 获取随机大写字母
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomUpperCase2(int minLength, int maxLength)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".GetRandomContent(minLength, maxLength);
        }



        /// <summary>
        /// 获取固定长度的随机小写字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomLowerCase(int length)
        {
            return GetRandomLowerCase(length, length);
        }
        /// <summary>
        /// 获取随机小写字母
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomLowerCase(int minLength, int maxLength)
        {
            DealMinMaxLength(ref minLength, ref maxLength);
            var random = new Random();
            var strLength = random.Next(minLength, maxLength + 1);
            var strBuilder = new StringBuilder();
            for (int i = 0; i < strLength; i++)
            {
                // 生成随机字符
                strBuilder.Append(Convert.ToChar(random.Next(26) + 97));
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 获取固定长度的随机小写字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomLowerCase2(int length)
        {
            return GetRandomLowerCase2(length, length);
        }
        /// <summary>
        /// 获取随机小写字母
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomLowerCase2(int minLength, int maxLength)
        {
            return "abcdefghijklmnopqrstuvwxyz".GetRandomContent(minLength, maxLength);
        }


        /// <summary>
        /// 获取固定长度的随机大小写字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomLetter(int length)
        {
            return GetRandomLetter(length, length);
        }
        /// <summary>
        /// 获取随机大小写字母
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomLetter(int minLength, int maxLength)
        {
            DealMinMaxLength(ref minLength, ref maxLength);
            var random = new Random();
            var strLength = random.Next(minLength, maxLength + 1);
            var strBuilder = new StringBuilder();
            char letter;
            for (int i = 0; i < strLength; i++)
            {
                // 生成随机字符
                var letterNum = random.Next(26);
                if (random.Next(10) >= 5)
                {
                    letter = Convert.ToChar(letterNum + 97);
                }
                else
                {
                    letter = Convert.ToChar(letterNum + 65);
                }
                strBuilder.Append(letter);
            }
            return strBuilder.ToString();
        }
        /// <summary>
        /// 获取固定长度的随机大小写字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomLetter2(int length)
        {
            return GetRandomLetter2(length, length);
        }
        /// <summary>
        /// 获取随机大小写字母
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomLetter2(int minLength, int maxLength)
        {
            return "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".GetRandomContent(minLength, maxLength);
        }

        /// <summary>
        /// 获取固定长度的随机大小写字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomAlphanumeric(int length)
        {
            return GetRandomAlphanumeric(length, length);
        }
        /// <summary>
        /// 获取随机大小写字母
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomAlphanumeric(int minLength, int maxLength)
        {
            DealMinMaxLength(ref minLength, ref maxLength);
            var random = new Random();
            var strLength = random.Next(minLength, maxLength + 1);
            var strBuilder = new StringBuilder();
            for (int i = 0; i < strLength; i++)
            {
                // 生成随机字符
                //random.Next(0,26);
                var letterNum = random.Next(26);
                var type = random.Next(3);
                if (type == 0)
                {
                    strBuilder.Append(Convert.ToChar(letterNum + 97));
                }
                else if (type == 1)
                {
                    strBuilder.Append(Convert.ToChar(letterNum + 65));
                }
                else
                {
                    strBuilder.Append(random.Next(10));
                }
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 获取固定长度的随机大小写字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomAlphanumeric2(int length)
        {
            return GetRandomAlphanumeric2(length, length);
        }
        /// <summary>
        /// 获取随机大小写字母
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetRandomAlphanumeric2(int minLength, int maxLength)
        {
            return "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".GetRandomContent(minLength, maxLength);
        }


        /// <summary>
        /// 获取随机中文字符
        /// </summary>
        /// <param name="length">字符长度</param>
        /// <param name="isAllZH">是否包含GB2312中所有文字，默认不包含不常用字</param>
        /// <returns></returns>
        public static string GetRandomGBStr(int length, bool isAllZH = false)
        {
            return GetRandomGBStr(length, length, isAllZH);
        }
        /// <summary>
        /// 获取随机中文字符
        /// </summary>
        /// <param name="minLength">随机中文字符的最小长度</param>
        /// <param name="maxLength">随机中文字符的最大长度</param>
        /// <param name="isAll">是否包含GB2312中所有文字，默认不包含不常用字</param>
        /// <returns></returns>
        public static string GetRandomGBStr(int minLength, int maxLength, bool isAllZH = false)
        {
            DealMinMaxLength(ref minLength, ref maxLength);
            var minRegionCode = 16;
            var maxRegionCode = 55;
            if (isAllZH)
            {
                maxRegionCode = 87;
            }
            var resultBytes = new List<byte>();

            var ran = new Random();
            var count = ran.Next(minLength, maxLength + 1);
            for (int i = 0; i < count; i++)
            {
                var regionCode = ran.Next(minRegionCode, maxRegionCode + 1);
                var positionCode = 0;
                if (regionCode == 55) //  处理特殊的55区最后5个字符，55区的90,91,92,93,94为空，没有汉字
                {
                    positionCode = ran.Next(1, 90);
                }
                else
                {
                    positionCode = ran.Next(1, 94 + 1);
                }

                resultBytes.Add(Convert.ToByte(regionCode + 160)); // (byte)(regionCode + 160)
                resultBytes.Add(Convert.ToByte(positionCode + 160)); // (byte)(positionCode + 160)
            }
            var gb = Encoding.GetEncoding("gb2312");
            return gb.GetString(resultBytes.ToArray());
        }

        /// <summary>
        /// 此函数在汉字编码范围内随机创建含两个元素的十六进制字节数组，每个字节数组代表一个汉字，并将
        /// 四个字节数组存储在object数组中。
        /// </summary>
        /// <param name="strlength">产生的汉字个数</param>
        /// <returns></returns>
        [Obsolete("未考虑判断并忽略55区的最后5个位码字符")]
        public static string CreateRandomBGStr(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new string[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };


            //定义一个object数组用来
            var bytes = new byte[strlength * 2];

            /**//*每循环一次产生一个含两个元素的十六进制字节，其代表汉字的区码和位码
             每个汉字有1个区位码（2个字节）组成，4个16进制位。
                第1位和第2位作为第一个字节；第3位和第4位作为第二个字节
            */
            Random rnd = new Random();
            for (int i = 0; i < strlength; i++)
            {
                //第1位 范围为 [b~d] 如果包含二级汉字，范围为[b~f]
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1];

                //第2位 范围为 [0~f]，只有当第一位为d时，此值为[0~7]；如果包含二级汉字，第一位为f是，此值范围为[0~7]
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的种子避免产生重复值
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7 + 1);  // 原代码为 r2 = rnd.Next(0, 7); 但是7也应该取到
                }
                else
                {
                    r2 = rnd.Next(0, 15 + 1);
                }
                string str_r2 = rBase[r2];

                //第3位 (位码 1~94) 取值为[a~f]
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3];

                //第4位 (位码 1~94) 取值为[0~f] ;第三位取值a时，第四位为[1~f];第三位取值f时，第四位为[1~e];
                // 此处未考虑判断并忽略55区的最后5个位码字符
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4];

                //定义两个字节变量存储产生的随机 16进制汉字区位码 
                byte byte_region = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte_position = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量分别因此添加到字节数组中
                bytes[2 * i] = byte_region;
                bytes[2 * i + 1] = byte_position;

            }

            var gb = Encoding.GetEncoding("gb2312");
            return gb.GetString(bytes);
        }


        /// <summary>
        /// 获取GB2312中所有中文
        /// </summary>
        /// <param name="isAllZH">是否包含所有中文，默认不包含不常用字</param>
        /// <returns></returns>
        public static string GetBG2312ZH(bool isAllZH = false)
        {
            var minRegionCode = 16;
            var maxRegionCode = 55;
            if (isAllZH)
            {
                maxRegionCode = 87;
            }
            var strBuild = new StringBuilder();
            var gb = Encoding.GetEncoding("gb2312");
            for (int regionCode = minRegionCode; regionCode <= maxRegionCode; regionCode++)
            {
                var endPositionCode = 94;
                if (regionCode == 55)
                {
                    endPositionCode = 89;//  跳过特殊的55区最后5个字符   string.IsNullOrWhiteSpace(s)无法判断这5个转换后的字符，虽然为空白
                }
                for (int positionCode = 1; positionCode <= endPositionCode; positionCode++)
                {
                    strBuild.Append(gb.GetString(new byte[] { Convert.ToByte(regionCode + 160), Convert.ToByte(positionCode + 160) }));
                }
                strBuild.AppendLine();
                strBuild.AppendLine();
            }

            return strBuild.ToString();
        }


        /// <summary>
        /// 处理随机字符长度的最大值和最小值
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        private static void DealMinMaxLength(ref int minLength, ref int maxLength)
        {
            if (minLength <= 0)
            {
                minLength = 1;
            }
            if (minLength > maxLength)
            {
                maxLength = minLength;
            }
        }
    }
}
