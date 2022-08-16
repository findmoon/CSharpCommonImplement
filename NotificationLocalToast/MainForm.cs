using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Foundation.Collections;


namespace NotificationLocalToast
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // 监听通知激活(点击)
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                // 通知参数
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                // 获取任何用户输入
                ValueSet userInput = toastArgs.UserInput;

                BeginInvoke(new Action( delegate
                {
                    // TODO: UI线程的操作
                    MessageBox.Show("Toast被激活（点击），参数是: " + toastArgs.Argument);
                }));
            };

            FormClosing += MainForm_FormClosing;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ToastNotificationManagerCompat.Uninstall();
            //ToastNotificationManagerCompat.History.Remove("tag");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ToastContentBuilder()
                .AddText("CodeMissing发来一条消息") // 标题文本
                .AddText("请检查消息内容，并及时处理")
                .Show(); // 7.0以上才提供Show方法

            //new ToastContentBuilder()
            //    //.AddArgument("action", "viewConversation")
            //    //.AddArgument("conversationId", 9813)
            //    .AddText("CodeMissing发来一条消息")
            //    .AddText("请检查消息内容，并及时处理")
            //    .Show(); // 7.0以上才提供Show方法

            //        new ToastContentBuilder()
            //.AddArgument("conversationId", 9813)

            //.AddText("Some text")

            //.AddButton(new ToastButton()
            //    .SetContent("Archive")
            //    .AddArgument("action", "archive")
            //    .SetBackgroundActivation())

            //.AddAudio(new Uri("ms-appx:///Sound.mp3"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = System.IO.Packaging.PackUriHelper.UriSchemePack;//uri访问本地资源，Winform中，防止报错 无效的端口

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            var imgFileFullPath = Path.GetFullPath("Resources/CSharp.png");
            var fileUriString = $"file:///{imgFileFullPath}";
            var imgUri = new Uri(fileUriString);
            //StreamResourceInfo info = Application.GetContentStream(uri);
            //BitmapImage img = new BitmapImage(uri); // Windows.UI.Xaml.Media.Imaging 无法使用，跨线程
            //pictureBox1.Image 
            //pictureBox1.Load
            //pictureBox1.ImageLocation = "pack://application:,,,/Resources/CSharp.png";
            pictureBox1.ImageLocation = fileUriString;
            //pictureBox1.Image = Properties.Resources.CSharp;

            new ToastContentBuilder()
                .AddArgument("action", "viewConversation") // 添加相关参数
                .AddArgument("conversationId", 9813)
                .AddText("CodeMissing发来一张图片") // 标题文本
                .AddText("这是C#的图片")
                // 内联图片
                //.AddInlineImage(new Uri("https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png")) // 网络图片必须小于200 KB    uwp/msix才能使用http
                //.AddInlineImage(new Uri("pack://application:,,,/NotificationLocalToast;component/Resources/CSharp.png", UriKind.RelativeOrAbsolute))
                //.AddInlineImage(new Uri("pack://application:,,,/Resources/CSharp.png"))
                //.AddInlineImage(imgUri)
                //.AddHeroImage(imgUri)
                // Profile (app logo override) image
                //.AddAppLogoOverride(new Uri("https://www.vippng.com/png/detail/398-3984434_c-programming-png.png"), ToastGenericAppLogoCrop.Circle)
                .AddAppLogoOverride(imgUri, ToastGenericAppLogoCrop.Circle)
                .Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new ToastContentBuilder()
            .AddText("我是含有Tag和Group的消息")
            .Show(toast =>
            {
                toast.Tag = "codemissing101";
                toast.Group = "codemissing";
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new ToastContentBuilder()
            .AddText("我是替换的消息")
            .Show(toast =>
            {
                toast.Tag = "codemissing101";
                toast.Group = "codemissing";
            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //ToastNotificationManagerCompat.History.Remove("codemissing101");
            ToastNotificationManagerCompat.History.Remove("codemissing101", "codemissing");
            //ToastNotificationManagerCompat.History.RemoveGroup("codemissing");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string s = System.IO.Packaging.PackUriHelper.UriSchemePack;//uri访问本地资源，Winform中，防止报错 无效的端口

            var imgPackUriStr = "pack://application:,,,/Resources/CSharp.png";
            pictureBox1.ImageLocation = imgPackUriStr;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var imgFileFullPath = Path.GetFullPath("Resources/CSharp.png");
           
            new ToastContentBuilder()
                .AddText("这是C#的图片")
                .AddAppLogoOverride(new Uri(imgFileFullPath), ToastGenericAppLogoCrop.Circle)
                .Show();
        }
    }
}
