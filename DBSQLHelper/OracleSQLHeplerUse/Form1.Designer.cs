namespace OracleSQLHeplerUse
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.pwdTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.userTxt = new System.Windows.Forms.TextBox();
            this.dbTxt = new System.Windows.Forms.TextBox();
            this.serverTxt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(124, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "密码(Password)：";
            // 
            // pwdTxt
            // 
            this.pwdTxt.Location = new System.Drawing.Point(236, 177);
            this.pwdTxt.Name = "pwdTxt";
            this.pwdTxt.PasswordChar = '*';
            this.pwdTxt.Size = new System.Drawing.Size(217, 23);
            this.pwdTxt.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 17);
            this.label3.TabIndex = 15;
            this.label3.Text = "用户名(User Name)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "数据库或服务ID(DB or SID)：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "Oracle服务器(ip or name)：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userTxt
            // 
            this.userTxt.Location = new System.Drawing.Point(236, 138);
            this.userTxt.Name = "userTxt";
            this.userTxt.Size = new System.Drawing.Size(217, 23);
            this.userTxt.TabIndex = 12;
            // 
            // dbTxt
            // 
            this.dbTxt.Location = new System.Drawing.Point(236, 100);
            this.dbTxt.Name = "dbTxt";
            this.dbTxt.Size = new System.Drawing.Size(217, 23);
            this.dbTxt.TabIndex = 11;
            // 
            // serverTxt
            // 
            this.serverTxt.Location = new System.Drawing.Point(236, 54);
            this.serverTxt.Name = "serverTxt";
            this.serverTxt.Size = new System.Drawing.Size(217, 23);
            this.serverTxt.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(195, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 52);
            this.button1.TabIndex = 9;
            this.button1.Text = "连接登陆";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(195, 303);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(164, 52);
            this.button2.TabIndex = 18;
            this.button2.Text = "其他测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 381);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pwdTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userTxt);
            this.Controls.Add(this.dbTxt);
            this.Controls.Add(this.serverTxt);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label4;
        private TextBox pwdTxt;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox userTxt;
        private TextBox dbTxt;
        private TextBox serverTxt;
        private Button button1;
        private Button button2;
    }
}