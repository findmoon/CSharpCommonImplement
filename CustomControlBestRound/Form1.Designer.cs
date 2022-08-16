namespace CustomControlBestRound
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonPro21 = new CMControls.ButtonPro2();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.DarkOrange;
            this.button1.Location = new System.Drawing.Point(9, 182);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(184, 81);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(328, 206);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 132);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Coral;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(34, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(554, 51);
            this.label1.TabIndex = 2;
            this.label1.Text = "没有最佳圆角最佳自定义控件的实现，除非可以创建无锯齿Region";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonPro21
            // 
            this.buttonPro21.FlatAppearance.BorderSize = 0;
            this.buttonPro21.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro21.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro21.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro21.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro21.ForeColor = System.Drawing.Color.White;
            this.buttonPro21.Location = new System.Drawing.Point(605, 38);
            this.buttonPro21.Name = "buttonPro21";
            this.buttonPro21.RegionNewModel = true;
            this.buttonPro21.Size = new System.Drawing.Size(592, 351);
            this.buttonPro21.TabIndex = 3;
            this.buttonPro21.Text = "buttonPro21";
            this.buttonPro21.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 450);
            this.Controls.Add(this.buttonPro21);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private CMControls.ButtonPro2 buttonPro21;
    }
}

