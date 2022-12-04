namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = 1;
            a++;
            {
                var b = a + 2;
            }


            Console.WriteLine("Hello, World!");
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
    }
}