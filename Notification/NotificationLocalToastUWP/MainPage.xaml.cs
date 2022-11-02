using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NotificationLocalToastUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation") // 添加相关参数
                .AddArgument("conversationId", 9813)
                .AddText("CodeMissing发来一张图片") // 标题文本
                .AddText("这是C#的图片")
                .AddInlineImage(new Uri("https://www.vippng.com/png/detail/398-3984434_c-programming-png.png"))
                //.AddInlineImage(new Uri("https://picsum.photos/360/202?image=883"))
                //.AddInlineImage(new Uri("https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fdepot.nipic.com%2Ffile%2F20191107%2F22525882_22243683098.jpg&refer=http%3A%2F%2Fdepot.nipic.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=auto?sec=1662204960&t=214b63c0ddbae4ddc888e8e6e0a19ddb"))
                //.AddInlineImage(new Uri("https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png"))
                //.AddHeroImage(new Uri("https://desk-fd.zol-img.com.cn/t_s960x600c5/g2/M00/00/05/ChMlWV6w8G6Icf63AALuQ6IKbecAAO2kgP0OegAAu5b135.jpg"))
                // 内联图片
                //.AddInlineImage(new Uri("https://desk-fd.zol-img.com.cn/t_s960x600c5/g2/M00/00/05/ChMlWV6w8G6Icf63AALuQ6IKbecAAO2kgP0OegAAu5b135.jpg")) // 网络图片必须小于200 KB
                //.AddInlineImage(new Uri("pack://application:,,,/NotificationLocalToast;component/Resources/CSharp.png", UriKind.RelativeOrAbsolute))
                //.AddInlineImage(new Uri("pack://application:,,,/Resources/CSharp.png")) // 无法正确加载到图片

                // Profile (app logo override) image
                .AddAppLogoOverride(new Uri("https://www.vippng.com/png/detail/398-3984434_c-programming-png.png"), ToastGenericAppLogoCrop.Circle)
                .Show();
        }
    }
}
