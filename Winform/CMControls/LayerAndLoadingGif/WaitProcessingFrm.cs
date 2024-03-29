﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CMControls.Layer
{
    /// <summary>
    /// 等待处理的遮罩效果的窗体
    /// </summary>
    public partial class WaitProcessingFrm : Form
    {
        private static Image m_Image = null;

        private EventHandler evtHandler = null;

        private ParameterizedThreadStart workAction = null;
        private object workActionArg = null;

        private Thread workThread = null;

        public string Message
        {
            get
            {
                return lbMessage.Text;
            }
            set
            {
                lbMessage.Text = value;
            }
        }

        public bool WorkCompleted = false;

        public Exception WorkException
        { get; private set; }

        public void SetWorkAction(ParameterizedThreadStart workAction, object arg)
        {
            this.workAction = workAction;
            this.workActionArg = arg;
        }

        public WaitProcessingFrm(string msg)
        {
            InitializeComponent();
            this.Message = msg;
            ShowInTaskbar = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_Image != null)
            {
                //获得当前gif动画下一步要渲染的帧。
                UpdateImage();

                //将获得的当前gif动画需要渲染的帧显示在界面上的某个位置。
                int x = (int)(panImage.ClientRectangle.Width - m_Image.Width) / 2;
                int y = 0;
                //e.Graphics.DrawImage(m_Image, new Rectangle(x, y, m_Image.Width, m_Image.Height));
                panImage.CreateGraphics().DrawImage(m_Image, new Rectangle(x, y, m_Image.Width, m_Image.Height));
            }
            if (this.WorkCompleted)
            {
                this.Close();
            }
        }


        private void FrmProcessing_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(this.Owner.Left, this.Owner.Top);
                //MessageBox.Show(string.Format("X={0},Y={1}", this.Owner.Left, this.Owner.Top));
                this.Width = this.Owner.Width;
                this.Height = this.Owner.Height;
            }
            else
            {
                Rectangle screenRect = Screen.PrimaryScreen.WorkingArea;
                this.Location = new Point((screenRect.Width - this.Width) / 2, (screenRect.Height - this.Height) / 2);
            }

            //为委托关联一个处理方法
            evtHandler = new EventHandler(OnImageAnimate);

            if (m_Image == null)
            {
                Assembly assy = Assembly.GetExecutingAssembly();
                //获取要加载的gif动画文件
                m_Image = Properties.LoadingResource.Loading_dotCircle;
            }
            //调用开始动画方法
            BeginAnimate();
        }


        //开始动画方法

        private void BeginAnimate()
        {
            if (m_Image != null)
            {
                //当gif动画每隔一定时间后，都会变换一帧，那么就会触发一事件，该方法就是将当前image每变换一帧时，都会调用当前这个委托所关联的方法。
                ImageAnimator.Animate(m_Image, evtHandler);
            }
        }

        //委托所关联的方法

        private void OnImageAnimate(Object sender, EventArgs e)
        {
            //该方法中，只是使得当前这个winform重绘，然后去调用该winform的OnPaint（）方法进行重绘)
            this.Invalidate();
        }

        //获得当前gif动画的下一步需要渲染的帧，当下一步任何对当前gif动画的操作都是对该帧进行操作)

        private void UpdateImage()
        {
            ImageAnimator.UpdateFrames(m_Image);
        }

        //关闭显示动画，该方法可以在winform关闭时，或者某个按钮的触发事件中进行调用，以停止渲染当前gif动画。

        private void StopAnimate()
        {
            m_Image = null;
            ImageAnimator.StopAnimate(m_Image, evtHandler);
        }

        private void FrmProcessing_Shown(object sender, EventArgs e)
        {
            if (this.workAction != null)
            {
                workThread = new Thread(ExecWorkAction);
                workThread.IsBackground = true;
                workThread.Start();
            }
        }

        private void ExecWorkAction()
        {
            try
            {
                var workTask = new Task((arg) =>
                {
                    this.workAction(arg);
                },
                            this.workActionArg);

                workTask.Start();
                Task.WaitAll(workTask);
            }
            catch (Exception ex)
            {
                this.WorkException = ex;
            }
            finally
            {
                this.WorkCompleted = true;
            }

        }
    }

    partial class WaitProcessingFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbMessage = new System.Windows.Forms.Label();
            this.panImage = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lbMessage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panImage, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(582, 318);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lbMessage
            // 
            this.lbMessage.BackColor = System.Drawing.Color.Transparent;
            this.lbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMessage.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMessage.Location = new System.Drawing.Point(3, 0);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
            this.lbMessage.Size = new System.Drawing.Size(576, 159);
            this.lbMessage.TabIndex = 1;
            this.lbMessage.Text = "lbMessage\r\nadsfadsf";
            this.lbMessage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panImage
            // 
            this.panImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panImage.Location = new System.Drawing.Point(3, 162);
            this.panImage.Name = "panImage";
            this.panImage.Size = new System.Drawing.Size(576, 153);
            this.panImage.TabIndex = 2;
            // 
            // FrmProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(582, 318);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmProcessing";
            this.Opacity = 0.85D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmProcessing";
            this.Load += new System.EventHandler(this.FrmProcessing_Load);
            this.Shown += new System.EventHandler(this.FrmProcessing_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbMessage;
        private System.Windows.Forms.Panel panImage;


    }
}