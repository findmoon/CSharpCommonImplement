using Microsoft.Toolkit.Uwp.Notifications;
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

namespace NotificationLocalToastWPFNet6
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
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation") // 添加相关参数
                .AddArgument("conversationId", 9813)
                .AddText("CodeMissing发来一张图片") // 标题文本
                .AddText("这是C#的图片")
                .AddInlineImage(new Uri("https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png"))  //    uwp/msix才能使用http
                //.AddHeroImage(new Uri("https://desk-fd.zol-img.com.cn/t_s960x600c5/g2/M00/00/05/ChMlWV6w8G6Icf63AALuQ6IKbecAAO2kgP0OegAAu5b135.jpg"))
                // 内联图片
                //.AddInlineImage(new Uri("https://desk-fd.zol-img.com.cn/t_s960x600c5/g2/M00/00/05/ChMlWV6w8G6Icf63AALuQ6IKbecAAO2kgP0OegAAu5b135.jpg")) // 网络图片必须小于200 KB
                //.AddInlineImage(new Uri("pack://application:,,,/NotificationLocalToast;component/Resources/CSharp.png", UriKind.RelativeOrAbsolute))
                //.AddInlineImage(new Uri("pack://application:,,,/Resources/CSharp.png")) // 无法正确加载到图片

                // Profile (app logo override) image
                //.AddAppLogoOverride(new Uri("https://www.vippng.com/png/detail/398-3984434_c-programming-png.png"), ToastGenericAppLogoCrop.Circle)
                .Show();
        }
    }
}
