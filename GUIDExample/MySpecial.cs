using System;
using System.Runtime.InteropServices;

namespace GUIDExample
{
    [Guid("b9af5139-9d00-4e9a-a8d8-44c099dd6f93")]
    class MySpecial
    {
        public static void Hi()
        {
            Console.WriteLine("我是特别的");
        }
    }
}
