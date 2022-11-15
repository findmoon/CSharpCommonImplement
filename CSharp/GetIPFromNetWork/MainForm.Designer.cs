namespace GetIPFromNetWork
{
    partial class MainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.listBoxGetAllUnicastAddresses2 = new System.Windows.Forms.ListBox();
            this.listBoxGetAllUnicastAddresses_New = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxLocalIps = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(159, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 48);
            this.button1.TabIndex = 0;
            this.button1.Text = "获取所有IP地址";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBoxGetAllUnicastAddresses2
            // 
            this.listBoxGetAllUnicastAddresses2.FormattingEnabled = true;
            this.listBoxGetAllUnicastAddresses2.ItemHeight = 12;
            this.listBoxGetAllUnicastAddresses2.Location = new System.Drawing.Point(8, 134);
            this.listBoxGetAllUnicastAddresses2.Name = "listBoxGetAllUnicastAddresses2";
            this.listBoxGetAllUnicastAddresses2.Size = new System.Drawing.Size(219, 304);
            this.listBoxGetAllUnicastAddresses2.TabIndex = 1;
            // 
            // listBoxGetAllUnicastAddresses_New
            // 
            this.listBoxGetAllUnicastAddresses_New.FormattingEnabled = true;
            this.listBoxGetAllUnicastAddresses_New.ItemHeight = 12;
            this.listBoxGetAllUnicastAddresses_New.Location = new System.Drawing.Point(259, 134);
            this.listBoxGetAllUnicastAddresses_New.Name = "listBoxGetAllUnicastAddresses_New";
            this.listBoxGetAllUnicastAddresses_New.Size = new System.Drawing.Size(223, 304);
            this.listBoxGetAllUnicastAddresses_New.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "GetAllUnicastAddresses2：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "GetAllUnicastAddresses_New：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(545, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "LocalIps：";
            // 
            // listBoxLocalIps
            // 
            this.listBoxLocalIps.FormattingEnabled = true;
            this.listBoxLocalIps.ItemHeight = 12;
            this.listBoxLocalIps.Location = new System.Drawing.Point(547, 134);
            this.listBoxLocalIps.Name = "listBoxLocalIps";
            this.listBoxLocalIps.Size = new System.Drawing.Size(223, 304);
            this.listBoxLocalIps.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBoxLocalIps);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxGetAllUnicastAddresses_New);
            this.Controls.Add(this.listBoxGetAllUnicastAddresses2);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "IPLookUp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBoxGetAllUnicastAddresses2;
        private System.Windows.Forms.ListBox listBoxGetAllUnicastAddresses_New;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxLocalIps;
    }
}

