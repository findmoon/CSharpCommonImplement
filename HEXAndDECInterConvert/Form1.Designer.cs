namespace HEXAndDECInterConvert
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
            this.label1 = new System.Windows.Forms.Label();
            this.hexFromTxt = new System.Windows.Forms.TextBox();
            this.decToTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.decToTxt2 = new System.Windows.Forms.TextBox();
            this.decFromTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.hexToTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "16进制";
            // 
            // hexFromTxt
            // 
            this.hexFromTxt.Location = new System.Drawing.Point(47, 60);
            this.hexFromTxt.Name = "hexFromTxt";
            this.hexFromTxt.Size = new System.Drawing.Size(149, 21);
            this.hexFromTxt.TabIndex = 2;
            // 
            // decToTxt
            // 
            this.decToTxt.Location = new System.Drawing.Point(258, 46);
            this.decToTxt.Name = "decToTxt";
            this.decToTxt.Size = new System.Drawing.Size(149, 21);
            this.decToTxt.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "10进制";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(218, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "10进制";
            // 
            // decToTxt2
            // 
            this.decToTxt2.Location = new System.Drawing.Point(258, 73);
            this.decToTxt2.Name = "decToTxt2";
            this.decToTxt2.Size = new System.Drawing.Size(149, 21);
            this.decToTxt2.TabIndex = 6;
            // 
            // decFromTxt
            // 
            this.decFromTxt.Location = new System.Drawing.Point(56, 277);
            this.decFromTxt.Name = "decFromTxt";
            this.decFromTxt.Size = new System.Drawing.Size(149, 21);
            this.decFromTxt.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 281);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "10进制";
            // 
            // hexToTxt
            // 
            this.hexToTxt.Location = new System.Drawing.Point(253, 277);
            this.hexToTxt.Name = "hexToTxt";
            this.hexToTxt.Size = new System.Drawing.Size(149, 21);
            this.hexToTxt.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(213, 280);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "16进制";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 415);
            this.Controls.Add(this.hexToTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.decFromTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.decToTxt2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.hexFromTxt);
            this.Controls.Add(this.decToTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "十进制和十六进制转换";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hexFromTxt;
        private System.Windows.Forms.TextBox decToTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox decToTxt2;
        private System.Windows.Forms.TextBox decFromTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox hexToTxt;
        private System.Windows.Forms.Label label5;
    }
}

