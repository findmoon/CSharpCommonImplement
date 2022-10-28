namespace FilesWatcher.AutoCopyNewFile
{
    partial class AutoCopy
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
            this.overWriteChk = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.targetBtn = new System.Windows.Forms.Button();
            this.sourceBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.targetPathTxt = new System.Windows.Forms.TextBox();
            this.sourcePathTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // overWriteChk
            // 
            this.overWriteChk.AutoSize = true;
            this.overWriteChk.Location = new System.Drawing.Point(35, 120);
            this.overWriteChk.Name = "overWriteChk";
            this.overWriteChk.Size = new System.Drawing.Size(192, 16);
            this.overWriteChk.TabIndex = 16;
            this.overWriteChk.Text = "目标路径中存在同名文件则覆盖";
            this.overWriteChk.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(138, 153);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(285, 60);
            this.button1.TabIndex = 15;
            this.button1.Text = "开始自动复制";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // targetBtn
            // 
            this.targetBtn.Image = global::FilesWatcher.Properties.Resources.open;
            this.targetBtn.Location = new System.Drawing.Point(447, 77);
            this.targetBtn.Name = "targetBtn";
            this.targetBtn.Size = new System.Drawing.Size(41, 23);
            this.targetBtn.TabIndex = 14;
            this.targetBtn.UseVisualStyleBackColor = true;
            this.targetBtn.Click += new System.EventHandler(this.targetBtn_Click);
            // 
            // sourceBtn
            // 
            this.sourceBtn.Image = global::FilesWatcher.Properties.Resources.open;
            this.sourceBtn.Location = new System.Drawing.Point(447, 26);
            this.sourceBtn.Name = "sourceBtn";
            this.sourceBtn.Size = new System.Drawing.Size(41, 23);
            this.sourceBtn.TabIndex = 13;
            this.sourceBtn.UseVisualStyleBackColor = true;
            this.sourceBtn.Click += new System.EventHandler(this.sourceBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "目标路径";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "源路径";
            // 
            // targetPathTxt
            // 
            this.targetPathTxt.Location = new System.Drawing.Point(92, 79);
            this.targetPathTxt.Name = "targetPathTxt";
            this.targetPathTxt.Size = new System.Drawing.Size(333, 21);
            this.targetPathTxt.TabIndex = 10;
            // 
            // sourcePathTxt
            // 
            this.sourcePathTxt.Location = new System.Drawing.Point(92, 28);
            this.sourcePathTxt.Name = "sourcePathTxt";
            this.sourcePathTxt.Size = new System.Drawing.Size(333, 21);
            this.sourcePathTxt.TabIndex = 9;
            // 
            // AutoCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 251);
            this.Controls.Add(this.overWriteChk);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.targetBtn);
            this.Controls.Add(this.sourceBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.targetPathTxt);
            this.Controls.Add(this.sourcePathTxt);
            this.Name = "AutoCopy";
            this.Text = "AutoCopy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox overWriteChk;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button targetBtn;
        private System.Windows.Forms.Button sourceBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox targetPathTxt;
        private System.Windows.Forms.TextBox sourcePathTxt;
    }
}