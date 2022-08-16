namespace MaskedTextBoxExample
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
            this.maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.maskTypeCbx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.copyMaskedFmtCbx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textMaskFmtCbx = new System.Windows.Forms.ComboBox();
            this.beepOnErrorCbx = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pwdCharTxt = new System.Windows.Forms.TextBox();
            this.promptCharTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.rejectOnFailureCbx = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // maskedTextBox
            // 
            this.maskedTextBox.Location = new System.Drawing.Point(83, 138);
            this.maskedTextBox.Name = "maskedTextBox";
            this.maskedTextBox.PasswordChar = '啊';
            this.maskedTextBox.Size = new System.Drawing.Size(213, 21);
            this.maskedTextBox.TabIndex = 0;
            // 
            // maskTypeCbx
            // 
            this.maskTypeCbx.FormattingEnabled = true;
            this.maskTypeCbx.Location = new System.Drawing.Point(83, 61);
            this.maskTypeCbx.Name = "maskTypeCbx";
            this.maskTypeCbx.Size = new System.Drawing.Size(121, 20);
            this.maskTypeCbx.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "格式类型";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "输入验证";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(296, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "(MaskedTextBox)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(224, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "复制Masked文本格式";
            // 
            // copyMaskedFmtCbx
            // 
            this.copyMaskedFmtCbx.FormattingEnabled = true;
            this.copyMaskedFmtCbx.Location = new System.Drawing.Point(343, 61);
            this.copyMaskedFmtCbx.Name = "copyMaskedFmtCbx";
            this.copyMaskedFmtCbx.Size = new System.Drawing.Size(121, 20);
            this.copyMaskedFmtCbx.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "获取 MaskedTextBox.Text 属性的格式";
            // 
            // textMaskFmtCbx
            // 
            this.textMaskFmtCbx.FormattingEnabled = true;
            this.textMaskFmtCbx.Location = new System.Drawing.Point(239, 202);
            this.textMaskFmtCbx.Name = "textMaskFmtCbx";
            this.textMaskFmtCbx.Size = new System.Drawing.Size(121, 20);
            this.textMaskFmtCbx.TabIndex = 7;
            // 
            // beepOnErrorCbx
            // 
            this.beepOnErrorCbx.AutoSize = true;
            this.beepOnErrorCbx.Location = new System.Drawing.Point(492, 63);
            this.beepOnErrorCbx.Name = "beepOnErrorCbx";
            this.beepOnErrorCbx.Size = new System.Drawing.Size(132, 16);
            this.beepOnErrorCbx.TabIndex = 9;
            this.beepOnErrorCbx.Text = "输入错误开启提示音";
            this.beepOnErrorCbx.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "密码显示字符";
            // 
            // pwdCharTxt
            // 
            this.pwdCharTxt.Location = new System.Drawing.Point(107, 100);
            this.pwdCharTxt.Name = "pwdCharTxt";
            this.pwdCharTxt.Size = new System.Drawing.Size(75, 21);
            this.pwdCharTxt.TabIndex = 11;
            // 
            // promptCharTxt
            // 
            this.promptCharTxt.Location = new System.Drawing.Point(331, 100);
            this.promptCharTxt.Name = "promptCharTxt";
            this.promptCharTxt.Size = new System.Drawing.Size(75, 21);
            this.promptCharTxt.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(224, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "未输入时提示字符";
            // 
            // rejectOnFailureCbx
            // 
            this.rejectOnFailureCbx.AutoSize = true;
            this.rejectOnFailureCbx.Location = new System.Drawing.Point(492, 102);
            this.rejectOnFailureCbx.Name = "rejectOnFailureCbx";
            this.rejectOnFailureCbx.Size = new System.Drawing.Size(216, 16);
            this.rejectOnFailureCbx.TabIndex = 14;
            this.rejectOnFailureCbx.Text = "当出现输入错误字符时禁止继续输入";
            this.rejectOnFailureCbx.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rejectOnFailureCbx);
            this.Controls.Add(this.promptCharTxt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pwdCharTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.beepOnErrorCbx);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textMaskFmtCbx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.copyMaskedFmtCbx);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.maskTypeCbx);
            this.Controls.Add(this.maskedTextBox);
            this.Name = "Form1";
            this.Text = "UseMaskedTextBoxForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox maskedTextBox;
        private System.Windows.Forms.ComboBox maskTypeCbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox copyMaskedFmtCbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox textMaskFmtCbx;
        private System.Windows.Forms.CheckBox beepOnErrorCbx;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox pwdCharTxt;
        private System.Windows.Forms.TextBox promptCharTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox rejectOnFailureCbx;
    }
}

