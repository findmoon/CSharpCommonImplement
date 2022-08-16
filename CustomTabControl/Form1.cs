using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomTabControl
{
    public partial class Form1 : Form
    {
        Color tabSelectedBackColor = Color.IndianRed;
        Color tabSelectedForeColor = Color.White;

        public Form1()
        {
            InitializeComponent();

            Form2 form2 = new Form2();
            form2.Show();

            //tabControl1.HotTrack = true;

            //tabControl1.Alignment = TabAlignment.Left;

            //tabControl1.Appearance = TabAppearance.FlatButtons;
            //tabControl1.Appearance = TabAppearance.Buttons;

            // 多行tab
            //tabControl1.Multiline = true;

            //for (int i = 0; i < 6; i++)
            //{
            //    //var page = new TabPage("项目" + (2 + i));
            //    //page.BackColor = Color.Transparent;
            //    tabControl1.TabPages.Add("项目" + (2 + i));
            //}

            //tabPage2.Hide(); // 无效
            //tabPage2.Parent = null;
            //tabControl1.TabPages.Remove(tabPage2);

            #region Hide tab
            //tabControl1.SizeMode = TabSizeMode.Fixed;
            //foreach (TabPage page in tabControl1.TabPages)
            //{
            //    page.Text = "";
            //}
            //tabControl1.ItemSize = new Size(0, 1);
            //// 去除线条效果（上面的设置仍然会在tabControl的顶部出现线条）
            //tabControl1.Appearance = TabAppearance.FlatButtons; 
            #endregion

            // 阻止选择某些tab
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;



            #region 绘制两边tab时的样式和文字
            //tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            //tabControl1.DrawItem += TabControl1_DrawItem; 
            #endregion

            #region 设置tab图标
            //tabControl1.ImageList = imageList1;
            //tabPage1.ImageIndex = 0;
            //tabPage2.ImageIndex = 1;
            //tabPage3.ImageIndex = 2; 
            #endregion

            #region 添加add和close按钮
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add("add", "add");
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += TabControl1_DrawItem2;

            // 创建句柄时触发。通过发送消息SendMessage是最后一个添加tab尽可能短。效果不明显
            tabControl1.HandleCreated += TabControl1_HandleCreated;

            // 如果只有一个tab，SelectedIndexChanged将无效
            //tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged1;

            tabControl1.MouseDown += TabControl1_MouseDown;
            tabControl1.MouseMove += TabControl1_MouseMove;

            // RTL的绘制优化
            tabControl1.RightToLeft = RightToLeft.Yes;
            tabControl1.RightToLeftLayout = true;
            #endregion


        }

        /// <summary>
        /// 鼠标Hover关闭按钮效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            // 依次循环判断，鼠标点击位置是否位于close图片范围内
            for (var i = 0; i < tabControl1.TabPages.Count - 1; i++)
            {
                var tabPage = tabControl1.TabPages[i];
                var tabRect = GetTabRect(tabControl1,i);
                //tabRect.Inflate(0, -2); // 似乎未其作用
                var closeImage = imageList1.Images["close"];
                var imageRect = new Rectangle(
                    tabRect.Right - closeImage.Width - (tabControl1.RightToLeftLayout && tabControl1.RightToLeft == RightToLeft.Yes?4:2),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                    closeImage.Width,
                    closeImage.Height);

                var mousePos = e.Location;
                if (tabControl1.RightToLeftLayout && tabControl1.RightToLeft == RightToLeft.Yes) // RTL调整鼠标位置
                {
                    mousePos.X = tabControl1.Right-mousePos.X;
                }
                if (imageRect.Contains(mousePos))
                {
                    if (tabPage.Tag?.ToString() != "1")
                    {
                        using (var g = tabControl1.CreateGraphics())
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 100, 100, 45)), imageRect);
                            tabPage.Tag = "1";//表示已有透明灰色背景
                        }
                    }
                    break;
                }
                else
                {
                    if (tabPage.Tag?.ToString() == "1")//清除已有的灰色背景
                    {
                        using (var g = tabControl1.CreateGraphics())
                        {
                            g.FillRectangle(new SolidBrush(tabPage.BackColor), imageRect);
                            g.DrawImage(closeImage, imageRect);
                            tabPage.Tag = null;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// RTF坐标
        /// </summary>
        /// <param name="container"></param>
        /// <param name="drawRectangle"></param>
        /// <returns></returns>
        public static Rectangle GetRTLCoordinates(Rectangle container, Rectangle drawRectangle)
        {
            return new Rectangle(
                container.Right - drawRectangle.Width - drawRectangle.X,
                drawRectangle.Y,
                drawRectangle.Width,
                drawRectangle.Height);
        }
        /// <summary>
        /// 绘制tab时需要考虑RTF模式
        /// </summary>
        /// <param name="tabCtl"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static Rectangle GetTabRect(TabControl tabCtl, int idx)
        {
            var tabRect = tabCtl.GetTabRect(idx);
            if (tabCtl.RightToLeftLayout && tabCtl.RightToLeft == RightToLeft.Yes) // RTL
            {
                tabRect = GetRTLCoordinates(tabCtl.ClientRectangle, tabRect);
            }
            return tabRect;
        }
        /// <summary>
        /// 获取鼠标按下时的位置，并依次判断是否点击在close按钮上或最后一个“添加”按钮的tab上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return; // 不是左键返回
            // 依次循环判断，鼠标点击位置是否位于close图片范围内；或是否位于“添加”按钮tab内
            for (var i = 0; i < tabControl1.TabPages.Count; i++)
            {
                var tabRect = GetTabRect(tabControl1, i);
                var mousePos = e.Location;
                if (tabControl1.RightToLeftLayout && tabControl1.RightToLeft == RightToLeft.Yes) // RTL调整鼠标位置
                {
                    mousePos.X = tabControl1.Right - mousePos.X;
                }
                if (i == tabControl1.TabPages.Count - 1) // 组后一个 add 按钮
                {
                    if (tabRect.Contains(mousePos))
                    {
                        CreateTabPage();
                    }
                }
                else
                {
                    //tabRect.Inflate(0, -2);
                    var closeImage = imageList1.Images["close"];
                    var imageRect = new Rectangle(
                        //tabRect.Right - closeImage.Width - 2,
                        tabRect.Right - closeImage.Width - (tabControl1.RightToLeftLayout && tabControl1.RightToLeft == RightToLeft.Yes ? 4 : 2),
                        tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                        closeImage.Width,
                        closeImage.Height);
                    if (imageRect.Contains(mousePos))
                    {
                        tabControl1.TabPages.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        //private void TabControl1_SelectedIndexChanged1(object sender, EventArgs e)
        //{
        //    if (tabControl1.SelectedIndex == tabControl1.TabPages.Count - 1) // 最后一创建tabpage
        //        CreateTabPage();
        //}

        private void CreateTabPage()
        {
            //insert会导致DrawItem中异常(索引错误)
            //tabControl1.TabPages.Insert(tabControl1.TabPages.Count - 1,"新选项卡"+(tabControl1.TabPages.Count - 1));
            tabControl1.TabPages.Add("新选项卡" + (tabControl1.TabPages.Count - 1));
            var addPage = tabControl1.TabPages["add"];
            tabControl1.TabPages.Remove(addPage);
            tabControl1.TabPages.Add(addPage);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int TCM_SETMINTABWIDTH = 0x1300 + 49;
        private void TabControl1_HandleCreated(object sender, EventArgs e)
        {
            SendMessage(tabControl1.Handle, TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
        }

        // 绘制add和close按钮
        private void TabControl1_DrawItem2(object sender, DrawItemEventArgs e)
        {
            var tabPage = tabControl1.TabPages[e.Index];
            var tabRect = GetTabRect(tabControl1,e.Index);

            //tabRect.Inflate(0, -2);
            //e.DrawBackground(); // 背景
            if (e.Index == tabControl1.TabCount - 1) // 最后一个TabPage
            {
                var addImage = imageList1.Images["add"]; // 也可以从路径获取new Bitmap(imagePath);
                e.Graphics.DrawImage(addImage,
                    tabRect.Left + (tabRect.Width - addImage.Width) / 2,
                    tabRect.Top + (tabRect.Height - addImage.Height) / 2);
            }
            else // 其他TabPages绘制关闭
            {
                using (var sf = new StringFormat(StringFormat.GenericDefault))
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    if (tabControl1.RightToLeft == RightToLeft.Yes && tabControl1.RightToLeftLayout == true) // RTL模式
                    {
                        sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                    }

                    var closeImage = imageList1.Images["close"];
                    var imgRect = new Rectangle(//tabRect.Right - closeImage.Width - 2,
                        tabRect.Right - closeImage.Width - (tabControl1.RightToLeftLayout && tabControl1.RightToLeft == RightToLeft.Yes ? 4 : 2),
                        tabRect.Top + (tabRect.Height - closeImage.Height) / 2, closeImage.Width, closeImage.Height);
                    e.Graphics.DrawImage(closeImage, imgRect.Location);

                    var textRect = new Rectangle(tabRect.X, tabRect.Y, tabRect.Width - closeImage.Width, tabRect.Height);
                    e.Graphics.DrawString(tabPage.Text, tabPage.Font, new SolidBrush(tabPage.ForeColor), textRect, sf); // 不使用DrawString

                    #region 无法处理RTL的情形
                    //var real_tabRect = tabControl1.GetTabRect(e.Index);
                    //var real_textRect = new Rectangle(tabRect.X, tabRect.Y, tabRect.Width - closeImage.Width, tabRect.Height);
                    //e.Graphics.DrawText(real_textRect, tabPage.Text, tabPage.ForeColor, tabPage.Font, rtl: RightToLeft.Yes); 
                    #endregion
                }
        }
        }

        // 修改两边位置时的tab
        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;
            Color _foreColor;

            // 获取当前tabPage
            TabPage _tabPage = tabControl1.TabPages[e.Index];

            // 获取tab边界
            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);
            // RTL模式下的可能处理 GetTabRect(tabControl1,i)
            // 选中与未选中的(背景)颜色
            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(tabSelectedForeColor);
                _foreColor = tabSelectedForeColor;
                g.FillRectangle(new SolidBrush(tabSelectedBackColor), e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                _foreColor = e.ForeColor;
                // tab上均有底部边框 
                e.DrawBackground();
                //g.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(_tabBounds.X, _tabBounds.Y, _tabBounds.Width+4, _tabBounds.Height+4)); // 不同的tab会不一样大小
                // g.FillRectangle(new SolidBrush(Color.Gray), e.Bounds); // 使用 e.Bounds 或 _tabBounds 边界也都有底部框  Color.Gray颜色稍微掩盖点
            }

            // Use our own font.
            // Font _tabFont = new Font("Arial", 10.0f, FontStyle.Bold, GraphicsUnit.Pixel);
            // 使用控件字体，也可以使用自定义字体
            Font _tabFont = e.Font;

            // 绘制居中字符串
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
            //e.Graphics.DrawText(_tabBounds, _tabPage.Text, _foreColor, _tabFont);
        }

        bool CredentialCheck = true;
        // 阻止选择tab
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                if (!CredentialCheck) //检查认证
                {
                    MessageBox.Show("无法加载标签页，没有足够的权限！");
                    tabControl1.SelectedTab = tabPage1; // 默认tabpage或之前的tabPage
                }
            }
        }
    }
}
