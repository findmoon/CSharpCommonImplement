namespace CustomControlRound
{
    partial class RoundTextBoxTest
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.roundTextBox2 = new CMControls.Rounds.RoundTextBoxNo();
            this.roundTextBox1 = new CMControls.Rounds.RoundTextBoxNo();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(193, 249);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(131, 14);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "你好";
            // 
            // roundTextBox2
            // 
            this.roundTextBox2.BackColor = System.Drawing.SystemColors.Window;
            this.roundTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.roundTextBox2.Location = new System.Drawing.Point(397, 248);
            this.roundTextBox2.Name = "roundTextBox2";
            this.roundTextBox2.RoundBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.roundTextBox2.RoundBorderSize = 2;
            this.roundTextBox2.RoundNormalColor = System.Drawing.SystemColors.Window;
            this.roundTextBox2.RoundRadius = -20;
            this.roundTextBox2.Size = new System.Drawing.Size(100, 14);
            this.roundTextBox2.TabIndex = 2;
            this.roundTextBox2.Text = "DD";
            // 
            // roundTextBox1
            // 
            this.roundTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.roundTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.roundTextBox1.Font = new System.Drawing.Font("宋体", 19F);
            this.roundTextBox1.Location = new System.Drawing.Point(364, 170);
            this.roundTextBox1.Name = "roundTextBox1";
            this.roundTextBox1.RoundBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.roundTextBox1.RoundBorderSize = 2;
            this.roundTextBox1.RoundNormalColor = System.Drawing.SystemColors.Window;
            this.roundTextBox1.RoundRadius = 16;
            this.roundTextBox1.Size = new System.Drawing.Size(361, 36);
            this.roundTextBox1.TabIndex = 1;
            this.roundTextBox1.Text = "你好我是圆角文本框？";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(266, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "失败实现，正确实现参见TextBoxPro控件";
            // 
            // RoundTextBoxTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.roundTextBox2);
            this.Controls.Add(this.roundTextBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "RoundTextBoxTest";
            this.Text = "RoundTextBoxTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private CMControls.Rounds.RoundTextBoxNo roundTextBox1;
        private CMControls.Rounds.RoundTextBoxNo roundTextBox2;
        private System.Windows.Forms.Label label1;
    }
}