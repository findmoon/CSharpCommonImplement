using CMCode.Register;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTypeRegisterTest
{
    public partial class Form1 : Form
    {
        public Form1(string filePath)
        {
            InitializeComponent();

            if (filePath!=null && File.Exists(filePath))
            {
                richTextBox1.Text = File.ReadAllText(filePath);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filetypeRegInfo = new FileTypeRegInfo(fileTypeTxt.Text,Application.ExecutablePath);
            // 默认图标
            filetypeRegInfo.Description = Application.ExecutablePath + "打开程序，默认使用程序图标";
            FileTypeRegister.RegisterFileType(filetypeRegInfo);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var filetypeRegInfo = new FileTypeRegInfo(fileTypeTxt.Text, Application.ExecutablePath);
            filetypeRegInfo.Description = Application.ExecutablePath + "打开程序";
            #region 四种显式指定资源管理器中文件图标的方式
            //filetypeRegInfo.IconPath = Path.GetFullPath("../../mytest1.png");
            //filetypeRegInfo.IconPath = Path.GetFullPath("../../mytest2.ico");
            //filetypeRegInfo.IconPath = Application.ExecutablePath + ",0"; // 程序有多个图标时,后面指定索引
            //filetypeRegInfo.IconPath = Application.ExecutablePath; 
            #endregion
            FileTypeRegister.RegisterFileTypeUpdate(filetypeRegInfo);
        }
    }
}
