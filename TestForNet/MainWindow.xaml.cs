using System;
using System.Collections.Generic;
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

namespace TestForNet
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
            var person = new Person(null!);

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
