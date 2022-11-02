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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace NotificationLocalToastWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            //Image

            var uri = new Uri("pack://application:,,,/Resources/CSharp.png");
            var img = new BitmapImage(uri); // 空
            //System.Drawing.Image myImage0 = System.Drawing.Image.FromStream(img.StreamSource); // null异常


            //StreamResourceInfo info = Application.GetContentStream(uri); // 得到的info为null
            StreamResourceInfo info = Application.GetResourceStream(uri);
            using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(info.Stream))
            {
                myImage.Save("my.Image.png", System.Drawing.Imaging.ImageFormat.Png);
            }

            // dragMoveBtn.MouseMove 按钮拖动调用 DragMove() 无效

            // 直接拖动窗体
            dragMoveImg.MouseMove += (sender, e) =>
            {
                if (e.LeftButton== MouseButtonState.Pressed)
                {
                    DragMove();
                }
            };
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ToastNotificationManagerCompat.Uninstall();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var path = System.IO.Path.GetFullPath("CSharpNew.png");
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation") // 添加相关参数
                .AddArgument("conversationId", 9813)
                .AddText("CodeMissing发来一张图片") // 标题文本
                .AddText("这是C#的图片")
                //.AddInlineImage(new Uri("https://picsum.photos/360/202?image=883"))  //  uwp/msix才能使用http
                // .AddInlineImage(new Uri("https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fdepot.nipic.com%2Ffile%2F20191107%2F22525882_22243683098.jpg&refer=http%3A%2F%2Fdepot.nipic.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=auto?sec=1662204960&t=214b63c0ddbae4ddc888e8e6e0a19ddb"))
                //.AddInlineImage(new Uri("https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png"))
                //.AddHeroImage(new Uri("https://desk-fd.zol-img.com.cn/t_s960x600c5/g2/M00/00/05/ChMlWV6w8G6Icf63AALuQ6IKbecAAO2kgP0OegAAu5b135.jpg"))
                // 内联图片
                //.AddInlineImage(new Uri("https://desk-fd.zol-img.com.cn/t_s960x600c5/g2/M00/00/05/ChMlWV6w8G6Icf63AALuQ6IKbecAAO2kgP0OegAAu5b135.jpg")) // 网络图片必须小于200 KB
                //.AddInlineImage(new Uri("pack://application:,,,/NotificationLocalToast;component/Resources/CSharp.png", UriKind.RelativeOrAbsolute))
                //.AddInlineImage(new Uri("pack://application:,,,/Resources/CSharp.png")) // 无法正确加载到图片
                //.AddInlineImage(new Uri($"pack://siteoforigin:,,,/CSharpNew.png")) // 无法正确加载到图片 

                // Profile (app logo override) image
                //.AddAppLogoOverride(new Uri("https://www.vippng.com/png/detail/398-3984434_c-programming-png.png"), ToastGenericAppLogoCrop.Circle)
                .Show();
        }
    }
}
