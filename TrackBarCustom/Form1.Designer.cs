namespace TrackBarCustom
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
            this.rColorTrackBar = new System.Windows.Forms.TrackBar();
            this.gColorTrackBar = new System.Windows.Forms.TrackBar();
            this.bColorTrackBar = new System.Windows.Forms.TrackBar();
            this.alphaColorTrackBar = new System.Windows.Forms.TrackBar();
            this.rColor = new System.Windows.Forms.Label();
            this.gColor = new System.Windows.Forms.Label();
            this.bColor = new System.Windows.Forms.Label();
            this.alphaColor = new System.Windows.Forms.Label();
            this.currColor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.colorTxt = new System.Windows.Forms.TextBox();
            this.CopyBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rColorTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gColorTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bColorTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaColorTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // rColorTrackBar
            // 
            this.rColorTrackBar.Location = new System.Drawing.Point(115, 148);
            this.rColorTrackBar.Name = "rColorTrackBar";
            this.rColorTrackBar.Size = new System.Drawing.Size(287, 45);
            this.rColorTrackBar.TabIndex = 0;
            // 
            // gColorTrackBar
            // 
            this.gColorTrackBar.Location = new System.Drawing.Point(115, 208);
            this.gColorTrackBar.Name = "gColorTrackBar";
            this.gColorTrackBar.Size = new System.Drawing.Size(287, 45);
            this.gColorTrackBar.TabIndex = 1;
            // 
            // bColorTrackBar
            // 
            this.bColorTrackBar.Location = new System.Drawing.Point(115, 268);
            this.bColorTrackBar.Name = "bColorTrackBar";
            this.bColorTrackBar.Size = new System.Drawing.Size(287, 45);
            this.bColorTrackBar.TabIndex = 2;
            // 
            // alphaColorTrackBar
            // 
            this.alphaColorTrackBar.Location = new System.Drawing.Point(115, 328);
            this.alphaColorTrackBar.Name = "alphaColorTrackBar";
            this.alphaColorTrackBar.Size = new System.Drawing.Size(287, 45);
            this.alphaColorTrackBar.TabIndex = 3;
            // 
            // rColor
            // 
            this.rColor.Location = new System.Drawing.Point(409, 147);
            this.rColor.Name = "rColor";
            this.rColor.Size = new System.Drawing.Size(30, 30);
            this.rColor.TabIndex = 4;
            // 
            // gColor
            // 
            this.gColor.Location = new System.Drawing.Point(409, 207);
            this.gColor.Name = "gColor";
            this.gColor.Size = new System.Drawing.Size(30, 30);
            this.gColor.TabIndex = 5;
            // 
            // bColor
            // 
            this.bColor.Location = new System.Drawing.Point(409, 267);
            this.bColor.Name = "bColor";
            this.bColor.Size = new System.Drawing.Size(30, 30);
            this.bColor.TabIndex = 6;
            // 
            // alphaColor
            // 
            this.alphaColor.Location = new System.Drawing.Point(409, 327);
            this.alphaColor.Name = "alphaColor";
            this.alphaColor.Size = new System.Drawing.Size(30, 30);
            this.alphaColor.TabIndex = 7;
            // 
            // currColor
            // 
            this.currColor.Location = new System.Drawing.Point(186, 23);
            this.currColor.Name = "currColor";
            this.currColor.Size = new System.Drawing.Size(183, 82);
            this.currColor.TabIndex = 8;
            this.currColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(95, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 28);
            this.label2.TabIndex = 9;
            this.label2.Text = "当前颜色：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // colorTxt
            // 
            this.colorTxt.Location = new System.Drawing.Point(62, 84);
            this.colorTxt.Name = "colorTxt";
            this.colorTxt.Size = new System.Drawing.Size(100, 21);
            this.colorTxt.TabIndex = 10;
            // 
            // CopyBtn
            // 
            this.CopyBtn.Location = new System.Drawing.Point(161, 85);
            this.CopyBtn.Name = "CopyBtn";
            this.CopyBtn.Size = new System.Drawing.Size(18, 18);
            this.CopyBtn.TabIndex = 13;
            this.CopyBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "R：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "G：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(97, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "B：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 333);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "Alpha：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 400);
            this.Controls.Add(this.rColorTrackBar);
            this.Controls.Add(this.gColorTrackBar);
            this.Controls.Add(this.bColorTrackBar);
            this.Controls.Add(this.alphaColorTrackBar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CopyBtn);
            this.Controls.Add(this.colorTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.currColor);
            this.Controls.Add(this.alphaColor);
            this.Controls.Add(this.bColor);
            this.Controls.Add(this.gColor);
            this.Controls.Add(this.rColor);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.rColorTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gColorTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bColorTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaColorTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar rColorTrackBar;
        private System.Windows.Forms.TrackBar gColorTrackBar;
        private System.Windows.Forms.TrackBar bColorTrackBar;
        private System.Windows.Forms.TrackBar alphaColorTrackBar;
        private System.Windows.Forms.Label rColor;
        private System.Windows.Forms.Label gColor;
        private System.Windows.Forms.Label bColor;
        private System.Windows.Forms.Label alphaColor;
        private System.Windows.Forms.Label currColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox colorTxt;
        private System.Windows.Forms.Button CopyBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

