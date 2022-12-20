using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HelloWorld
{
    internal class InitAccessor
    {
        public static void TestMethod()
        {
            var person = new Person_InitExampleAutoProperty()
            {
                YearOfBirth = 1970
            };

            // person.YearOfBirth = 1970; // 错误 CS8852  只能在对象初始值设定项中或在实例构造函数或 "init" 访问器中的 "this" 或 "base" 上分配 init-only 属性或索引器 

        }
    }

    class Person_InitExampleAutoProperty
    {
        public int YearOfBirth { get; init; }
    }
}
