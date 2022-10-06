namespace PGDBManagement
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TitlePnl = new System.Windows.Forms.Panel();
            this.TitlePanelTitle = new System.Windows.Forms.Label();
            this.MinimizePicb = new System.Windows.Forms.PictureBox();
            this.MaximizeNormalPicb = new System.Windows.Forms.PictureBox();
            this.ClosePicb = new System.Windows.Forms.PictureBox();
            this.TitleIconPicb = new System.Windows.Forms.PictureBox();
            this.installPSQLBtn = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.initPSQLDBBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.progressBar = new PGDBManagement.CMControls.ProgressBarExt();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userTxt = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.TitlePnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimizePicb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximizeNormalPicb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePicb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleIconPicb)).BeginInit();
            this.SuspendLayout();
            // 
            // TitlePnl
            // 
            this.TitlePnl.BackColor = System.Drawing.Color.Transparent;
            this.TitlePnl.Controls.Add(this.TitlePanelTitle);
            this.TitlePnl.Controls.Add(this.MinimizePicb);
            this.TitlePnl.Controls.Add(this.MaximizeNormalPicb);
            this.TitlePnl.Controls.Add(this.ClosePicb);
            this.TitlePnl.Controls.Add(this.TitleIconPicb);
            this.TitlePnl.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePnl.Location = new System.Drawing.Point(0, 0);
            this.TitlePnl.Margin = new System.Windows.Forms.Padding(0);
            this.TitlePnl.Name = "TitlePnl";
            this.TitlePnl.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.TitlePnl.Size = new System.Drawing.Size(800, 30);
            this.TitlePnl.TabIndex = 2;
            // 
            // TitlePanelTitle
            // 
            this.TitlePanelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TitlePanelTitle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.TitlePanelTitle.Location = new System.Drawing.Point(35, 2);
            this.TitlePanelTitle.Name = "TitlePanelTitle";
            this.TitlePanelTitle.Size = new System.Drawing.Size(636, 26);
            this.TitlePanelTitle.TabIndex = 4;
            this.TitlePanelTitle.Text = "PostgreSQL 一键安装、初始化";
            this.TitlePanelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinimizePicb
            // 
            this.MinimizePicb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimizePicb.Image = ((System.Drawing.Image)(resources.GetObject("MinimizePicb.Image")));
            this.MinimizePicb.Location = new System.Drawing.Point(677, 0);
            this.MinimizePicb.Margin = new System.Windows.Forms.Padding(0);
            this.MinimizePicb.Name = "MinimizePicb";
            this.MinimizePicb.Size = new System.Drawing.Size(40, 30);
            this.MinimizePicb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MinimizePicb.TabIndex = 2;
            this.MinimizePicb.TabStop = false;
            // 
            // MaximizeNormalPicb
            // 
            this.MaximizeNormalPicb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximizeNormalPicb.Enabled = false;
            this.MaximizeNormalPicb.Image = ((System.Drawing.Image)(resources.GetObject("MaximizeNormalPicb.Image")));
            this.MaximizeNormalPicb.Location = new System.Drawing.Point(718, 0);
            this.MaximizeNormalPicb.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeNormalPicb.Name = "MaximizeNormalPicb";
            this.MaximizeNormalPicb.Size = new System.Drawing.Size(40, 30);
            this.MaximizeNormalPicb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MaximizeNormalPicb.TabIndex = 1;
            this.MaximizeNormalPicb.TabStop = false;
            // 
            // ClosePicb
            // 
            this.ClosePicb.Dock = System.Windows.Forms.DockStyle.Right;
            this.ClosePicb.Image = ((System.Drawing.Image)(resources.GetObject("ClosePicb.Image")));
            this.ClosePicb.Location = new System.Drawing.Point(759, 0);
            this.ClosePicb.Margin = new System.Windows.Forms.Padding(0);
            this.ClosePicb.Name = "ClosePicb";
            this.ClosePicb.Size = new System.Drawing.Size(40, 30);
            this.ClosePicb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ClosePicb.TabIndex = 0;
            this.ClosePicb.TabStop = false;
            // 
            // TitleIconPicb
            // 
            this.TitleIconPicb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TitleIconPicb.Location = new System.Drawing.Point(6, 2);
            this.TitleIconPicb.Margin = new System.Windows.Forms.Padding(0);
            this.TitleIconPicb.MaximumSize = new System.Drawing.Size(30, 30);
            this.TitleIconPicb.Name = "TitleIconPicb";
            this.TitleIconPicb.Size = new System.Drawing.Size(26, 26);
            this.TitleIconPicb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.TitleIconPicb.TabIndex = 3;
            this.TitleIconPicb.TabStop = false;
            // 
            // installPSQLBtn
            // 
            this.installPSQLBtn.BackColor = System.Drawing.Color.Coral;
            this.installPSQLBtn.FlatAppearance.BorderSize = 0;
            this.installPSQLBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.installPSQLBtn.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.installPSQLBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.installPSQLBtn.Location = new System.Drawing.Point(111, 150);
            this.installPSQLBtn.Name = "installPSQLBtn";
            this.installPSQLBtn.Size = new System.Drawing.Size(159, 41);
            this.installPSQLBtn.TabIndex = 3;
            this.installPSQLBtn.Text = "一键安装PostgreSQL";
            this.installPSQLBtn.UseVisualStyleBackColor = false;
            this.installPSQLBtn.Click += new System.EventHandler(this.installPSQLBtn_ClickAsync);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 30);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 420);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // initPSQLDBBtn
            // 
            this.initPSQLDBBtn.BackColor = System.Drawing.Color.Coral;
            this.initPSQLDBBtn.FlatAppearance.BorderSize = 0;
            this.initPSQLDBBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.initPSQLDBBtn.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.initPSQLDBBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.initPSQLDBBtn.Location = new System.Drawing.Point(489, 150);
            this.initPSQLDBBtn.Name = "initPSQLDBBtn";
            this.initPSQLDBBtn.Size = new System.Drawing.Size(159, 41);
            this.initPSQLDBBtn.TabIndex = 6;
            this.initPSQLDBBtn.Text = "初始化数据库";
            this.initPSQLDBBtn.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(67, 278);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "重新开始进度";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(167, 278);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "设置显示Model";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(298, 278);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "设置颜色";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(395, 278);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(229, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "测试PerformStep()和Progress文本显示";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(41, 415);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(138, 23);
            this.button5.TabIndex = 18;
            this.button5.Text = "打开测试长路径";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // progressBar
            // 
            this.progressBar.DisplayMode = PGDBManagement.CMControls.ProgressBarDisplayMode.Progress;
            this.progressBar.FixedText = true;
            this.progressBar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.progressBar.Location = new System.Drawing.Point(43, 351);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(674, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 20;
            this.progressBar.Text = "progressBarExt1";
            this.progressBar.TextColor = System.Drawing.SystemColors.ControlLightLight;
            this.progressBar.TextPositionModel = PGDBManagement.CMControls.ProgressBarTextPositionModel.Above;
            this.progressBar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(80, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(95, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "密码：";
            // 
            // userTxt
            // 
            this.userTxt.Font = new System.Drawing.Font("宋体", 10.5F);
            this.userTxt.Location = new System.Drawing.Point(141, 84);
            this.userTxt.Name = "userTxt";
            this.userTxt.Size = new System.Drawing.Size(140, 23);
            this.userTxt.TabIndex = 23;
            this.userTxt.Text = "myuser";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 10.5F);
            this.textBox2.Location = new System.Drawing.Point(141, 115);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(139, 23);
            this.textBox2.TabIndex = 24;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.userTxt);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.initPSQLDBBtn);
            this.Controls.Add(this.installPSQLBtn);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.TitlePnl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.TitlePnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinimizePicb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximizeNormalPicb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePicb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleIconPicb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel TitlePnl;
        private System.Windows.Forms.Label TitlePanelTitle;
        private System.Windows.Forms.PictureBox MinimizePicb;
        private System.Windows.Forms.PictureBox MaximizeNormalPicb;
        private System.Windows.Forms.PictureBox ClosePicb;
        private System.Windows.Forms.PictureBox TitleIconPicb;
        private System.Windows.Forms.Button installPSQLBtn;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button initPSQLDBBtn;
       
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    
        private System.Windows.Forms.Button button5;
        private CMControls.ProgressBarExt progressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox userTxt;
        private System.Windows.Forms.TextBox textBox2;
    }
}