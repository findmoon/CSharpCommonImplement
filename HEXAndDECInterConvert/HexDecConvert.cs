using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    static class HexDecConvert
    {
        /// <summary>
        /// Hex十六进制数字转十进制
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int HexToDecimal(string hex)
        {
            if (!Regex.Match(hex, "^[0-9A-F]+$", RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("不是十六进制数字");
            }

            var decimalValue = 0;

            var hexUp = hex.ToUpper();
            // 从最后一位到第一位循环获取每位的值，并乘以基数的n-1次方
            for (int i = hexUp.Length - 1; i >= 0; i--)
            {
                int currV = 0;
                switch (hexUp[i])
                {
                    case 'A':
                        currV = 10;
                        break;
                    case 'B':
                        currV = 11;
                        break;
                    case 'C':
                        currV = 12;
                        break;
                    case 'D':
                        currV = 13;
                        break;
                    case 'E':
                        currV = 14;
                        break;
                    case 'F':
                        currV = 15;
                        break;
                    case '0':
                        currV = 0;
                        break;
                    case '1':
                        currV = 1;
                        break;
                    case '2':
                        currV = 2;
                        break;
                    case '3':
                        currV = 3;
                        break;
                    case '4':
                        currV = 4;
                        break;
                    case '5':
                        currV = 5;
                        break;
                    case '6':
                        currV = 6;
                        break;
                    case '7':
                        currV = 7;
                        break;
                    case '8':
                        currV = 8;
                        break;
                    case '9':
                        currV = 9;
                        break;
                    default:
                        break;
                }

                for (int n = 0; n < hexUp.Length - 1 - i; n++)
                {
                    currV *= 16;
                }
                decimalValue += currV;
            }
            return decimalValue;
        }

        /// <summary>
        /// 十进制数字转十六进制
        /// </summary>
        /// <param name="dec">十进制数字</param>
        /// <param name="lower">16进制结果是否为小写，默认false</param>
        /// <returns></returns>
        public static string DecimalToHex(int dec, bool lower = false)
        {
            if (dec==0)
            {
                return "0";
            }
            var hexBuilder = new StringBuilder();
            while (dec != 0)
            {
                var currV = dec % 16;

                char currHex;
                switch (currV)
                {
                    case 0:
                        currHex = '0';
                        break;
                    case 1:
                        currHex = '1';
                        break;
                    case 2:
                        currHex = '2';
                        break;
                    case 3:
                        currHex = '3';
                        break;
                    case 4:
                        currHex = '4';
                        break;
                    case 5:
                        currHex = '5';
                        break;
                    case 6:
                        currHex = '6';
                        break;
                    case 7:
                        currHex = '7';
                        break;
                    case 8:
                        currHex = '8';
                        break;
                    case 9:
                        currHex = '9';
                        break;
                    case 10:
                        currHex = 'A';
                        break;
                    case 11:
                        currHex = 'B';
                        break;
                    case 12:
                        currHex = 'C';
                        break;
                    case 13:
                        currHex = 'D';
                        break;
                    case 14:
                        currHex = 'E';
                        break;
                    case 15:
                        currHex = 'F';
                        break;
                    default:
                        currHex = '-';
                        break;
                }
                // 从个位即最右边开始往前获取16进制值
                hexBuilder.Insert(0, currHex);

                dec /= 16;
            }

            return lower ? hexBuilder.ToString().ToLower() : hexBuilder.ToString();
        }

        /// <summary>
        /// 另一种16进制转10进制的处理方式，Multiplier参与*16的循环很巧妙，对Multiplier的处理很推荐，逻辑统一
        /// </summary>
        /// <param name="HexaDecimalString"></param>
        /// <returns></returns>
        public static int HexaToDecimal(string HexaDecimalString)
        {
            int Decimal = 0;
            int Multiplier = 1;

            for (int i = HexaDecimalString.Length - 1; i >= 0; i--)
            {
                Decimal += HexaToDecimal(HexaDecimalString[i]) * Multiplier;
                Multiplier *= 16;
            }
            return Decimal;
        }

        static int HexaToDecimal(char c)
        {
            switch (c)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'A':
                case 'a':
                    return 10;
                case 'B':
                case 'b':
                    return 11;
                case 'C':
                case 'c':
                    return 12;
                case 'D':
                case 'd':
                    return 13;
                case 'E':
                case 'e':
                    return 14;
                case 'F':
                case 'f':
                    return 15;
            }
            return -1;
        }


    }
}
