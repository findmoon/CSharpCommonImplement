namespace CustomControlSplitButton
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.onwToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitButton1 = new CMControls.SplitButton();
            this.splitButton_BelowLeft = new CMControls.SplitButton();
            this.splitButton_Right = new CMControls.SplitButton();
            this.splitButton_Left = new CMControls.SplitButton();
            this.splitButton_AboveRight = new CMControls.SplitButton();
            this.splitButton_AboveLeft = new CMControls.SplitButton();
            this.splitButton2 = new CMControls.SplitButton();
            this.splitButtonDefalut = new CMControls.SplitButton();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onwToolStripMenuItem,
            this.twoToolStripMenuItem,
            this.threeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(110, 70);
            // 
            // onwToolStripMenuItem
            // 
            this.onwToolStripMenuItem.Name = "onwToolStripMenuItem";
            this.onwToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.onwToolStripMenuItem.Text = "One";
            // 
            // twoToolStripMenuItem
            // 
            this.twoToolStripMenuItem.Name = "twoToolStripMenuItem";
            this.twoToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.twoToolStripMenuItem.Text = "Two";
            // 
            // threeToolStripMenuItem
            // 
            this.threeToolStripMenuItem.Name = "threeToolStripMenuItem";
            this.threeToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.threeToolStripMenuItem.Text = "Three";
            // 
            // splitButton1
            // 
            this.splitButton1.AutoSize = true;
            this.splitButton1.ContextMenuStrip = this.contextMenuStrip1;
            this.splitButton1.Location = new System.Drawing.Point(64, 65);
            this.splitButton1.Name = "splitButton1";
            this.splitButton1.Size = new System.Drawing.Size(177, 46);
            this.splitButton1.SplitMenuStrip = this.contextMenuStrip1;
            this.splitButton1.SplitMenuStripAlign = System.Windows.Forms.ToolStripDropDownDirection.BelowRight;
            this.splitButton1.TabIndex = 8;
            this.splitButton1.Text = "splitButton下拉等宽";
            this.splitButton1.UseVisualStyleBackColor = true;
            // 
            // splitButton_BelowLeft
            // 
            this.splitButton_BelowLeft.AutoSize = true;
            this.splitButton_BelowLeft.ContextMenuStrip = this.contextMenuStrip1;
            this.splitButton_BelowLeft.Location = new System.Drawing.Point(267, 186);
            this.splitButton_BelowLeft.Name = "splitButton_BelowLeft";
            this.splitButton_BelowLeft.Size = new System.Drawing.Size(189, 46);
            this.splitButton_BelowLeft.SplitMenuStrip = this.contextMenuStrip1;
            this.splitButton_BelowLeft.SplitMenuStripAlign = System.Windows.Forms.ToolStripDropDownDirection.BelowRight;
            this.splitButton_BelowLeft.TabIndex = 7;
            this.splitButton_BelowLeft.Text = "splitButton_Drop_BelowLeft";
            this.splitButton_BelowLeft.UseVisualStyleBackColor = true;
            // 
            // splitButton_Right
            // 
            this.splitButton_Right.AutoSize = true;
            this.splitButton_Right.ContextMenuStrip = this.contextMenuStrip1;
            this.splitButton_Right.Location = new System.Drawing.Point(52, 252);
            this.splitButton_Right.Name = "splitButton_Right";
            this.splitButton_Right.Size = new System.Drawing.Size(165, 46);
            this.splitButton_Right.SplitMenuStrip = this.contextMenuStrip1;
            this.splitButton_Right.SplitMenuStripAlign = System.Windows.Forms.ToolStripDropDownDirection.BelowRight;
            this.splitButton_Right.TabIndex = 6;
            this.splitButton_Right.Text = "splitButton_Drop_Right";
            this.splitButton_Right.UseVisualStyleBackColor = true;
            // 
            // splitButton_Left
            // 
            this.splitButton_Left.AutoSize = true;
            this.splitButton_Left.ContextMenuStrip = this.contextMenuStrip1;
            this.splitButton_Left.Location = new System.Drawing.Point(278, 252);
            this.splitButton_Left.Name = "splitButton_Left";
            this.splitButton_Left.Size = new System.Drawing.Size(159, 46);
            this.splitButton_Left.SplitMenuStrip = this.contextMenuStrip1;
            this.splitButton_Left.SplitMenuStripAlign = System.Windows.Forms.ToolStripDropDownDirection.BelowRight;
            this.splitButton_Left.TabIndex = 5;
            this.splitButton_Left.Text = "splitButton_Drop_Left";
            this.splitButton_Left.UseVisualStyleBackColor = true;
            // 
            // splitButton_AboveRight
            // 
            this.splitButton_AboveRight.AutoSize = true;
            this.splitButton_AboveRight.ContextMenuStrip = this.contextMenuStrip1;
            this.splitButton_AboveRight.Location = new System.Drawing.Point(52, 385);
            this.splitButton_AboveRight.Name = "splitButton_AboveRight";
            this.splitButton_AboveRight.Size = new System.Drawing.Size(195, 46);
            this.splitButton_AboveRight.SplitMenuStrip = this.contextMenuStrip1;
            this.splitButton_AboveRight.SplitMenuStripAlign = System.Windows.Forms.ToolStripDropDownDirection.BelowRight;
            this.splitButton_AboveRight.TabIndex = 4;
            this.splitButton_AboveRight.Text = "splitButton_Drop_AboveRight";
            this.splitButton_AboveRight.UseVisualStyleBackColor = true;
            // 
            // splitButton_AboveLeft
            // 
            this.splitButton_AboveLeft.AutoSize = true;
            this.splitButton_AboveLeft.ContextMenuStrip = this.contextMenuStrip1;
            this.splitButton_AboveLeft.Location = new System.Drawing.Point(52, 322);
            this.splitButton_AboveLeft.Name = "splitButton_AboveLeft";
            this.splitButton_AboveLeft.Size = new System.Drawing.Size(189, 46);
            this.splitButton_AboveLeft.SplitMenuStrip = this.contextMenuStrip1;
            this.splitButton_AboveLeft.SplitMenuStripAlign = System.Windows.Forms.ToolStripDropDownDirection.BelowRight;
            this.splitButton_AboveLeft.TabIndex = 3;
            this.splitButton_AboveLeft.Text = "splitButton_Drop_AboveLeft";
            this.splitButton_AboveLeft.UseVisualStyleBackColor = true;
            // 
            // splitButton2
            // 
            this.splitButton2.AutoSize = true;
            this.splitButton2.Location = new System.Drawing.Point(584, 84);
            this.splitButton2.Name = "splitButton2";
            this.splitButton2.Size = new System.Drawing.Size(157, 46);
            this.splitButton2.SplitMenuStripAlign = System.Windows.Forms.ToolStripDropDownDirection.BelowRight;
            this.splitButton2.TabIndex = 2;
            this.splitButton2.Text = "splitButton2";
            this.splitButton2.UseVisualStyleBackColor = true;
            // 
            // splitButtonDefalut
            // 
            this.splitButtonDefalut.AutoSize = true;
            this.splitButtonDefalut.ContextMenuStrip = this.contextMenuStrip1;
            this.splitButtonDefalut.Location = new System.Drawing.Point(52, 186);
            this.splitButtonDefalut.Name = "splitButtonDefalut";
            this.splitButtonDefalut.Size = new System.Drawing.Size(177, 46);
            this.splitButtonDefalut.SplitMenuStrip = this.contextMenuStrip1;
            this.splitButtonDefalut.SplitMenuStripAlign = System.Windows.Forms.ToolStripDropDownDirection.BelowRight;
            this.splitButtonDefalut.TabIndex = 0;
            this.splitButtonDefalut.Text = "splitButton_Drop_defalut";
            this.splitButtonDefalut.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitButton1);
            this.Controls.Add(this.splitButton_BelowLeft);
            this.Controls.Add(this.splitButton_Right);
            this.Controls.Add(this.splitButton_Left);
            this.Controls.Add(this.splitButton_AboveRight);
            this.Controls.Add(this.splitButton_AboveLeft);
            this.Controls.Add(this.splitButton2);
            this.Controls.Add(this.splitButtonDefalut);
            this.Name = "Form1";
            this.Text = "Form1";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CMControls.SplitButton splitButtonDefalut;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem onwToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem threeToolStripMenuItem;
        private CMControls.SplitButton splitButton2;
        private CMControls.SplitButton splitButton_AboveLeft;
        private CMControls.SplitButton splitButton_AboveRight;
        private CMControls.SplitButton splitButton_Left;
        private CMControls.SplitButton splitButton_Right;
        private CMControls.SplitButton splitButton_BelowLeft;
        private CMControls.SplitButton splitButton1;
    }
}

