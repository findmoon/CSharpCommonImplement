using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmbedFont
{
    public partial class Form1 : Form
    {
        PrivateFontCollection pfc = new PrivateFontCollection(); // using System.Drawing.Text;

        /// <summary>
        /// 添加第三方字体
        /// </summary>
        void AddPrivateFont()
        {
            #region font file
            //// 字体路径
            //string[] fontNames = { "沐瑶软笔手写体.ttf", "SourceHanSansCN-Regular.otf", "SourceHanSansSC-Heavy.otf" };
            //for (int i = 0; i < fontNames.Length; i++)
            //{
            //    pfc.AddFontFile(fontNames[i]);
            //} 
            #endregion

            #region Resources Font
            //var fontData1 = Properties.Resources.沐瑶软笔手写体;
            //var fontData2 = Properties.Resources.SourceHanSansCN_Regular;
            //var fontData3 = Properties.Resources.SourceHanSansSC_Heavy;
            //unsafe  // 属性设置，生成中 允许不安全代码
            //{
            //    // 将字体添加到PrivateFontCollection
            //    fixed (byte* pFontData = fontData1)
            //    {
            //        pfc.AddMemoryFont((System.IntPtr)pFontData, fontData1.Length);
            //    }
            //    fixed (byte* pFontData = fontData2)
            //    {
            //        pfc.AddMemoryFont((System.IntPtr)pFontData, fontData2.Length);
            //    }
            //    fixed (byte* pFontData = fontData3)
            //    {
            //        pfc.AddMemoryFont((System.IntPtr)pFontData, fontData3.Length);
            //    }
            //} 
            #endregion

            var fontName = "沐瑶软笔手写体.ttf";
            // 加载字体
            Assembly assembly = Assembly.GetExecutingAssembly();
            //var names = assembly.GetManifestResourceNames();
            string projectName = assembly.GetName().Name;
            // 加载程序资源
            using (Stream stream = assembly.GetManifestResourceStream($"{projectName}.{fontName}"))
            {
                if (stream == null)
                {
                    throw new Exception("资源文件不存在");
                }
                byte[] fontData = new byte[stream.Length];

                stream.Read(fontData, 0, (int)stream.Length);
                stream.Close();

                unsafe  // 属性设置，生成中 允许不安全代码
                {
                    // 将字体添加到PrivateFontCollection
                    fixed (byte* pFontData = fontData)
                    {
                        pfc.AddMemoryFont((System.IntPtr)pFontData, fontData.Length);
                    }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            button1.FlatStyle = button2.FlatStyle = button3.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = button2.FlatAppearance.BorderSize = button3.FlatAppearance.BorderSize = 0;
            button1.ForeColor = button2.ForeColor = button3.ForeColor = Color.White;

            button1.BackColor = Color.MediumPurple;
            button2.BackColor = Color.CadetBlue;
            button3.BackColor = Color.OrangeRed;

            //// 黑体
            //button1.Font = button2.Font = button3.Font = new Font("SimHei", 11f, FontStyle.Regular);

            //// 微软雅黑
            //button1.Font = button2.Font = button3.Font = new Font("Microsoft Yahei", 11f, FontStyle.Regular);

            //// 隶书
            //button1.Font = button2.Font = button3.Font = new Font("LiSu", 13f, FontStyle.Regular);

            //// 华文行楷
            //button1.Font = button2.Font = button3.Font = new Font("STXingkai", 13f, FontStyle.Regular);

            AddPrivateFont();

            // 使用内存字体，需要设置 SetCompatibleTextRenderingDefault(true) 或 UseCompatibleTextRendering=true
            button1.UseCompatibleTextRendering = button2.UseCompatibleTextRendering = button3.UseCompatibleTextRendering = true;
            // 使用字体文件提供的字体
            //button1.Font = new Font(pfc.Families[2], 12f);
            //button2.Font = new Font(pfc.Families[1], 12f);
            button3.Font = new Font(pfc.Families[0], 15f);
            //button2.TextAlign = ContentAlignment.BottomCenter;

            Paint += Form1_Paint;

           
        }

        /// <summary>
        /// 窗体的Paint事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font(pfc.Families[0], 20);
            g.DrawString("我是使用内存字体写的文字内容", font, new SolidBrush(Color.PaleVioletRed), 200, 300);
            
        }
    }
}
