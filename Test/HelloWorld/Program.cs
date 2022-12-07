namespace HelloWorld
{
    internal class Program12567
    {
        static void Main(string[] args)
        {
            int a = 1;
            a++;
            {
                var b = a + 2;
            }

            var aa = false;
            var bb = true;

            Console.WriteLine("aa---"+ aa);
            Console.WriteLine("bb---"+ bb);
            Console.WriteLine("aa---"+ aa.ToString());
            Console.WriteLine("bb---"+ bb.ToString());

            var trueStr=Boolean.TrueString;
            
            var trueStr2=bool.TrueString;

            Console.WriteLine("Hello, World!---"+a);

            Test.TestMethod();
            Console.ReadLine();
        }
    }

    public class Location
    {
        private string locationName;

        public Location(string name) => Name = name;

        public string Name
        {
            get => locationName;
            set => locationName = value;
        }

        public string GetName() => $"name:{locationName.Trim()}";

        /// <summary>
        /// 求和方法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>返回两个参数的和</returns>
        public int Add(int a,int b)
        {
            // 返回两个数相加的和
            return a + b;
            /*
             * 我是多行注释
             * 可以有多行
             作为说明解释
             */
        }
    }
}