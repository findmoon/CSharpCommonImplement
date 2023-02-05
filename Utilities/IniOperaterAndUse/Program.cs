using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IniOperaterAndUse
{
    class Program
    {
        static void Main(string[] args)
        {
            var iniFile = new IniOperater("my.ini");

            // ini中如果出现多个=，将有可能读取错乱
            iniFile.Write("a=c", "http://127.0.0.1:8090?id=10");
            var r=iniFile.Read("a=c");
            var r_a=iniFile.Read("a");
            Console.WriteLine(r);
            Console.WriteLine(r_a);

            var r_a_iniTest = iniFile.Read("a", "IniTest");
            Console.WriteLine(r_a_iniTest);

            staticTest.GetstaticTest();

            // 获取所有section
            var sections = iniFile.GetAllSections();
            Console.WriteLine(string.Join(",", sections));

            var sections_no = iniFile.GetAllSectionsWithOutDefault();

            // 新指定文件，自动生成
            var iniFile2 = new IniOperater("my2.ini");
            iniFile2.Write("s","m");
            Console.WriteLine(iniFile2.Read("s"));

            var r2 = iniFile2.Read("s2",defaultValue: "m2");
            var r3 = iniFile2.Read("s3", defaultValue: "m3");
            Console.WriteLine(r2);
            Console.WriteLine(r3);

            Console.ReadLine();

            //// 没有section 无法 设置读取
            //iniFile2.WriteWithoutSection("s1", "m1");
            //Console.WriteLine(iniFile2.ReadWithoutSection("s1"));
        }
    }



    static class staticTest
    {
        static staticTest()
        {
            Console.WriteLine("123静态");
        }
        public static void GetstaticTest()
        {

        }
    }
}
