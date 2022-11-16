using CMControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlRound
{
    /// <summary>
    ///通过 base.CreateParams.ExStyle |= 0x00080000; 扩展样式，启用 WS_EX_LAYERED ，相当于在原有的控件Region基础上重新绘制一个Layer层，原有窗体内容均被覆盖，需要获取其子控件并绘制到新的layer层   
    /// </summary>
    public class RoundedFormTest : RoundedForm
    {
        private Panel panel1;
        private Button button1;
        private Button button2;
        private CheckBox checkBox1;
        private TreeView treeView1;
        private TransparentsControl transparentsControl1;
        //private Timer drawTimer = new Timer();

        #region 构造函数
        public RoundedFormTest()
        {
            InitializeComponent();
            // 居中无效
            //StartPosition = FormStartPosition.CenterScreen;
            Load += RoundedFormTest_Load;
        }

        private void RoundedFormTest_Load(object sender, EventArgs e)
        {
            // 有效
            Location = new Point(300, 300);
          
        }

        #endregion


        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.transparentsControl1 = new CustomControlRound.TransparentsControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.transparentsControl1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(73, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // transparentsControl1
            // 
            this.transparentsControl1.BackColor = System.Drawing.Color.Transparent;
            this.transparentsControl1.Location = new System.Drawing.Point(51, 62);
            this.transparentsControl1.Name = "transparentsControl1";
            this.transparentsControl1.Size = new System.Drawing.Size(75, 23);
            this.transparentsControl1.TabIndex = 1;
            this.transparentsControl1.Text = "transparentsControl1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(26, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(73, 240);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(195, 215);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 16);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(195, 262);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 97);
            this.treeView1.TabIndex = 3;
            // 
            // RoundedFormTest
            // 
            this.ClientSize = new System.Drawing.Size(335, 371);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel1);
            this.FixedSize = false;
            this.Name = "RoundedFormTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你好1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你好呀2");
        }
    }


   
}
