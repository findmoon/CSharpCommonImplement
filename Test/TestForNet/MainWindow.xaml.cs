using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiscellaneousTestForNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<int>? nullInts=null;
            Debug.WriteLine($"nullInts的元素数量为：{nullInts?.Count ?? 0}");
            nullInts = new List<int> { 1 };
            Debug.WriteLine($"nullInts的元素数量为：{nullInts?.Count ?? 0}");


            string notNull = "Hello";
            string? nullable = default;
            notNull = nullable!; // null forgiveness


            int? a = 16;
            if (a is int)
            {
                Debug.WriteLine("int? instance is compatible with int");
            }
            if (a is int?)
            {
                Debug.WriteLine("int? instance is compatible with int?");
            }

            int b = 15;
            if (b is int?)
            {
                Debug.WriteLine("int instance is compatible with int?");
            }
            if (b is int)
            {
                Debug.WriteLine("int instance is compatible with int");
            }


            //int? a = 17;
            //Type typeOfA = a.GetType();
            //Debug.WriteLine(typeOfA.FullName); //   // System.Int32

            //int b = 16;
            //Type typeOfB = b.GetType();
            //Debug.WriteLine(typeOfB.FullName); //   // System.Int32


            //Debug.WriteLine(Nullable.GetUnderlyingType(typeof(int)));       // [无基础类型]
            //Debug.WriteLine(Nullable.GetUnderlyingType(typeof(int?)));  // System.Int32
            //Debug.WriteLine(Nullable.GetUnderlyingType(typeof(DateTime)));  // [无]
            //Debug.WriteLine(Nullable.GetUnderlyingType(typeof(DateTime?))); // System.DateTime
            //Debug.WriteLine(Nullable.GetUnderlyingType(typeof(MyTest)));    // [无]
            //// Debug.WriteLine(Nullable.GetUnderlyingType(typeof(MyTest?)));
            Debug.WriteLine(1);


            NullNameShouldThrowTest();
        }
        //int MyTest1()
        //{
        //    int? a = null;
        //    int b = a!;

        //    return a!;
        //}

        Test MyTest2()
        {
            Test? a = new Test();
            //Test? a = null ;

            var b = a ;

            var c = a!;

            return a!;
        }

    
        public void NullNameShouldThrowTest()
        {
            //var person = new Person(null!);

            Person? p = new Person("John");
            if (IsValid(p))
            {
                Console.WriteLine($"Found {p!.Name}");
            }
        }

        public static bool IsValid(Person? person)
        => person is not null && person.Name is not null;

        public static bool IsValid2([NotNullWhen(true)] Person? person)
        => person is not null && person.Name is not null;
    }
    class MyTest { }

#nullable enable
    public class Person
    {
        public Person(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

        public string Name { get; }
    }


    class Test
    {

    }
}
