namespace TimeStampsCalc
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
            this.button1 = new System.Windows.Forms.Button();
            this.timestampsTxt = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.ticksTxt = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.dtNowUtcNowTxt = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(76, 344);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "时间戳";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timestampsTxt
            // 
            this.timestampsTxt.Location = new System.Drawing.Point(12, 39);
            this.timestampsTxt.Multiline = true;
            this.timestampsTxt.Name = "timestampsTxt";
            this.timestampsTxt.Size = new System.Drawing.Size(366, 288);
            this.timestampsTxt.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(496, 353);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(203, 46);
            this.button2.TabIndex = 2;
            this.button2.Text = "计时周期Ticks";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ticksTxt
            // 
            this.ticksTxt.Location = new System.Drawing.Point(397, 39);
            this.ticksTxt.Multiline = true;
            this.ticksTxt.Name = "ticksTxt";
            this.ticksTxt.Size = new System.Drawing.Size(401, 288);
            this.ticksTxt.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1, 453);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(139, 46);
            this.button3.TabIndex = 4;
            this.button3.Text = "DateTime的Now和UtcNow";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dtNowUtcNowTxt
            // 
            this.dtNowUtcNowTxt.Location = new System.Drawing.Point(146, 434);
            this.dtNowUtcNowTxt.Multiline = true;
            this.dtNowUtcNowTxt.Name = "dtNowUtcNowTxt";
            this.dtNowUtcNowTxt.Size = new System.Drawing.Size(652, 81);
            this.dtNowUtcNowTxt.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 542);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(139, 46);
            this.button4.TabIndex = 6;
            this.button4.Text = "DateTime的Now和UtcNow";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 634);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.dtNowUtcNowTxt);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ticksTxt);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.timestampsTxt);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "获取时间戳";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox timestampsTxt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox ticksTxt;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox dtNowUtcNowTxt;
        private System.Windows.Forms.Button button4;
    }
}

