using System;
using System.Collections.Generic;
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

namespace ClickOnceWPFFx
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // System.Windows.Media.Color
                var color_Colors = Colors.Red;

                // 报错 颜色上下文为空
                //var color1_Colors = color_Colors.GetNativeColorValues();
                
                var color2_Colors = System.Windows.Media.ColorConverter.ConvertFromString("Red");
                // color_Colors.ToString() - "Colors.Red" 无法转换
                var color3_Colors = System.Windows.Media.ColorConverter.ConvertFromString(color_Colors.ToString());
                var color4_Colors = System.Windows.Media.ColorConverter.ConvertFromString("Colors.Yellow".Replace("Colors.",""));
                var color5_Colors = System.Windows.Media.ColorConverter.ConvertFromString("Yellow".Replace("Colors.",""));

                var color6_Colors = System.Windows.Media.ColorConverter.ConvertFromString(SystemColors.ControlLightColor.ToString());
                // 无法识别转换 "SystemColors.ControlLightColor"、"ControlLightColor" 字符串
                //var color7_Colors = System.Windows.Media.ColorConverter.ConvertFromString("SystemColors.ControlLightColor".Replace("SystemColors.", ""));

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
