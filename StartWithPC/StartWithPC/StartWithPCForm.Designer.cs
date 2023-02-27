namespace StartWithPC
{
    partial class StartWithPCForm
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
            this.runWithPC_Admin_Btn = new System.Windows.Forms.Button();
            this.runWithPCBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // runWithPC_Admin_Btn
            // 
            this.runWithPC_Admin_Btn.Location = new System.Drawing.Point(78, 69);
            this.runWithPC_Admin_Btn.Name = "runWithPC_Admin_Btn";
            this.runWithPC_Admin_Btn.Size = new System.Drawing.Size(105, 43);
            this.runWithPC_Admin_Btn.TabIndex = 1;
            this.runWithPC_Admin_Btn.Text = "设置为开机启动（管理员）";
            this.runWithPC_Admin_Btn.UseVisualStyleBackColor = true;
            this.runWithPC_Admin_Btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // runWithPCBtn
            // 
            this.runWithPCBtn.Location = new System.Drawing.Point(78, 166);
            this.runWithPCBtn.Name = "runWithPCBtn";
            this.runWithPCBtn.Size = new System.Drawing.Size(105, 43);
            this.runWithPCBtn.TabIndex = 2;
            this.runWithPCBtn.Text = "设置为开机启动（非管理员）";
            this.runWithPCBtn.UseVisualStyleBackColor = true;
            this.runWithPCBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // StartWithPCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 450);
            this.Controls.Add(this.runWithPCBtn);
            this.Controls.Add(this.runWithPC_Admin_Btn);
            this.Name = "StartWithPCForm";
            this.Text = "StartWithPC";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button runWithPC_Admin_Btn;
        private System.Windows.Forms.Button runWithPCBtn;
    }
}

