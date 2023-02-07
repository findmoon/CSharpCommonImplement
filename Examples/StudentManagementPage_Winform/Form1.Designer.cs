namespace StudentManagementPage_Winform
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.studentListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.studentAgeNum = new System.Windows.Forms.NumericUpDown();
            this.studentFemaleRadio = new System.Windows.Forms.RadioButton();
            this.studentMaleRadio = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.studentNameTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.studentIdTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.updateBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.majorsDGV = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.studentAgeNum)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.majorsDGV)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.studentListBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 515);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "学生列表";
            // 
            // studentListBox
            // 
            this.studentListBox.FormattingEnabled = true;
            this.studentListBox.ItemHeight = 17;
            this.studentListBox.Location = new System.Drawing.Point(9, 22);
            this.studentListBox.Name = "studentListBox";
            this.studentListBox.Size = new System.Drawing.Size(238, 480);
            this.studentListBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.studentAgeNum);
            this.groupBox2.Controls.Add(this.studentFemaleRadio);
            this.groupBox2.Controls.Add(this.studentMaleRadio);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.studentNameTxt);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.studentIdTxt);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(287, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(236, 284);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "学生详情";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(69, 241);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(141, 25);
            this.comboBox1.TabIndex = 15;
            // 
            // studentAgeNum
            // 
            this.studentAgeNum.Location = new System.Drawing.Point(69, 156);
            this.studentAgeNum.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.studentAgeNum.Name = "studentAgeNum";
            this.studentAgeNum.Size = new System.Drawing.Size(141, 23);
            this.studentAgeNum.TabIndex = 14;
            this.studentAgeNum.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // studentFemaleRadio
            // 
            this.studentFemaleRadio.AutoSize = true;
            this.studentFemaleRadio.Location = new System.Drawing.Point(118, 120);
            this.studentFemaleRadio.Name = "studentFemaleRadio";
            this.studentFemaleRadio.Size = new System.Drawing.Size(38, 21);
            this.studentFemaleRadio.TabIndex = 13;
            this.studentFemaleRadio.Text = "女";
            this.studentFemaleRadio.UseVisualStyleBackColor = true;
            // 
            // studentMaleRadio
            // 
            this.studentMaleRadio.AutoSize = true;
            this.studentMaleRadio.Checked = true;
            this.studentMaleRadio.Location = new System.Drawing.Point(74, 120);
            this.studentMaleRadio.Name = "studentMaleRadio";
            this.studentMaleRadio.Size = new System.Drawing.Size(38, 21);
            this.studentMaleRadio.TabIndex = 12;
            this.studentMaleRadio.TabStop = true;
            this.studentMaleRadio.Text = "男";
            this.studentMaleRadio.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "性别：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 242);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "专业：";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(69, 197);
            this.textBox3.Name = "textBox3";
            this.textBox3.PlaceholderText = "专业总成绩，没有可不填";
            this.textBox3.Size = new System.Drawing.Size(141, 23);
            this.textBox3.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "成绩：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "年龄：";
            // 
            // studentNameTxt
            // 
            this.studentNameTxt.Location = new System.Drawing.Point(69, 77);
            this.studentNameTxt.Name = "studentNameTxt";
            this.studentNameTxt.Size = new System.Drawing.Size(141, 23);
            this.studentNameTxt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "姓名：";
            // 
            // studentIdTxt
            // 
            this.studentIdTxt.Location = new System.Drawing.Point(69, 32);
            this.studentIdTxt.Name = "studentIdTxt";
            this.studentIdTxt.Size = new System.Drawing.Size(141, 23);
            this.studentIdTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "学号：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.deleteBtn);
            this.groupBox3.Controls.Add(this.updateBtn);
            this.groupBox3.Controls.Add(this.addBtn);
            this.groupBox3.Location = new System.Drawing.Point(546, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(160, 162);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "学生操作";
            // 
            // deleteBtn
            // 
            this.deleteBtn.Location = new System.Drawing.Point(37, 122);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(75, 23);
            this.deleteBtn.TabIndex = 2;
            this.deleteBtn.Text = "删除";
            this.deleteBtn.UseVisualStyleBackColor = true;
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(37, 72);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(75, 23);
            this.updateBtn.TabIndex = 1;
            this.updateBtn.Text = "更新";
            this.updateBtn.UseVisualStyleBackColor = true;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(37, 22);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 0;
            this.addBtn.Text = "新建";
            this.addBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.majorsDGV);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Location = new System.Drawing.Point(287, 315);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(556, 212);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "专业管理";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(441, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "删除选中项";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(441, 170);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 29);
            this.button2.TabIndex = 1;
            this.button2.Text = "保存";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // majorsDGV
            // 
            this.majorsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.majorsDGV.Location = new System.Drawing.Point(6, 22);
            this.majorsDGV.Name = "majorsDGV";
            this.majorsDGV.RowTemplate.Height = 25;
            this.majorsDGV.Size = new System.Drawing.Size(413, 177);
            this.majorsDGV.TabIndex = 3;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button4);
            this.groupBox5.Location = new System.Drawing.Point(546, 191);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(299, 118);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "非Windows认证时SQL Server用户登陆";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(206, 82);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "连接";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 539);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Student Information";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.studentAgeNum)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.majorsDGV)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private ListBox studentListBox;
        private GroupBox groupBox2;
        private ComboBox comboBox1;
        private NumericUpDown studentAgeNum;
        private RadioButton studentFemaleRadio;
        private RadioButton studentMaleRadio;
        private Label label7;
        private Label label5;
        private TextBox textBox3;
        private Label label4;
        private Label label3;
        private TextBox studentNameTxt;
        private Label label2;
        private TextBox studentIdTxt;
        private Label label1;
        private GroupBox groupBox3;
        private Button addBtn;
        private Button deleteBtn;
        private Button updateBtn;
        private GroupBox groupBox4;
        private DataGridView majorsDGV;
        private Button button1;
        private Button button2;
        private GroupBox groupBox5;
        private Button button4;
    }
}