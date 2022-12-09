using GCCollectIntExample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace GCHandleUse
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
            Console.Out.WriteLine("11");

            TextWriter tw = Console.Out;
            GCHandle gch = GCHandle.Alloc(tw);
            //gch.AddrOfPinnedObject();

            //GCHandle.ToIntPtr();
            var a= GCHandle.FromIntPtr(gch.AddrOfPinnedObject()).Target;
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            MyGCCollectClass.TestMethod();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var test = new OnlyForTest();
            // 固定内存地址，获取托管对象的GC句柄
            GCHandle gch = GCHandle.Alloc(test);

            // 从GC句柄获取内存地址 IntPtr 指针
            var test_intPtr = GCHandle.ToIntPtr(gch);


            #region AddrOfPinnedObject() 需要获取的是pin住对象地址，非pinned对象获取时报错
            // GCHandleType.Pinned pin住对象时报错 System.ArgumentException:“Object contains non-primitive or non-blittable data. Arg_ParamName_Name”
            // 只能pin基元类型或可复制(传输)类型。对象引用为 non-blittable 类型
            //var test_forPinned = new OnlyForTest();
            //GCHandle gch_pined = GCHandle.Alloc(test_forPinned, GCHandleType.Pinned);
            //// 获取pin住的GC句柄的内存地址 IntPtr 指针
            //var test_intPtr2 = gch_pined.AddrOfPinnedObject(); 
            #endregion

            //Debug.WriteLine($"从GC句柄获取内存地址：{test_intPtr2}");

            Debug.WriteLine($"从GC句柄获取内存地址：{test_intPtr}");

            // 从 IntPtr 指针 获取GC句柄
            var gch2 = GCHandle.FromIntPtr(test_intPtr);

            // 从GC句柄获取对应的 托管对象
            var test2 = (OnlyForTest)gch2.Target!;

            Debug.WriteLine($"托管对象和GC句柄相互转换后是否相等：{Object.ReferenceEquals(test, test2)}");

            Debug.WriteLine($"通过intPtr转换后GC句柄是否相等：{Object.ReferenceEquals(test, test2)}");

            gch.Free();
            gch2.Free();
        }

        class OnlyForTest
        {

        }
    }
}
