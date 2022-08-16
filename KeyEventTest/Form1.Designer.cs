namespace KeyEventTest
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
            this.keyTestTxt = new System.Windows.Forms.TextBox();
            this.keyEventLabel = new System.Windows.Forms.Label();
            this.keyCntLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // keyTestTxt
            // 
            this.keyTestTxt.Location = new System.Drawing.Point(259, 91);
            this.keyTestTxt.Name = "keyTestTxt";
            this.keyTestTxt.Size = new System.Drawing.Size(258, 21);
            this.keyTestTxt.TabIndex = 0;
            // 
            // keyEventLabel
            // 
            this.keyEventLabel.AutoSize = true;
            this.keyEventLabel.Location = new System.Drawing.Point(354, 185);
            this.keyEventLabel.Name = "keyEventLabel";
            this.keyEventLabel.Size = new System.Drawing.Size(41, 12);
            this.keyEventLabel.TabIndex = 1;
            this.keyEventLabel.Text = "label1";
            // 
            // keyCntLabel
            // 
            this.keyCntLabel.AutoSize = true;
            this.keyCntLabel.Location = new System.Drawing.Point(354, 219);
            this.keyCntLabel.Name = "keyCntLabel";
            this.keyCntLabel.Size = new System.Drawing.Size(41, 12);
            this.keyCntLabel.TabIndex = 2;
            this.keyCntLabel.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.keyCntLabel);
            this.Controls.Add(this.keyEventLabel);
            this.Controls.Add(this.keyTestTxt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox keyTestTxt;
        private System.Windows.Forms.Label keyEventLabel;
        private System.Windows.Forms.Label keyCntLabel;
    }
}

