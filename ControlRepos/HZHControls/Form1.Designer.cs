namespace HZHControls
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
            this.ucBtnExt1 = new HZH_Controls.Controls.UCBtnExt();
            this.ucBtnFillet1 = new HZH_Controls.Controls.UCBtnFillet();
            this.ucBtnImg1 = new HZH_Controls.Controls.UCBtnImg();
            this.ucDropDownBtn1 = new HZH_Controls.Controls.UCDropDownBtn();
            this.SuspendLayout();
            // 
            // ucBtnExt1
            // 
            this.ucBtnExt1.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnExt1.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnExt1.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnText = "按钮测试";
            this.ucBtnExt1.ConerRadius = 20;
            this.ucBtnExt1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExt1.EnabledMouseEffect = false;
            this.ucBtnExt1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucBtnExt1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnExt1.IsRadius = true;
            this.ucBtnExt1.IsShowRect = true;
            this.ucBtnExt1.IsShowTips = false;
            this.ucBtnExt1.Location = new System.Drawing.Point(71, 47);
            this.ucBtnExt1.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnExt1.Name = "ucBtnExt1";
            this.ucBtnExt1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ucBtnExt1.RectWidth = 1;
            this.ucBtnExt1.Size = new System.Drawing.Size(184, 60);
            this.ucBtnExt1.TabIndex = 0;
            this.ucBtnExt1.TabStop = false;
            this.ucBtnExt1.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnExt1.TipsText = "";
            this.ucBtnExt1.BtnClick += new System.EventHandler(this.ucBtnExt1_BtnClick);
            // 
            // ucBtnFillet1
            // 
            this.ucBtnFillet1.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnFillet1.BtnImage = ((System.Drawing.Image)(resources.GetObject("ucBtnFillet1.BtnImage")));
            this.ucBtnFillet1.BtnText = "按钮1   ";
            this.ucBtnFillet1.ConerRadius = 5;
            this.ucBtnFillet1.FillColor = System.Drawing.Color.Transparent;
            this.ucBtnFillet1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnFillet1.IsRadius = true;
            this.ucBtnFillet1.IsShowRect = true;
            this.ucBtnFillet1.Location = new System.Drawing.Point(71, 205);
            this.ucBtnFillet1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucBtnFillet1.Name = "ucBtnFillet1";
            this.ucBtnFillet1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucBtnFillet1.RectWidth = 1;
            this.ucBtnFillet1.Size = new System.Drawing.Size(149, 65);
            this.ucBtnFillet1.TabIndex = 1;
            // 
            // ucBtnImg1
            // 
            this.ucBtnImg1.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnImg1.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnImg1.BtnFont = new System.Drawing.Font("微软雅黑", 17F);
            this.ucBtnImg1.BtnForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ucBtnImg1.BtnText = "自定义按钮";
            this.ucBtnImg1.ConerRadius = 5;
            this.ucBtnImg1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnImg1.EnabledMouseEffect = false;
            this.ucBtnImg1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucBtnImg1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnImg1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.ucBtnImg1.Image = ((System.Drawing.Image)(resources.GetObject("ucBtnImg1.Image")));
            this.ucBtnImg1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ucBtnImg1.ImageFontIcons = null;
            this.ucBtnImg1.IsRadius = true;
            this.ucBtnImg1.IsShowRect = true;
            this.ucBtnImg1.IsShowTips = false;
            this.ucBtnImg1.Location = new System.Drawing.Point(58, 131);
            this.ucBtnImg1.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnImg1.Name = "ucBtnImg1";
            this.ucBtnImg1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ucBtnImg1.RectWidth = 1;
            this.ucBtnImg1.Size = new System.Drawing.Size(184, 60);
            this.ucBtnImg1.TabIndex = 2;
            this.ucBtnImg1.TabStop = false;
            this.ucBtnImg1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ucBtnImg1.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnImg1.TipsText = "";
            // 
            // ucDropDownBtn1
            // 
            this.ucDropDownBtn1.BackColor = System.Drawing.Color.Transparent;
            this.ucDropDownBtn1.BtnBackColor = System.Drawing.Color.White;
            this.ucDropDownBtn1.BtnFont = new System.Drawing.Font("微软雅黑", 14F);
            this.ucDropDownBtn1.BtnForeColor = System.Drawing.Color.White;
            this.ucDropDownBtn1.Btns = null;
            this.ucDropDownBtn1.BtnText = "自定义按钮";
            this.ucDropDownBtn1.ConerRadius = 35;
            this.ucDropDownBtn1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucDropDownBtn1.DropPanelHeight = -1;
            this.ucDropDownBtn1.EnabledMouseEffect = false;
            this.ucDropDownBtn1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucDropDownBtn1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucDropDownBtn1.ForeColor = System.Drawing.Color.White;
            this.ucDropDownBtn1.Image = ((System.Drawing.Image)(resources.GetObject("ucDropDownBtn1.Image")));
            this.ucDropDownBtn1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ucDropDownBtn1.ImageFontIcons = null;
            this.ucDropDownBtn1.IsRadius = true;
            this.ucDropDownBtn1.IsShowRect = true;
            this.ucDropDownBtn1.IsShowTips = false;
            this.ucDropDownBtn1.Location = new System.Drawing.Point(58, 294);
            this.ucDropDownBtn1.Margin = new System.Windows.Forms.Padding(2);
            this.ucDropDownBtn1.Name = "ucDropDownBtn1";
            this.ucDropDownBtn1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ucDropDownBtn1.RectWidth = 1;
            this.ucDropDownBtn1.Size = new System.Drawing.Size(184, 60);
            this.ucDropDownBtn1.TabIndex = 3;
            this.ucDropDownBtn1.TabStop = false;
            this.ucDropDownBtn1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ucDropDownBtn1.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucDropDownBtn1.TipsText = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ucDropDownBtn1);
            this.Controls.Add(this.ucBtnImg1);
            this.Controls.Add(this.ucBtnFillet1);
            this.Controls.Add(this.ucBtnExt1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private HZH_Controls.Controls.UCBtnExt ucBtnExt1;
        private HZH_Controls.Controls.UCBtnFillet ucBtnFillet1;
        private HZH_Controls.Controls.UCBtnImg ucBtnImg1;
        private HZH_Controls.Controls.UCDropDownBtn ucDropDownBtn1;
    }
}

