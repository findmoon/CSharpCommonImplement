using CMControls.Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragDropUse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            #region 窗体拖放
            AllowDrop = true;
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            #endregion

            //pictureBox1.Image = LoadingImg.LoadingSimple;


        }

        #region 窗体拖放
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // 判断拖拽的数据格式
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }
        private async void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            await InitialFileOrDir(paths);
        }
        #endregion

        /// <summary>
        /// 选择文件或文件夹时的初始化
        /// </summary>
        /// <param name="fileOrDir"></param>
        private async Task InitialFileOrDir(string[] fileOrDirs)
        {
            if (fileOrDirs==null || fileOrDirs.Length==0)
            {
                return;
            }

            using (var layer = new CMLayer())
            {
                //Controls.Add(layer); // Add 需调用BringToFront
                //layer.BringToFront();
                // 推荐Parent
                layer.Parent = this;

                // 显示操作进度
                using (var progressBar = new ProgressBar())
                {
                    #region 遮罩层上添加一个progressBar【layer.Controls.Add的方式】
                    //progressBar.Style = ProgressBarStyle.Marquee;
                    //progressBar.Width = layer.Width - 10;

                    //// 居中
                    //progressBar.Location = new Point((layer.Width - progressBar.Width) / 2, (layer.Height - progressBar.Height) / 2);

                    //layer.Controls.Add(progressBar);
                    ////progressBar.BringToFront();
                    #endregion

                    #region 遮罩层上添加一个progressBar【ControlOnLayer属性的方式】
                    progressBar.Style = ProgressBarStyle.Marquee;
                    progressBar.Width = layer.Width - 10;

                    layer.ControlOnLayer = progressBar;
                    #endregion

                    #region 加载图片
                    layer.LayerImage = LoadingImg.LoadingSimple;
                    //layer.LayerImage = LoadingImg.LoadingStripBar;
                    //layer.LayerImage = LoadingImg.LoadingStripBar2;
                    layer.LayerColor = Color.CornflowerBlue;
                    #endregion

                    layer.Show();

                    // 显示拖入的文件所在路径
                    if (File.Exists(fileOrDirs[0]))
                    {
                        dirLinkLabel.Text = Path.GetDirectoryName(fileOrDirs[0]);
                    }
                    else if (Directory.Exists(fileOrDirs[0]))
                    {
                        dirLinkLabel.Text = fileOrDirs[0];
                    }
                    else
                    {
                        dirLinkLabel.Text = "";
                    }

                    targetFilesTreeView.Nodes.Clear();
                    foreach (var fileOrDir in fileOrDirs)
                    {
                        var node = new TreeNode(Path.GetFileName(fileOrDir));
                       
                        if (File.Exists(fileOrDir))
                        {
                            node.Tag = Path.GetDirectoryName(fileOrDir);
                        }
                        else if (Directory.Exists(fileOrDir))
                        {
                            await Task.Run(() => node.MapDirToTreeNode(fileOrDir, onlyCurrDir:false));
                        }

                        targetFilesTreeView.Nodes.Add(node); // 放在最后直接添加，如果node的节点和后代节点很多，则可能添加就会花费很长时间并卡顿
                        // 最好的处理办法是，延迟加载，当展开节点时，如果未加载再执行加载；已加载则直接展开
                    }
                }

                layer.Hide();
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var layer = new CMLayer();

            this.Controls.Add(layer); // Add 需调用BringToFront
            layer.BringToFront();
            layer.Show();
            layer.LayerColor = Color.MediumPurple;

      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WaitProcessingFrm processingForm = new WaitProcessingFrm("等待效果的窗体");
            processingForm.SetWorkAction(new System.Threading.ParameterizedThreadStart(arg => {
                Thread.Sleep((int)arg * 1000);
            }), 10);

            //processingForm.Opacity = 0.2;
            processingForm.ShowDialog(this);
            if (processingForm.WorkException != null)
            {
                throw processingForm.WorkException;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var onLayerForm = new OnLayerForm();
            var layerForm = new LayerForm(this, onLayerForm);
            //layerForm.ShowDialog(); // 推荐Show();
            layerForm.Show();

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 3000;
            timer.Tick += (t, ea)=> {

                timer.Stop();

                // 处理完耗时操作，关闭Layer窗体
                layerForm.Close();
            };
            timer.Start();
        }

    }
}
