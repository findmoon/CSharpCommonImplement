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

namespace StartWithPCWpf
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
            startWithPCAndAutoRun_Click(sender);
        }
        #region 按钮状态变更的操作
        /// <summary>
        /// 设置 开启自启动且自动运行 按钮 的状态(文字、颜色)
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="startText"> 或 "设置开机启动且自动运行"</param>
        /// <param name="stopText"> 或 "关闭开机启动且自动运行"</param>
        /// <returns></returns>
        private bool SetStartupAutoRunButton(ContentControl btn, string startText = "设置开机启动", string stopText = "关闭开机启动")
        {
            if (StartWithPC.IsRunWithPC)
            {
                btn.Content = stopText;
                btn.Background = SystemColors.ActiveBorderBrush;
                return true;
            }
            else
            {
                btn.Content = startText;
                btn.Background = new SolidColorBrush(Colors.PaleTurquoise);
                return false;
            }
        }
        private void startWithPCAndAutoRun_Click(object sender)
        {
            if (sender is ContentControl btn)
            {
                StartWithPC.SetMeAutoStart(btn.Content.ToString().StartsWith("设置"));
                SetStartupAutoRunButton(btn, "设置为开机启动（非管理员）", "取消开机启动（非管理员）");
            }
        }
        #endregion
    }
}
