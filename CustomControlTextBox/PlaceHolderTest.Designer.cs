namespace CustomControlTextBox
{
    partial class PlaceHolderTest
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
            this.simplePlaceHolderTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxPlaceholder3 = new CMControls.TextBoxPlaceholder();
            this.textBoxPlaceholder1 = new CMControls.TextBoxPlaceholder();
            this.SuspendLayout();
            // 
            // simplePlaceHolderTxt
            // 
            this.simplePlaceHolderTxt.Location = new System.Drawing.Point(14, 24);
            this.simplePlaceHolderTxt.Name = "simplePlaceHolderTxt";
            this.simplePlaceHolderTxt.Size = new System.Drawing.Size(218, 21);
            this.simplePlaceHolderTxt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "简单placeholder";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(332, 215);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(137, 21);
            this.textBox1.TabIndex = 0;
            // 
            // textBoxPlaceholder3
            // 
            this.textBoxPlaceholder3.Font = new System.Drawing.Font("宋体", 12F);
            this.textBoxPlaceholder3.Location = new System.Drawing.Point(409, 99);
            this.textBoxPlaceholder3.Name = "textBoxPlaceholder3";
            this.textBoxPlaceholder3.PlaceHolderColor = System.Drawing.Color.Wheat;
            this.textBoxPlaceholder3.PlaceHolderText = "密码";
            this.textBoxPlaceholder3.Size = new System.Drawing.Size(152, 26);
            this.textBoxPlaceholder3.TabIndex = 5;
            // 
            // textBoxPlaceholder1
            // 
            this.textBoxPlaceholder1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.textBoxPlaceholder1.Font = new System.Drawing.Font("宋体", 12F);
            this.textBoxPlaceholder1.Location = new System.Drawing.Point(409, 56);
            this.textBoxPlaceholder1.Name = "textBoxPlaceholder1";
            this.textBoxPlaceholder1.PlaceHolderColor = System.Drawing.Color.LightGray;
            this.textBoxPlaceholder1.PlaceHolderText = "用户名";
            this.textBoxPlaceholder1.Size = new System.Drawing.Size(152, 26);
            this.textBoxPlaceholder1.TabIndex = 3;
            // 
            // PlaceHolderTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxPlaceholder3);
            this.Controls.Add(this.textBoxPlaceholder1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.simplePlaceHolderTxt);
            this.Name = "PlaceHolderTest";
            this.Text = "PlaceHolderTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox simplePlaceHolderTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private CMControls.TextBoxPlaceholder textBoxPlaceholder1;
        private CMControls.TextBoxPlaceholder textBoxPlaceholder3;
    }
}