namespace WinformTransparent
{
    partial class UseTranspCtrl
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
            this.transpCtrl1 = new TranspCtrl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(629, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // transpCtrl1
            // 
            this.transpCtrl1.BackColor = System.Drawing.Color.Blue;
            this.transpCtrl1.Location = new System.Drawing.Point(12, 63);
            this.transpCtrl1.Name = "transpCtrl1";
            this.transpCtrl1.Opacity = 50;
            this.transpCtrl1.Size = new System.Drawing.Size(349, 265);
            this.transpCtrl1.TabIndex = 2;
            this.transpCtrl1.Text = "transpCtrl1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 47);
            this.button1.TabIndex = 3;
            this.button1.Text = "点击";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UseTranspCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WinformTransparent.Properties.Resources.Snipaste_20220712_091629;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.transpCtrl1);
            this.Controls.Add(this.label1);
            this.Name = "UseTranspCtrl";
            this.Text = "UseTranspCtrl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private TranspCtrl transpCtrl1;
        private System.Windows.Forms.Button button1;
    }
}