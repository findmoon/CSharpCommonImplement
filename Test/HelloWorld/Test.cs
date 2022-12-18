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

            Console.WriteLine($"CompareTo：{Season.Spring.CompareTo(Season.Winter)}");
            Console.WriteLine($"CompareTo：{Season.Autumn.CompareTo(Season.Summer)}");
            Console.WriteLine($"CompareTo：{Season.Autumn.CompareTo((Season)2)}");
            Console.WriteLine($"Equals：{Season.Spring.Equals(0)}");
            Console.WriteLine($"Equals：{Season.Spring.Equals(Season.Spring)}");

            foreach (var name in Enum.GetNames(typeof(Season)))
            {
                Console.WriteLine(name);
            }
            Console.WriteLine("---------");

            Console.WriteLine(Enum.GetName(typeof(Season),1)); // Summer

            Console.WriteLine(Enum.GetName(typeof(Season), Season.Summer)); // Summer

            Console.WriteLine(Season.Spring.ToString()); // Spring

            var season=Enum.Parse(typeof(Season),"Autumn",ignoreCase:true);
            var result = Enum.TryParse(typeof(Season), "Autumn", ignoreCase: true, out object? season1);
            var result2 = Enum.TryParse("Autumn", ignoreCase: true, out Season season2); // 推荐


            Console.WriteLine((int)Season.Summer);
            Console.WriteLine((byte)Season.Summer);

            var season3 = Enum.ToObject(typeof(Season),3);
            Console.WriteLine(season3);

        

            Console.WriteLine("---------");

            Console.WriteLine(Enum.IsDefined(typeof(Season), 1));
            Console.WriteLine(Enum.IsDefined(typeof(Season), "Autumn"));

            var k = 1 << 0;
            var k2 = 1 << 1;
            var k3 = 1 << 2;
            var k4 = 1 << 3;
            Console.WriteLine(k);

            //Season.Winter.HasFlag(Season.Winter);

            var permission = Permission.Create | Permission.Read | Permission.Update | Permission.Delete;
            permission = permission & ~Permission.Update;
            permission = permission & ~Permission.Delete;

            permission = permission | Permission.Update;

            permission = Permission.Unknown | Permission.Create | Permission.Update;

            Console.WriteLine(permission.ToString());
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

    //  [Flags]
    //  public enum Permission
    //{　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　
    //　　  Unknown,　　　// 0　　　　　　　　
    // 　　 Create,　　　 // 1　　　　　　
    //  　　Read,        // 2
    // 　　 Update,      // Permission.Create | Permission.Read -- 3
    //      Delete,      // 4
    //}

    [Flags]
    public enum Permission
    {
        Unknown = 0, // 也可以写成0x00或0　　　　　　　　　　　
        Create = 1 << 0, // 0x01或1　　　　　　　　　
        Read = 1 << 1,  //0x02或2
        Update = 1 << 2, //0x04或4
        Delete = 1 << 3,  //0x08或8
        ReadWrite= Permission.Read| Permission.Update
    }

}
