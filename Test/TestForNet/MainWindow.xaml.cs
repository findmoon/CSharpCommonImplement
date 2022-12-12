using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            JsonNode jnode = JsonNode.Parse(@"{""a"":""你好"",""b"":10,""C"":[1,2,3]}")!;     //将string 解析为JsonNode

            JsonObject jobject = jnode.AsObject()!;     //JsonNode 转化为JsonObject对象

            
            JsonArray jarray = jobject["C"]!.AsArray();     //解析并且转化成JsonArray


            //获取节点
            JsonNode jnode1 = jarray[1]!;
            Debug.WriteLine($"Parent Node：{jnode1.Parent}");
            Debug.WriteLine($"Root Node：{jnode1.Root}");
            Debug.WriteLine($"JsonNodeOptions：{jnode1.Options}");

            //取值
            Debug.WriteLine($"ToJsonString():{jarray.ToJsonString()}");
            Debug.WriteLine($"GetValue<int>():{jnode1.GetValue<int>()}");
            Debug.WriteLine($"强制类型转换:{(int?)jarray[2]}");
            Debug.WriteLine($"反序列化获取Int[]:{JsonSerializer.Deserialize<int[]>(jarray)}{Environment.NewLine}  反序列化后取值：{Environment.NewLine}{JsonSerializer.Deserialize<int[]>(jarray)?.Aggregate("", (aggStr, v) => $"{aggStr}    值：{v}{Environment.NewLine}")}");

            Debug.WriteLine($"ToJsonString()序列化：{jnode1.Root.ToJsonString(new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            unsafe
            {
                char* pointerToChars = stackalloc char[123];

                for (int i = 65; i < 123; i++)
                {
                    pointerToChars[i] = (char)i;
                }

                Debug.Write("Uppercase letters: ");
                for (int i = 65; i < 91; i++)
                {
                    Debug.Write(pointerToChars[i]);
                }
                Debug.WriteLine("");
                Debug.Write("Lowercase letters: ");
                for (int i = 97; i < 123; i++)
                {
                    Debug.Write(pointerToChars[i]);
                }
            }
            // Output:
            // Uppercase letters: ABCDEFGHIJKLMNOPQRSTUVWXYZ
            // Lowercase letters: abcdefghijklmnopqrstuvwxyz
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            int number = 1024;

            unsafe
            {
                // Convert to byte:
                byte* p = (byte*)&number;

                Debug.Write("整型的四个字节:");

                // Display the 4 bytes of the int variable:
                for (int i = 0; i < sizeof(int); ++i)
                {
                    Debug.Write((*p).ToString("X2")+" ");
                    // Increment the pointer:
                    p++;
                }
                Debug.WriteLine("");
                Debug.WriteLine("整型的值: {0}", number);

                /* Output:
                    整型的四个字节: 00 04 00 00
                    整型的值: 1024
                */
            }
        }

        private void Button_Click5(object sender, RoutedEventArgs e)
        {
            // 获取 当前用户的临时文件夹
            string tempDir = System.IO.Path.GetTempPath();
            Debug.WriteLine("临时文件夹：" + tempDir);

            // 创建并获取 一个 当前用户的临时文件名: %LocalAppData%\Temp\tmpxxxxx.tmp
            string tempFile =System.IO.Path.GetTempFileName();
            Debug.WriteLine("Using： " + tempFile);
            File.Delete(tempFile);


            // 获取 TEMP 变量，通常为用户的临时文件夹，但 有时会获取到的路径为 C:\\Users\\WIN7HO~1\\AppData\\Local\\Temp 即包含"~1"，无法正确访问
            string? tempVar = Environment.GetEnvironmentVariable("TEMP");
            Debug.WriteLine("系统临时文件夹： " + tempVar);

            Debug.WriteLine("随机文件名： " + System.IO.Path.GetRandomFileName()); 
        }
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
