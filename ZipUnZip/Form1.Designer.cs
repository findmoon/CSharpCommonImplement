namespace ZipUnZip
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
            this.ZIPFileGrp = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonPro5 = new CMControls.ButtonPro();
            this.buttonPro6 = new CMControls.ButtonPro();
            this.buttonPro3 = new CMControls.ButtonPro();
            this.buttonPro4 = new CMControls.ButtonPro();
            this.buttonPro1 = new CMControls.ButtonPro();
            this.buttonPro2 = new CMControls.ButtonPro();
            this.ZIPFileGrp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ZIPFileGrp
            // 
            this.ZIPFileGrp.Controls.Add(this.buttonPro1);
            this.ZIPFileGrp.Controls.Add(this.buttonPro2);
            this.ZIPFileGrp.Location = new System.Drawing.Point(1, 2);
            this.ZIPFileGrp.Name = "ZIPFileGrp";
            this.ZIPFileGrp.Size = new System.Drawing.Size(450, 100);
            this.ZIPFileGrp.TabIndex = 2;
            this.ZIPFileGrp.TabStop = false;
            this.ZIPFileGrp.Text = "ZIPFile";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonPro3);
            this.groupBox1.Controls.Add(this.buttonPro4);
            this.groupBox1.Location = new System.Drawing.Point(1, 122);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GZipStream";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonPro5);
            this.groupBox2.Controls.Add(this.buttonPro6);
            this.groupBox2.Location = new System.Drawing.Point(1, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 100);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DeflateStream";
            // 
            // buttonPro5
            // 
            this.buttonPro5.BackColor = System.Drawing.Color.Transparent;
            this.buttonPro5.FlatAppearance.BorderSize = 0;
            this.buttonPro5.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro5.ForeColor = System.Drawing.Color.White;
            this.buttonPro5.Location = new System.Drawing.Point(18, 28);
            this.buttonPro5.Name = "buttonPro5";
            this.buttonPro5.Size = new System.Drawing.Size(130, 57);
            this.buttonPro5.TabIndex = 0;
            this.buttonPro5.Text = "压缩解压字节数据";
            this.buttonPro5.UseVisualStyleBackColor = false;
            this.buttonPro5.Click += new System.EventHandler(this.buttonPro5_Click);
            // 
            // buttonPro6
            // 
            this.buttonPro6.BackColor = System.Drawing.Color.Transparent;
            this.buttonPro6.FlatAppearance.BorderSize = 0;
            this.buttonPro6.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro6.ForeColor = System.Drawing.Color.White;
            this.buttonPro6.Location = new System.Drawing.Point(170, 28);
            this.buttonPro6.Name = "buttonPro6";
            this.buttonPro6.Size = new System.Drawing.Size(140, 57);
            this.buttonPro6.TabIndex = 1;
            this.buttonPro6.Text = "解压缩文件";
            this.buttonPro6.UseVisualStyleBackColor = false;
            this.buttonPro6.Click += new System.EventHandler(this.buttonPro6_Click);
            // 
            // buttonPro3
            // 
            this.buttonPro3.BackColor = System.Drawing.Color.Transparent;
            this.buttonPro3.FlatAppearance.BorderSize = 0;
            this.buttonPro3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro3.ForeColor = System.Drawing.Color.White;
            this.buttonPro3.Location = new System.Drawing.Point(18, 28);
            this.buttonPro3.Name = "buttonPro3";
            this.buttonPro3.Size = new System.Drawing.Size(130, 57);
            this.buttonPro3.TabIndex = 0;
            this.buttonPro3.Text = "压缩解压字节数据";
            this.buttonPro3.UseVisualStyleBackColor = false;
            this.buttonPro3.Click += new System.EventHandler(this.buttonPro3_Click);
            // 
            // buttonPro4
            // 
            this.buttonPro4.BackColor = System.Drawing.Color.Transparent;
            this.buttonPro4.FlatAppearance.BorderSize = 0;
            this.buttonPro4.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro4.ForeColor = System.Drawing.Color.White;
            this.buttonPro4.Location = new System.Drawing.Point(170, 28);
            this.buttonPro4.Name = "buttonPro4";
            this.buttonPro4.Size = new System.Drawing.Size(140, 57);
            this.buttonPro4.TabIndex = 1;
            this.buttonPro4.Text = "解压缩文件";
            this.buttonPro4.UseVisualStyleBackColor = false;
            this.buttonPro4.Click += new System.EventHandler(this.buttonPro4_Click);
            // 
            // buttonPro1
            // 
            this.buttonPro1.BackColor = System.Drawing.Color.Transparent;
            this.buttonPro1.FlatAppearance.BorderSize = 0;
            this.buttonPro1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro1.ForeColor = System.Drawing.Color.White;
            this.buttonPro1.Location = new System.Drawing.Point(18, 28);
            this.buttonPro1.Name = "buttonPro1";
            this.buttonPro1.Size = new System.Drawing.Size(232, 57);
            this.buttonPro1.TabIndex = 0;
            this.buttonPro1.Text = "压缩解压缩当前目录下的test文件夹";
            this.buttonPro1.UseVisualStyleBackColor = false;
            this.buttonPro1.Click += new System.EventHandler(this.buttonPro1_Click);
            // 
            // buttonPro2
            // 
            this.buttonPro2.BackColor = System.Drawing.Color.Transparent;
            this.buttonPro2.FlatAppearance.BorderSize = 0;
            this.buttonPro2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPro2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPro2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPro2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPro2.ForeColor = System.Drawing.Color.White;
            this.buttonPro2.Location = new System.Drawing.Point(302, 28);
            this.buttonPro2.Name = "buttonPro2";
            this.buttonPro2.Size = new System.Drawing.Size(140, 57);
            this.buttonPro2.TabIndex = 1;
            this.buttonPro2.Text = "向ZIP中添加文件";
            this.buttonPro2.UseVisualStyleBackColor = false;
            this.buttonPro2.Click += new System.EventHandler(this.buttonPro2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ZIPFileGrp);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ZIPFileGrp.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CMControls.ButtonPro buttonPro1;
        private CMControls.ButtonPro buttonPro2;
        private System.Windows.Forms.GroupBox ZIPFileGrp;
        private System.Windows.Forms.GroupBox groupBox1;
        private CMControls.ButtonPro buttonPro3;
        private CMControls.ButtonPro buttonPro4;
        private System.Windows.Forms.GroupBox groupBox2;
        private CMControls.ButtonPro buttonPro5;
        private CMControls.ButtonPro buttonPro6;
    }
}

