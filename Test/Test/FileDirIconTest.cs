using HelperCollections;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiscellaneousTest
{
    public partial class FileDirIconTest : Form
    {
        System.Windows.Forms.FlowLayoutPanel flowLayout = new System.Windows.Forms.FlowLayoutPanel();
        public FileDirIconTest()
        {
            InitializeComponent();

            AllowDrop = true;
            this.DragEnter += Form1_DragEnter1;//拖动操作进入处理
            this.DragDrop += Form1_DragDrop1;//拖动操作确认处理

            Load += Form_Load;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            label1.Text = "\r\n\r\n";
            label1.Text = label1.Text + "可将需要测试的文件或文件夹拖入窗口，点测试方法看效果\r\n\r\n";
            label1.Text = label1.Text + "方法1：只对文件有效，且只能获取1个图标\r\n\r\n";
            label1.Text = label1.Text + "方法2：只对应用程序文件有效\r\n\r\n";
            label1.Text = label1.Text + "方法3：文件或文件夹都有效，可获取4种不同大小图标\r\n\r\n";
            label1.Text = label1.Text + "方法4：文件或文件夹都有效，只能获取2个图标\r\n\r\n";

            label1.Text = label1.Text + "获取系统所有图标后，再测试单个文件将导致有黑图标出现，重开程序会恢复正常，可能是系统缓存的原因。\r\n\r\n";




            flowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayout.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(flowLayout);
        }
        private void Form1_DragDrop1(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            textBox1.Text = s[0];
        }

        private void Form1_DragEnter1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;//拖动时的图标
            else
                e.Effect = DragDropEffects.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            flowLayout.Controls.Clear();

            test1(textBox1.Text);

            if (flowLayout.Controls.Count == 0)
                label1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            flowLayout.Controls.Clear();

            test2(textBox1.Text);

            if (flowLayout.Controls.Count == 0)
                label1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            flowLayout.Controls.Clear();

            test3(textBox1.Text);

            if (flowLayout.Controls.Count == 0)
                label1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            flowLayout.Controls.Clear();

            test4(textBox1.Text);

            if (flowLayout.Controls.Count == 0)
                label1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            flowLayout.Controls.Clear();
            test5();
            if (flowLayout.Controls.Count == 0)
                label1.Visible = true;
            else
                MessageBox.Show(string.Format("总计获取[{0}]个图标", flowLayout.Controls.Count));
        }

        private void button6_Click(object sender, EventArgs e)
        {

            label1.Visible = false;
            flowLayout.Controls.Clear();
            test6();
            if (flowLayout.Controls.Count == 0)
                label1.Visible = true;
            else
                MessageBox.Show(string.Format("总计获取[{0}]个图标", flowLayout.Controls.Count));
        }


        private void test1(string name)
        {
            System.Drawing.Icon icon = FileIconHelper.GetIconFromFile(name);
            if (icon == null)
                return;
            System.Drawing.Bitmap bitmap = icon.ToBitmap();

            System.Windows.Forms.PictureBox pic = new System.Windows.Forms.PictureBox();
            pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            flowLayout.Controls.Add(pic);
            pic.Image = bitmap;
            icon.Dispose();
        }
        private void test2(string name)
        {
            foreach (System.Drawing.Icon icon in FileIconHelper.GetIconFromFile2(name))
            {
                System.Drawing.Bitmap bitmap = icon.ToBitmap();
                System.Windows.Forms.PictureBox pic = new System.Windows.Forms.PictureBox();
                pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
                flowLayout.Controls.Add(pic);
                pic.Image = bitmap;
                icon.Dispose();
            }
        }
        private void test3(string name)
        {
            System.Drawing.Icon icon = FileDirIconHelper_ImageList.GetFileDirIcon(name);
            System.Drawing.Bitmap bitmap = icon.ToBitmap();
            System.Windows.Forms.PictureBox pic = new System.Windows.Forms.PictureBox();
            pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            flowLayout.Controls.Add(pic);
            pic.Image = bitmap;
            icon.Dispose();

            icon = FileDirIconHelper_ImageList.GetFileDirIcon(name, IMAGELIST_SIZE_FLAG.SHIL_LARGE);
            bitmap = icon.ToBitmap();
            pic = new System.Windows.Forms.PictureBox();
            pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            flowLayout.Controls.Add(pic);
            pic.Image = bitmap;
            icon.Dispose();

            icon = FileDirIconHelper_ImageList.GetFileDirIcon(name, IMAGELIST_SIZE_FLAG.SHIL_EXTRALARGE);
            bitmap = icon.ToBitmap();
            pic = new System.Windows.Forms.PictureBox();
            pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            flowLayout.Controls.Add(pic);
            pic.Image = bitmap;
            icon.Dispose();

            icon = FileDirIconHelper_ImageList.GetFileDirIcon(name, IMAGELIST_SIZE_FLAG.SHIL_JUMBO);
            bitmap = icon.ToBitmap();
            pic = new System.Windows.Forms.PictureBox();
            pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            flowLayout.Controls.Add(pic);
            pic.Image = bitmap;
            icon.Dispose();
        }
        private void test4(string name)
        {
            System.Drawing.Icon icon = FileDirIconHelper.GetFileDirIcon(name);
            if (icon == null)
                return;
            System.Drawing.Bitmap bitmap = icon.ToBitmap();
            System.Windows.Forms.PictureBox pic = new System.Windows.Forms.PictureBox();
            pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            flowLayout.Controls.Add(pic);
            pic.Image = bitmap;
            icon.Dispose();

            icon = FileDirIconHelper.GetFileDirIcon(name, true);
            bitmap = icon.ToBitmap();
            pic = new System.Windows.Forms.PictureBox();
            pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            flowLayout.Controls.Add(pic);
            pic.Image = bitmap;
            icon.Dispose();

            //icon = MySystemIcon.GetSystemIconB.GetDirectoryIcon(System.IO.Directory.GetParent(name).FullName, false);
            //bitmap = icon.ToBitmap();
            //pic = new System.Windows.Forms.PictureBox();
            //pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            //flowLayout.Controls.Add(pic);
            //pic.Image = bitmap;
            //icon.Dispose();

            //icon = MySystemIcon.GetSystemIconB.GetDirectoryIcon(System.IO.Directory.GetParent(name).FullName, true);
            //bitmap = icon.ToBitmap();
            //pic = new System.Windows.Forms.PictureBox();
            //pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
            //flowLayout.Controls.Add(pic);
            //pic.Image = bitmap;
            //icon.Dispose();
        }
        /// <summary>
        /// 读取所有系统图标
        /// </summary>
        private void test5()
        {
            #region 多次（第二次）调用时就会出现`System.ArgumentException`的异常
            var icons_1 = FileDirIconHelper_ImageList.GetImageList(IMAGELIST_SIZE_FLAG.SHIL_SYSSMALL);
            SetIconPictureBox(icons_1);

            var icons_2 = FileDirIconHelper_ImageList.GetImageList(IMAGELIST_SIZE_FLAG.SHIL_SMALL);
            SetIconPictureBox(icons_2);
            var icons_3 = FileDirIconHelper_ImageList.GetImageList(IMAGELIST_SIZE_FLAG.SHIL_LARGE);
            SetIconPictureBox(icons_3);
            var icons_4 = FileDirIconHelper_ImageList.GetImageList(IMAGELIST_SIZE_FLAG.SHIL_EXTRALARGE);
            SetIconPictureBox(icons_4);
            var icons_5 = FileDirIconHelper_ImageList.GetImageList(IMAGELIST_SIZE_FLAG.SHIL_JUMBO);
            SetIconPictureBox(icons_5);
            //var icons_6 = FileDirIconHelper_ImageList.GetImageList(IMAGELIST_SIZE_FLAG.SHIL_LAST);
            //SetIconPictureBox(icons_6);
            #endregion

            // | 无法使用
            //var icons_1 = FileDirIconHelper_ImageList.GetImageList(IMAGELIST_SIZE_FLAG.SHIL_SYSSMALL | IMAGELIST_SIZE_FLAG.SHIL_SMALL | IMAGELIST_SIZE_FLAG.SHIL_LARGE |
            //        IMAGELIST_SIZE_FLAG.SHIL_EXTRALARGE | IMAGELIST_SIZE_FLAG.SHIL_JUMBO | IMAGELIST_SIZE_FLAG.SHIL_LAST);
            //SetIconPictureBox(icons_1);
        }

        private void SetIconPictureBox(Icon[] icons_1)
        {
            for (int i = 0; i < icons_1.Length; i++)
            {
                var icon = icons_1[i];
                System.Drawing.Bitmap bitmap = icon.ToBitmap();
                System.Windows.Forms.PictureBox pic = new System.Windows.Forms.PictureBox();
                pic.Size = new System.Drawing.Size(icon.Size.Width, icon.Size.Height);
                flowLayout.Controls.Add(pic);
                pic.Image = bitmap;
                icon.Dispose();
            }
        }

        /// <summary>
        /// 读取所有32*32系统图标
        /// </summary>
        private void test6()
        {
            var icons_1 = FileDirIconHelper_ImageList.GetImageList(IMAGELIST_SIZE_FLAG.SHIL_LARGE);
            SetIconPictureBox(icons_1);
        }
    }
}
