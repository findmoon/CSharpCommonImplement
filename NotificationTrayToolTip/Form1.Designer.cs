namespace NotificationTrayToolTip
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonPro1 = new CMControls.ButtonPro();
            this.buttonPro2 = new CMControls.ButtonPro();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // buttonPro1
            // 
            this.buttonPro1.FlatAppearance.BorderSize = 0;
            this.buttonPro1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro1.ForeColor = System.Drawing.Color.White;
            this.buttonPro1.Location = new System.Drawing.Point(12, 40);
            this.buttonPro1.Name = "buttonPro1";
            this.buttonPro1.Size = new System.Drawing.Size(194, 47);
            this.buttonPro1.TabIndex = 0;
            this.buttonPro1.Text = "显示系统托盘气泡提示框";
            this.buttonPro1.UseVisualStyleBackColor = true;
            this.buttonPro1.Click += new System.EventHandler(this.buttonPro1_Click);
            // 
            // buttonPro2
            // 
            this.buttonPro2.FlatAppearance.BorderSize = 0;
            this.buttonPro2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro2.ForeColor = System.Drawing.Color.White;
            this.buttonPro2.Location = new System.Drawing.Point(256, 40);
            this.buttonPro2.Name = "buttonPro2";
            this.buttonPro2.RoundNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(30)))), ((int)(((byte)(120)))));
            this.buttonPro2.Size = new System.Drawing.Size(194, 47);
            this.buttonPro2.TabIndex = 1;
            this.buttonPro2.Text = "关闭系统托盘气泡提示框";
            this.buttonPro2.UseVisualStyleBackColor = true;
            this.buttonPro2.Click += new System.EventHandler(this.buttonPro2_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 135);
            this.Controls.Add(this.buttonPro2);
            this.Controls.Add(this.buttonPro1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private CMControls.ButtonPro buttonPro1;
        private CMControls.ButtonPro buttonPro2;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

