namespace WMIAndMIUse
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
            this.button13 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(508, 27);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(116, 37);
            this.button13.TabIndex = 26;
            this.button13.Text = "列出所有进程";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(84, 386);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(116, 37);
            this.button10.TabIndex = 25;
            this.button10.Text = "获取串口信息";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(84, 332);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(116, 37);
            this.button9.TabIndex = 24;
            this.button9.Text = "获取MAC地址";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(84, 274);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(116, 37);
            this.button8.TabIndex = 23;
            this.button8.Text = "获取硬盘序列号";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(84, 222);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(116, 37);
            this.button7.TabIndex = 22;
            this.button7.Text = "获取主板序列号";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(84, 162);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(116, 37);
            this.button6.TabIndex = 21;
            this.button6.Text = "获取CPU序列号";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(84, 27);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(116, 37);
            this.button4.TabIndex = 20;
            this.button4.Text = "获取进程信息";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 450);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
    }
}

