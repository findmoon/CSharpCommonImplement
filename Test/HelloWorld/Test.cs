using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    internal class Test
    {
        public static void TestMethod()
        {
            Console.WriteLine((int)Season.Spring);
            Console.WriteLine((int)Season.Summer);
            Console.WriteLine((int)Season.Autumn);
            Console.WriteLine((int)Season.Winter);

            // 调用枚举的扩展方法
            Season.Autumn.EnumExtMethod();


            Console.WriteLine($"枚举默认值：{default(Season)}");
            Console.WriteLine($"枚举默认值：{(int)default(Season)}");
        }
    }
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public static class ExtenMethod
    {
        public static void EnumExtMethod(this Season season)
        {
            var t=typeof(Season);
            Console.WriteLine($"我是 enum 枚举类型 {season.GetType().Name} 的扩展方法。我的值是{season}：{(int)season}");
        }
    }
}
