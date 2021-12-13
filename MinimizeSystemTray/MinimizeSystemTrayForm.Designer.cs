namespace MinimizeSystemTray
{
    partial class MinimizeSystemTrayForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinimizeSystemTrayForm));
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.showForm = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyCtxMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyCtxMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 16F);
            this.label1.Location = new System.Drawing.Point(182, 190);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "我是点击关闭会最小化到系统托盘的程序";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Visible = true;
            // 
            // exit
            // 
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(100, 22);
            this.exit.Text = "退出";
            // 
            // showForm
            // 
            this.showForm.Name = "showForm";
            this.showForm.Size = new System.Drawing.Size(100, 22);
            this.showForm.Text = "显示";
            // 
            // notifyCtxMenuStrip
            // 
            this.notifyCtxMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.notifyCtxMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exit,
            this.showForm});
            this.notifyCtxMenuStrip.Name = "contextMenuStrip1";
            this.notifyCtxMenuStrip.Size = new System.Drawing.Size(101, 48);
            // 
            // MinimizeSystemTrayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Name = "MinimizeSystemTrayForm";
            this.Text = "MinimizeSystemTrayForm";
            this.Load += new System.EventHandler(this.MinimizeSystemTrayForm_Load);
            this.notifyCtxMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem exit;
        private System.Windows.Forms.ToolStripMenuItem showForm;
        private System.Windows.Forms.ContextMenuStrip notifyCtxMenuStrip;
    }
}

