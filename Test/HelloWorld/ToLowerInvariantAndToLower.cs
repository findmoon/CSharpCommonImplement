using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    internal class ToLowerInvariantAndToLower
    {
        public static void Test()
        {

            string invariant = "iii".ToUpperInvariant();
            string invariant_toLower = "III".ToLowerInvariant();
            
            CultureInfo turkey = new CultureInfo("tr-TR");
            //Thread.CurrentThread.CurrentCulture = turkey;
            string cultured = "iii".ToUpper(turkey);
            string cultured_toLower = "III".ToLower(turkey);

            string invariant1 = "iii".ToUpper(CultureInfo.InvariantCulture);
            string invariant1_toLower = "III".ToLower(CultureInfo.InvariantCulture);

            Console.WriteLine($"invariant：{invariant}");    // invariant：III
            Console.WriteLine($"cultured：{cultured}");      // cultured：???
            Console.WriteLine($"invariant1：{invariant1}" ); // invariant1：III
            Console.WriteLine($"invariant_toLower：{invariant_toLower}");    // invariant_toLower：iii
            Console.WriteLine($"cultured_toLower：{cultured_toLower}");      // cultured_toLower：???
            Console.WriteLine($"invariant1_toLower：{invariant1_toLower}" ); // invariant1_toLower：iii
        }

        public static void StringCompare()
        {
            var str1=string.Empty; 
            var str2="A";
            if (str1==str2)
            {
                
            }

            if (string.Equals(str1,str2))
            {

            }
            if (string.Equals(str1,str2, StringComparison.Ordinal))
            {

            }
            if (string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase))
            {

            }


            string str3 = null; 
            if (str1 == str3)
            {

            }

            if (string.Equals(str1, str3))
            {

            }
            if (string.Equals(str1, str3, StringComparison.Ordinal))
            {

            }
            if (string.Equals(str1, str3, StringComparison.OrdinalIgnoreCase))
            {

            }

            if (str1.ToLowerInvariant()=="a")
            {

            }
        }
    }
}
