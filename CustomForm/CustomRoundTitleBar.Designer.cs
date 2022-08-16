namespace CustomForm
{
    partial class CustomRoundTitleBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomRoundTitleBar));
            this.TitlePnl = new CMControls.RoundPanel();
            this.TitlePanelTitle = new System.Windows.Forms.Label();
            this.MinimizePicb = new System.Windows.Forms.PictureBox();
            this.MaximizeNormalPicb = new System.Windows.Forms.PictureBox();
            this.ClosePicb = new System.Windows.Forms.PictureBox();
            this.TitleIconPicb = new System.Windows.Forms.PictureBox();
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
            this.TitlePnl.Name = "TitlePnl";
            this.TitlePnl.Padding = new System.Windows.Forms.Padding(13, 0, 13, 0);
            this.TitlePnl.RoundBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.TitlePnl.RoundMode = ((CMControls.RoundMode)((CMControls.RoundMode.LeftTop | CMControls.RoundMode.RightTop)));
            this.TitlePnl.RoundNormalColor = System.Drawing.Color.LightSalmon;
            this.TitlePnl.Size = new System.Drawing.Size(1257, 30);
            this.TitlePnl.TabIndex = 15;
            // 
            // TitlePanelTitle
            // 
            this.TitlePanelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TitlePanelTitle.Location = new System.Drawing.Point(40, 2);
            this.TitlePanelTitle.Name = "TitlePanelTitle";
            this.TitlePanelTitle.Size = new System.Drawing.Size(1079, 26);
            this.TitlePanelTitle.TabIndex = 9;
            this.TitlePanelTitle.Text = "我是自定义标题栏";
            this.TitlePanelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinimizePicb
            // 
            this.MinimizePicb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimizePicb.Image = ((System.Drawing.Image)(resources.GetObject("MinimizePicb.Image")));
            this.MinimizePicb.Location = new System.Drawing.Point(1122, 0);
            this.MinimizePicb.Margin = new System.Windows.Forms.Padding(0);
            this.MinimizePicb.Name = "MinimizePicb";
            this.MinimizePicb.Size = new System.Drawing.Size(40, 30);
            this.MinimizePicb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MinimizePicb.TabIndex = 7;
            this.MinimizePicb.TabStop = false;
            // 
            // MaximizeNormalPicb
            // 
            this.MaximizeNormalPicb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximizeNormalPicb.Image = ((System.Drawing.Image)(resources.GetObject("MaximizeNormalPicb.Image")));
            this.MaximizeNormalPicb.Location = new System.Drawing.Point(1163, 0);
            this.MaximizeNormalPicb.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeNormalPicb.Name = "MaximizeNormalPicb";
            this.MaximizeNormalPicb.Size = new System.Drawing.Size(40, 30);
            this.MaximizeNormalPicb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MaximizeNormalPicb.TabIndex = 6;
            this.MaximizeNormalPicb.TabStop = false;
            // 
            // ClosePicb
            // 
            this.ClosePicb.Dock = System.Windows.Forms.DockStyle.Right;
            this.ClosePicb.Image = global::CustomForm.Properties.Resources.Close_16_16_White;
            this.ClosePicb.Location = new System.Drawing.Point(1204, 0);
            this.ClosePicb.Margin = new System.Windows.Forms.Padding(0);
            this.ClosePicb.Name = "ClosePicb";
            this.ClosePicb.Size = new System.Drawing.Size(40, 30);
            this.ClosePicb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ClosePicb.TabIndex = 5;
            this.ClosePicb.TabStop = false;
            // 
            // TitleIconPicb
            // 
            this.TitleIconPicb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TitleIconPicb.Image = global::CustomForm.Properties.Resources.DefaultImg_32_32;
            this.TitleIconPicb.Location = new System.Drawing.Point(11, 2);
            this.TitleIconPicb.Margin = new System.Windows.Forms.Padding(0);
            this.TitleIconPicb.MaximumSize = new System.Drawing.Size(30, 30);
            this.TitleIconPicb.Name = "TitleIconPicb";
            this.TitleIconPicb.Size = new System.Drawing.Size(26, 26);
            this.TitleIconPicb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.TitleIconPicb.TabIndex = 8;
            this.TitleIconPicb.TabStop = false;
            // 
            // CustomRoundTitleBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1257, 450);
            this.Controls.Add(this.TitlePnl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomRoundTitleBar";
            this.Text = "CustomRoundTitleBar";
            this.TitlePnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinimizePicb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximizeNormalPicb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePicb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleIconPicb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CMControls.RoundPanel TitlePnl;
        private System.Windows.Forms.Label TitlePanelTitle;
        private System.Windows.Forms.PictureBox MinimizePicb;
        private System.Windows.Forms.PictureBox MaximizeNormalPicb;
        private System.Windows.Forms.PictureBox ClosePicb;
        private System.Windows.Forms.PictureBox TitleIconPicb;
    }
}