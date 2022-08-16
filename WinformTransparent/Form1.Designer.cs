namespace WinformTransparent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.transparentTextBox1 = new WinformTransparent.Controls.TransparentTextBox();
            this.transparentLabel1 = new WinformTransparent.TransparentLabel();
            this.invisible_Button1 = new WinformTransparent.Invisible_Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.BackgroundImage = global::WinformTransparent.Properties.Resources.start;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(617, 333);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 123);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 326);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 49);
            this.button2.TabIndex = 5;
            this.button2.Text = "显示用代码设置透明背景的窗体";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(557, 78);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(133, 123);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "我是label标签";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 381);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 49);
            this.button3.TabIndex = 8;
            this.button3.Text = "打开加载图片的窗体";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "label";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 436);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(102, 49);
            this.button4.TabIndex = 10;
            this.button4.Text = "打开UseTranspCtrl窗体";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(127, 37);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(78, 42);
            this.button5.TabIndex = 11;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 210);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(102, 49);
            this.button6.TabIndex = 13;
            this.button6.Text = "显示UseExtendedPanel窗体";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(99, 289);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "label";
            // 
            // button7
            // 
            this.button7.Image = global::WinformTransparent.Properties.Resources.pause1;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button7.Location = new System.Drawing.Point(617, 235);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(92, 48);
            this.button7.TabIndex = 16;
            this.button7.Text = "button背景透明色";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // transparentTextBox1
            // 
            this.transparentTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.transparentTextBox1.Location = new System.Drawing.Point(72, 142);
            this.transparentTextBox1.Name = "transparentTextBox1";
            this.transparentTextBox1.Size = new System.Drawing.Size(121, 21);
            this.transparentTextBox1.TabIndex = 14;
            // 
            // transparentLabel1
            // 
            this.transparentLabel1.AutoSize = true;
            this.transparentLabel1.Image = ((System.Drawing.Image)(resources.GetObject("transparentLabel1.Image")));
            this.transparentLabel1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.transparentLabel1.Location = new System.Drawing.Point(75, 37);
            this.transparentLabel1.Name = "transparentLabel1";
            this.transparentLabel1.Size = new System.Drawing.Size(107, 12);
            this.transparentLabel1.TabIndex = 12;
            this.transparentLabel1.Text = "transparentLabel1";
            // 
            // invisible_Button1
            // 
            this.invisible_Button1.BackColor = System.Drawing.Color.Transparent;
            this.invisible_Button1.Location = new System.Drawing.Point(54, 169);
            this.invisible_Button1.Name = "invisible_Button1";
            this.invisible_Button1.Size = new System.Drawing.Size(189, 90);
            this.invisible_Button1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinformTransparent.Properties.Resources.Snipaste_20220712_091629;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(778, 487);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.transparentTextBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.transparentLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.invisible_Button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "背景透明测试";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private Invisible_Button invisible_Button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private TransparentLabel transparentLabel1;
        private System.Windows.Forms.Button button6;
        private Controls.TransparentTextBox transparentTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button7;
    }
}

