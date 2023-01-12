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

            false.ToString().ToLowerInvariant();

            Console.WriteLine("Hello, World!---"+a);

            EnumType.TestMethod();

            StructureType structureType = new StructureType()
            {
                Id = 10
            };
            //structureType.Id = 10;
            Console.WriteLine(structureType.Id);

            StructureType1 structureType1 = new StructureType1();

            Console.WriteLine(structureType1.Id);


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