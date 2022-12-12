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

namespace PGDBManagement
{
    public partial class LongPathTest : Form
    {
        public LongPathTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // .NET 4.6.2以下需要`App.config`添加如下设置：
            //<runtime>
            //    <AppContextSwitchOverrides value="Switch.System.IO.UseLegacyPathHandling=false;Switch.System.IO.BlockLongPaths=false" />
            //</runtime>

            string reallyLongDirectory = @"C:\Test\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Console.WriteLine($"Creating a directory that is {reallyLongDirectory.Length} characters long");

            var isLongPath = false;
            if (reallyLongDirectory.Length>=248) // 注意文件完全路径限制小于260
            {
                isLongPath = true;
            }

            Directory.CreateDirectory((isLongPath? @"\\?\":"") + reallyLongDirectory);

            #region 捕获长路径异常，悖论
            //try
            //{
            //    string reallyLongDirectory = @"C:\Test\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //    reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //    reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //    Console.WriteLine($"Creating a directory that is {reallyLongDirectory.Length} characters long");
            //    Directory.CreateDirectory(@"\\?\" + reallyLongDirectory);
            //}
            //catch (PathTooLongException ex)
            //{
            //    // 这个长路径异常捕获不合理，因为只有.NET 4.6.2以下才会报错PathTooLongException，而报错后仅仅提示 \\?\ 形式并使用不能解决问题，还需要App.config配置
            //    // 但是.NET 4.6.2及以上则不会报此错误，而是NotFound，所以不会捕获，就无法提醒使用 \\?\ 形式
            //    throw new Exception(@"发生长路径异常，请使用 \\?\ + 长路径 形式" + ex.Message);
            //}
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //using (var s=new StreamReader())
            //{

            //}
        }
    }
}
