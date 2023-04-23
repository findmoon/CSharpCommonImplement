
    internal class Program
    {
    public static void Main(string[] args)
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

            Console.ReadLine();
        }
    }
