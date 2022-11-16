namespace CustomControlRound
{
    partial class CustomPanelTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.customPanel1 = new CMControls.CustomPanelNo();
            this.roundPanel2 = new CMControls.RoundPanel();
            this.roundPanel1 = new CMControls.RoundPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.roundPanel3 = new CMControls.RoundPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(76, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 67);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.Transparent;
            this.customPanel1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.customPanel1.BorderFocusColor = System.Drawing.Color.HotPink;
            this.customPanel1.BorderRadius = 20;
            this.customPanel1.BorderSize = 6;
            this.customPanel1.Location = new System.Drawing.Point(12, 213);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(536, 261);
            this.customPanel1.TabIndex = 6;
            this.customPanel1.UnderlinedStyle = false;
            // 
            // roundPanel2
            // 
            this.roundPanel2.BackColor = System.Drawing.Color.Transparent;
            this.roundPanel2.Location = new System.Drawing.Point(375, 7);
            this.roundPanel2.Name = "roundPanel2";
            this.roundPanel2.RoundBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.roundPanel2.RoundNormalColor = System.Drawing.Color.LightCoral;
            this.roundPanel2.Size = new System.Drawing.Size(269, 138);
            this.roundPanel2.TabIndex = 5;
            // 
            // roundPanel1
            // 
            this.roundPanel1.BackColor = System.Drawing.Color.Transparent;
            this.roundPanel1.Location = new System.Drawing.Point(554, 213);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.RoundBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.roundPanel1.RoundBorderSize = 6;
            this.roundPanel1.RoundNormalColor = System.Drawing.Color.Transparent;
            this.roundPanel1.Size = new System.Drawing.Size(536, 261);
            this.roundPanel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "似乎边框拐角更流畅些";
            // 
            // roundPanel3
            // 
            this.roundPanel3.BackColor = System.Drawing.Color.Transparent;
            this.roundPanel3.Location = new System.Drawing.Point(707, 53);
            this.roundPanel3.Name = "roundPanel3";
            this.roundPanel3.RoundBorderColor = System.Drawing.Color.DarkViolet;
            this.roundPanel3.RoundBorderSize = 6;
            this.roundPanel3.RoundNormalColor = System.Drawing.Color.Transparent;
            this.roundPanel3.Size = new System.Drawing.Size(253, 125);
            this.roundPanel3.TabIndex = 6;
            this.roundPanel3.UnderlinedStyle = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Location = new System.Drawing.Point(1139, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 415);
            this.panel1.TabIndex = 8;
            // 
            // CustomPanelTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1468, 536);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.roundPanel3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.customPanel1);
            this.Controls.Add(this.roundPanel2);
            this.Controls.Add(this.roundPanel1);
            this.Controls.Add(this.button1);
            this.Name = "CustomPanelTest";
            this.Text = "CustomPanelTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private CMControls.RoundPanel roundPanel1;
        private CMControls.RoundPanel roundPanel2;
        private CMControls.CustomPanelNo customPanel1;
        private System.Windows.Forms.Label label1;
        private CMControls.RoundPanel roundPanel3;
        private System.Windows.Forms.Panel panel1;
    }
}