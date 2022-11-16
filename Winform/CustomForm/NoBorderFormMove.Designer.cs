namespace CustomForm
{
    partial class NoBorderFormMove
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.originLocationLbl = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.roundPanel1 = new CMControls.RoundPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F);
            this.label1.Location = new System.Drawing.Point(170, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(409, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "方法0：Win32 API ReleaseCapture 和 SendMessage 实现";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 384);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "移动时原来的鼠标位置：";
            // 
            // originLocationLbl
            // 
            this.originLocationLbl.AutoSize = true;
            this.originLocationLbl.Location = new System.Drawing.Point(210, 384);
            this.originLocationLbl.Name = "originLocationLbl";
            this.originLocationLbl.Size = new System.Drawing.Size(0, 12);
            this.originLocationLbl.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.roundPanel1);
            this.panel1.Location = new System.Drawing.Point(441, 280);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 156);
            this.panel1.TabIndex = 4;
            // 
            // roundPanel1
            // 
            this.roundPanel1.BackColor = System.Drawing.Color.Transparent;
            this.roundPanel1.Location = new System.Drawing.Point(61, 27);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.RoundBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.roundPanel1.RoundNormalColor = System.Drawing.SystemColors.Highlight;
            this.roundPanel1.Size = new System.Drawing.Size(200, 100);
            this.roundPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 11F);
            this.label3.Location = new System.Drawing.Point(170, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(409, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "方法1： 鼠标按下、移动和抬起事件中，Left、Top直接变化";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 11F);
            this.label4.Location = new System.Drawing.Point(170, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(424, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "方法2：鼠标按下、移动和抬起事件中，计算移动后的Location";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 11F);
            this.label5.Location = new System.Drawing.Point(101, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(622, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "方法3 计算鼠标相对于(窗体)左上角的位置，借助 Offset 鼠标屏幕坐标点平移(相对位置) ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 11F);
            this.label6.Location = new System.Drawing.Point(623, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "方法1、2、3的简化方法";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 11F);
            this.label7.Location = new System.Drawing.Point(192, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(339, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "方法4，2、3、4原理一致，只是使用的方法不一样";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 11F);
            this.label8.Location = new System.Drawing.Point(192, 240);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(344, 15);
            this.label8.TabIndex = 10;
            this.label8.Text = "方法5：Win32 API SendMessage 发送 0XA1 消息";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("隶书", 18F);
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(301, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(238, 24);
            this.label9.TabIndex = 11;
            this.label9.Text = "6种方法实现窗体移动";
            // 
            // NoBorderFormMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.originLocationLbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NoBorderFormMove";
            this.Text = "NoBorderFormUseEventDrag";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label originLocationLbl;
        private System.Windows.Forms.Panel panel1;
        private CMControls.RoundPanel roundPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}